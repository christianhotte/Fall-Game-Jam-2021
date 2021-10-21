using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerInstance : MonoBehaviour
{
    //Function: Gets input from an individual Controller and sends it to InputMaster as 2 separate joysticks (with accompanying button commands)

    //Objects & Components:
    private PlayerInput playerInput; //The playerInput receiver on this object

    //Input Vars:
    internal Vector2 currentJoystick1; //Current state of left joystick input
    internal Vector2 currentJoystick2; //Current state of right joystick input
    internal bool currentButton1;      //Current state of left action button input
    internal bool currentButton2;      //Current state of right action button input

    //Status Vars:
    internal bool isKeyboard; //Used to nullify the one inevitable keyboard instance that'll get in here
    internal InputMaster.Joystick joystick1 = new InputMaster.Joystick(); //Object representing left joystick
    internal InputMaster.Joystick joystick2 = new InputMaster.Joystick(); //Object representing right joystick

    private void Awake()
    {
        //Get Objects & Components:
        playerInput = GetComponent<PlayerInput>();
        
        //Initial Safety Check:
        if (playerInput.currentControlScheme == "Keyboard&Mouse") isKeyboard = true; //Check if instance is not a controller
        if (!isKeyboard) print("Controller joined"); //Only really worry about non-keyboard controllers

        //Establish Connection:
        if (!isKeyboard) //Ignore the keyboard, only connect actual controllers
        {
            //Populate Joystick Information:
            joystick1.controller = this; //Set this script as controller of left joystick
            joystick2.controller = this; //Set this script as controller of right joystick
            joystick1.isLeftJoystick = true;  //Tell left joystick that it is left
            joystick2.isLeftJoystick = false; //Tell right joystick that it is right

            //Introduce New Controller to Database:
            InputMaster.inputMaster.AddNewController(this); //Add this controller to pool of active controllers
        }
    }

    //CONNECTION METHODS:
    public void OnDeviceLost()
    {
        //Function: Called when device disconnects, despawns existing player pawns and removes associated joysticks from game

        print("Controller left");
        if (!isKeyboard) InputMaster.inputMaster.RemoveController(this); //Remove controller references from running database
        Destroy(gameObject); //Destroy controller object
    }

    //INPUT METHODS:
    public void OnJoystick1(InputAction.CallbackContext context)
    {
        currentJoystick1 = context.ReadValue<Vector2>();
    }
    public void OnJoystick2(InputAction.CallbackContext context)
    {
        currentJoystick2 = context.ReadValue<Vector2>();
    }
    public void OnAbility1(InputAction.CallbackContext context)
    {
        //currentButton1 = context.ReadValueAsButton();
    }
    public void OnAbility2(InputAction.CallbackContext context)
    {
        //currentButton2 = context.ReadValueAsButton();
    }
}
