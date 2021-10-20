using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using HotteStuff;

public class PlayerController : MonoBehaviour
{
    //Objects & Components:
    private Transform body; //The bug body (SHOULD BE NAMED "Body")

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
    [Header("Debug Stuff:")]
    public bool useDebugInput;

    //Memory Vars:
    private Vector2 currentJoystick; //Where the joystick was last time it changed
    private bool currentButton;      //State the button was last time it changed
    internal Vector2 velocity;       //How fast da bug is going

    //Game Vars:
    internal PlayerController lastBugTouched; //Stores the last bug this bug bugged

    //LOOP METHODS:
    private void Awake()
    {
        //Get Objects and Components:
        body = transform.Find("Body"); //Get body

        //Stats:
        transform.localScale = new Vector3(baseSize, baseSize, baseSize); //Set initial scale
    }
    private void Update()
    {
        //Bug Movement:
        MoveBug();
        RotateBug();
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

    }
    public void ChangeBugSize(float newSize)
    {
        //Changes size of bug

        sizeModifier = newSize;
        transform.localScale = new Vector3(newSize, newSize, newSize); //Set initial scale
        //NOTE: Add thing to affect bug Y position
    }
    public void ResetBugSize()
    {
        //Reverts size of bug to base size
        sizeModifier = 0;
        transform.localScale = new Vector3(baseSize, baseSize, baseSize);
    }

    //INPUT METH:
    /*public void OnMove(InputAction.CallbackContext context)
    {
        //Temporary joystick functionality
        if (useDebugInput) ReceiveJoystick(context.ReadValue<Vector2>());
    }
    public void OnFire(InputAction.CallbackContext context)
    {
        //Temporary button functionality
        if (!useDebugInput) return;
        if (context.performed) ReceiveButton(true);
        else ReceiveButton(false);
    }*/
    public void ReceiveJoystick(Vector2 input)
    {
        //Should be the normalized joystick value

        currentJoystick = input; //Update memory
    }
    public void ReceiveButton(bool pressed)
    {
        //Send button-based signal
        if (pressed != currentButton)
        {
            if (pressed) ButtonDown();
            else ButtonUp();
            currentButton = pressed; //Update memory
        }
    }
    private void ButtonUp()
    {
        //Called when button is released

    }
    private void ButtonDown()
    {
        //Called when button is pressed

    }

    //UTILITY FUNCTIONS:
    private float GetNormalizedSpeed()
    {
        //Function: Returns speed as percentage (0-1), with 1 being the player's current maximum possible speed

        return Mathf.InverseLerp(0, baseMaxSpeed, velocity.magnitude);
    }
}
