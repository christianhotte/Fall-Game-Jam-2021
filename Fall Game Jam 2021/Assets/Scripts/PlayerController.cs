using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Objects & Components:
    private Transform body; //The bug body (SHOULD BE NAMED "Body")

    //Bugstats:
    [Header("BugStats:")]
    public float baseMaxSpeed; //How fast the bug go
    public float baseAccel; //How fast bug accelerates to max speed
    public float baseDrag; //How fast the bug stops when it is not being controlled
    public float baseMaxRotationSpeed; //How fast bug body lerps to direction of motion (speed is also factored in)
    //Experimental stuff:
    public float baseSize;     //How big the bug be (scale of bug in Unity units)
    public float baseWeight;   //How hard bug is to push around
    [Header("Movement Stuff:")]
    public float bugStopSnap; //How close to Vector2.zero bug velocity must be for bug to stop (should just be a really small number)
    public AnimationCurve speedAccelCurve; //Determines bug acceleration (depending on how fast bug is going out of max speed)
    public AnimationCurve speedRotSpeedCurve; //Determines how fast bug can turn (depending on how fast bug is going)

    //Memory Vars:
    private Vector2 currentJoystick; //Where the joystick was last time it changed
    private bool currentButton;      //State the button was last time it changed

    internal Vector2 velocity; //How fast da bug is going

    //GAME METHODS:
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
            targetVelocity = currentJoystick * baseMaxSpeed; //Get target velocity for bug
            accelFactor = baseAccel * speedAccelCurve.Evaluate(GetNormalizedSpeed()); //Apply acceleration curve to accel speed
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
        body.rotation = Quaternion.Slerp(body.rotation, oog, rotationStrength);
    }

    //INPUT METH:
    public void OnMove(InputAction.CallbackContext context)
    {
        //Temporary joystick functionality
        ReceiveJoystick(context.ReadValue<Vector2>());
    }
    public void OnFire(InputAction.CallbackContext context)
    {
        //Temporary button functionality
        if (context.performed) ReceiveButton(true);
        else ReceiveButton(false);
    }
    public void ReceiveJoystick(Vector2 input)
    {
        //Should be the normalized joystick value

        //Cleanup:
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
