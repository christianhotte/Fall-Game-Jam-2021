using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using HotteStuff;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour, IControllable
{
    //Objects & Components:
    private Transform body; //The bug body (SHOULD BE NAMED "Body")
    internal Collider headBox; //This bug's head collider
    internal Collider bodyBox; //This bug's body collider

    //Bugstats:
    [Header("Stats:")]
    public float baseMaxSpeed; //How fast the bug go
    public float baseAccel;    //How fast bug accelerates to max speed
    public float baseDrag;     //How fast the bug stops when it is not being controlled
    public float baseMaxRotationSpeed; //How fast bug body lerps to direction of motion (speed is also factored in)
    public float baseSize;     //How big the bug be (scale of bug in Unity units)
    public float baseStrength; //How hard bug pushes other bugs around
    
    [Header("BugStats:")]
    internal float speedModifier; //Additional (or subtractive) speed
    internal float accelModifier; //Additional (or subtractive) acceleration
    internal float rotationSpeedModifier; //Additional (or subtractive) rotation speed
    internal float sizeModifier; //Additional (or subtractive) bug size
    internal float strengthModifier; //Additional (or subtractive) bugstrength
    internal float knockbackResistModifier = 1; //[0-1] Amount bug resists being bumped (percentage decrease in incoming bumps)
    
    [Header("Movement Stuff:")]
    public float bugStopSnap; //How close to Vector2.zero bug velocity must be for bug to stop (should just be a really small number)
    [Range(0, 1)] public float bumpRecoilMultiplier; //Determines how much of a bump impacts the bug who initiates it
    public float bounceBaseForce; //How much force is naturally applied to bug when they bump into things (and other bugs)
    public float maxSpeed; //Maximum acceleration a player can have
    public AnimationCurve speedAccelCurve; //Determines bug acceleration (depending on how fast bug is going out of max speed)
    public AnimationCurve speedRotSpeedCurve; //Determines how fast bug can turn (depending on how fast bug is going)
    public AnimationCurve speedBumpCurve; //Determines how speed adds power to a bug bump (power multiplier based on speed number)
    [Space()]
    public Vector3 bugDeathTumbleVector; //Vector determining force applied to bug when dying (and ragdolling)

    [Header("Score Stuff:")]
    public TMP_Text pointCountUI;
    public int pointCountValue = 0; // J - Score counter
    float killVsSuicideTimer = 0; // J - timer that determines the time it takes to forget lastBugTouched to determine kill vs suicide

    //Memory Vars:
    internal PlayerController lastBugTouched; //Stores the last bug this bug bugged (resets after given amount of time
    internal Vector2 velocity;       //How fast da bug is going
    private Vector2 currentJoystick; //Where the joystick was last time it changed
    private bool currentButton;      //State the button was last time it changed

    //BugDie Stuff:
    internal bool bugDead = false;   // J - triggers respawn timer
    private RaycastHit hit; //Container to store death raycasts
    private static int deathZoneLayerMask = 1 << 8;  //Layermask for bugDie procedure

    //Alice Dash Code Shit
    internal BugDash BugDash;

    //LOOP METHODS:
    private void Awake()
    {
        //Get Objects and Components:
        body = transform.Find("Body"); //Get body
        headBox = body.GetChild(0).GetComponent<Collider>();
        bodyBox = body.GetChild(1).GetComponent<Collider>();
        BugDash = GetComponent<BugDash>();


        //Stats:
        transform.localScale = new Vector3(baseSize, baseSize, baseSize); //Set initial scale
    }
    private void Update()
    {
        //Bug Movement:
        if (!BugDash.isDash)
        {
            MoveBug();
            RotateBug();
        }

        //Check Last Bug Touched Expiration:
        if (lastBugTouched != null) // If bug touches this bug, forget it after 5 seconds to determine kill or suicide point
        {
            killVsSuicideTimer += Time.deltaTime;
            if (killVsSuicideTimer >= 3) lastBugTouched = null;
        }
    }
    private void FixedUpdate()
    {
        if (!bugDead) CheckForBugDie(); //Check for bug death if bug is alive
    }


    //MOVEMENT METHODS:
    private void MoveBug()
    {
        //Function: Moves bug based on current velocity

        //Compute change in velocity:
        Vector2 targetVelocity;
        float accelFactor;
        if (currentJoystick != Vector2.zero) //Bug being moved by player
        {
            targetVelocity = currentJoystick * (baseMaxSpeed + speedModifier); //Get target velocity for bug
            accelFactor = (baseAccel + accelModifier) * speedAccelCurve.Evaluate(GetNormalizedSpeed()); //Apply acceleration curve to accel speed
        }
        else //No joystick input
        {
            targetVelocity = Vector2.zero;
            accelFactor = baseDrag;
        }
        velocity = Vector2.Lerp(velocity, targetVelocity, accelFactor * Time.deltaTime);
        
        //Check for stop:
        if (currentJoystick == Vector2.zero && velocity.sqrMagnitude <= bugStopSnap) //Bug is decelerating
        {
            velocity = Vector2.zero;
        }
        if (velocity == Vector2.zero) return; //We're done here

        //Check for max velocity:
        if (velocity.magnitude > maxSpeed) velocity = velocity.normalized * maxSpeed;

        //Apply velocity to bug position:
        Vector3 realVelocity = new Vector3(velocity.x, 0, velocity.y); //Rearrange velocity to fit in world
        transform.Translate(realVelocity * Time.deltaTime); //Move bug by velocity (along x/y axis)

    }
    private void RotateBug()
    {
        //Function: Rotates bug body to match direction of travel (scales based on speed)

        if (currentJoystick == Vector2.zero) return; //Do not rotate bug if it is stationary
        
        //Get stuff:
        Vector3 targetRotation = new Vector3(0,currentJoystick.x, currentJoystick.y);
        float turnSpeedModifier = speedRotSpeedCurve.Evaluate(GetNormalizedSpeed());
        float rotationStrength = baseMaxRotationSpeed * turnSpeedModifier * Time.deltaTime;

        //Rotate bug:
        float angle = (Mathf.Atan2(targetRotation.y, targetRotation.z) * Mathf.Rad2Deg);
        Quaternion oog = Quaternion.AngleAxis(angle, Vector3.up);
        body.rotation = Quaternion.Slerp(body.rotation, oog, rotationStrength + rotationSpeedModifier);
    }


    //BUG METHODS:
    public void BugBump(Collider other)
    {
        //Function: Called when this bug's head hits any part of another bug, determines how hard it hits the thing
        //NOTE: Bumps the other bug based on bugstats

        //Get other bug:
        PlayerController otherBug = other.GetComponentInParent<PlayerController>();

        //Determine modifiers:
        float hitStrength = baseStrength + strengthModifier; //Get normal hit strength
        hitStrength *= speedBumpCurve.Evaluate(GetNormalizedSpeed());

        //Determine force:
        Vector2 hitDirection = Vector2.up.Rotate(body.eulerAngles.y);
        hitDirection = Vector2.Reflect(hitDirection, Vector2.left);
        Vector2 hitForce = hitDirection * hitStrength; //Apply hitforce to direction
        
        //Add force to bugs:
        otherBug.velocity += hitForce * otherBug.knockbackResistModifier; //Bump other bug
        velocity -= hitForce * bumpRecoilMultiplier * knockbackResistModifier; //Bump this bug

        //Cleanup:
        lastBugTouched = otherBug;
    }
    public void BugBounce(Collider other)
    {
        //Function: Called when this bug's body hits the body of another bug
        //NOTE: Gently bounces this bug away from the other bug

        //Get other bug:
        PlayerController otherBug = other.GetComponentInParent<PlayerController>();

        //Determine force:
        float hitStrength = bounceBaseForce; //Get hit strength
        Vector2 hitDirection = Vector2.up.Rotate(body.eulerAngles.y);
        hitDirection = Vector2.Reflect(hitDirection, Vector2.left);
        Vector2 hitForce = hitDirection * hitStrength; //Apply hitforce to direction

        //Add force to bugs:
        otherBug.velocity += hitForce * otherBug.knockbackResistModifier; //Bump other bug
        velocity -= hitForce * bumpRecoilMultiplier * knockbackResistModifier; //Bump this bug

        //Cleanup:
        lastBugTouched = otherBug;
    }
    public void BugDie()
    {
        //Function: Called when the bug die
        //function is called from the bug Die class that needs to be on an object, requires a plane tagged "Death" just below stump level

        //Mark Dead:
        bugDead = true;
        currentJoystick = Vector2.zero; //Cancel potential phantom inputs
        currentButton = false;          //Cancel potential phantom inputs

        //Determine Cause of Die:
        if (lastBugTouched == null) { pointCountValue -= 1; pointCountUI.text = pointCountValue.ToString(); }// Suicide ADD UI CHANGE PLEASE
        else { lastBugTouched.pointCountValue += 5; lastBugTouched.pointCountUI.text = pointCountValue.ToString(); } // Give other player a point

        //Make Bug Look Dead:
        headBox.gameObject.SetActive(false); //Deactivate inter-bug collision
        bodyBox.gameObject.SetActive(false); //Deactivate inter-bug collision
        GetComponent<Rigidbody>().isKinematic = false;  //Activate ragbug rigidBody
        GetComponent<CapsuleCollider>().enabled = true; //Activate ragbug collider
        GetComponent<Rigidbody>().AddForce(bugDeathTumbleVector); //Make bug tumble

        //Begin Adaptation and Respawn Process:
        DeathHandler.deathHandler.BugDiedProcedure(this); //Indicate that this bug has died
    }
    public void BugResurrect()
    {
        //Function: Used on a copy of a dead bug to undie it

        //Undo deadness on playerController:
        bugDead = false;
        lastBugTouched = null;

        //Undo deadness caused by Keegan:
        headBox.gameObject.SetActive(true);
        bodyBox.gameObject.SetActive(true);
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().enabled = false;

        //Reset memory/tracker variables:
        velocity = Vector2.zero;
        currentJoystick = Vector2.zero;
        currentButton = false;
    }
    public void ChangeBugSize(float newSize)
    {
        //Function: Changes size of bug

        //@KEEGAN: I just looked at this and realized how legendarily smooth brain it is so I changed it and if it breaks something and I forget why that's why this note is here
        sizeModifier = newSize;
        float setSize = baseSize + sizeModifier;
        transform.localScale = new Vector3(setSize, setSize, setSize); //Set initial scale
        //NOTE: Add thing to affect bug Y position
    }
    public void ResetBugSize()
    {
        //Function: Reverts size of bug to base size

        sizeModifier = 0;
        transform.localScale = new Vector3(baseSize, baseSize, baseSize);
    }


    //INPUT METH:
    public void ReceiveJoystick(Vector2 input)
    {
        //Function: Called by input manager when sending commands from Player to IControllable pawn (this)

        //Check Validity:
        if (bugDead) return; //DEATH LOCKOUT: Dead bugs make no moves
        //@ALICE: If you want bug to not be able to steer while dashing, you can put something here to check for dash

        //Record Joystick Input:
        currentJoystick = input; //Update memory (nothing fancy)
    }
    public void ReceiveButton(bool pressed)
    {
        //Function: Called by input manager when sending commands from Player to IControllable pawn (this)

        //Check Validity:
        if (bugDead) return; //DEATH LOCKOUT: Dead bugs make no moves
        //@ALICE: Same as with joystick

        //Compare Button State:
        if (pressed != currentButton)
        {
            if (pressed) ButtonDown();
            else ButtonUp();
            currentButton = pressed; //Update memory
        }
    }
    public void DestroyPawn()
    {
        Destroy(gameObject); //Destroy this pawn (NOTE: Maybe add stuff if this causes problems)
    }
    private void ButtonDown()
    {
        //Called (by this script) when ACTION/ABILITY button is pressed

    }
    private void ButtonUp()
    {
        //Called (by this script) when ACTION/ABILITY button is released

    }


    //UTILITY FUNCTIONS:
    private float GetNormalizedSpeed()
    {
        //Function: Returns speed as percentage (0-1), with 1 being the player's current maximum possible speed

        return Mathf.InverseLerp(0, baseMaxSpeed, velocity.magnitude);
    }
    private void CheckForBugDie()
    {
        //Function: Called during FixedUpdate to check if bug is die

        deathZoneLayerMask = ~deathZoneLayerMask; //Weird layermask shit I dunno
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 5, deathZoneLayerMask))
        {
            if (hit.collider.CompareTag("Death")) BugDie(); //Kill bug if death zone is found
        }
    }
}
