using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerInstance : MonoBehaviour
{
    //Function: Gets input from an individual Controller and sends it to InputMaster as 2 separate joysticks (with accompanying button commands)

    //Objects & Components:
    private PlayerInput playerInput; //The playerInput receiver on this object

    //Status Vars:
    private bool isKeyboard; //Used to nullify the one inevitable keyboard instance that'll get in here

    private void Awake()
    {
        //Get Objects & Components:
        playerInput = GetComponent<PlayerInput>();
        
        //Initial Safety Check:
        if (playerInput.currentControlScheme == "Keyboard&Mouse") isKeyboard = true; //Check if instance is not a controller
        if (!isKeyboard) print("Controller joined"); //Only really worry about non-keyboard controllers
    }

    //CONNECTION METHODS:
    public void OnDeviceLost()
    {
        //Function: Called when device disconnects, should disconnect both associated players

        print("Controller left");

        //Cleanup:
        Destroy(gameObject); //Destroy this object
    }

    //INPUT METHODS:
    public void OnJoystick1()
    {

    }
}
