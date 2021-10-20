using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputMaster : MonoBehaviour
{
    //Function: Tracks player instances, spawns new players when new inputs are detected, removes players when inputs are disconnected

    //Classes, Structs & Enums:
    public class Controller
    {
        //One of these will get spawned for every new controller that enters the game

        public Joystick stick1; //First stick on this controller
        public Joystick stick2; //Second stick on this controller
    }
    public class Joystick
    {
        //One of these will be created (under a controller) every time a new joystick (or WASD set) gets used

        public float timeSinceLastInput; //Time (in seconds) since player last touched joystick, used to despawn characters

    }

    //Objects & Components:
    public static InputMaster inputMaster;
    public GameObject playerPrefab;
    public List<Joystick> activeKeyboardPlayers = new List<Joystick>(); //List of all active keyboard players currently in scene
    public List<Controller> activeControllers = new List<Controller>(); //List of all active controllers currently in scene

    //LOOP METHODS:
    private void Awake()
    {
        if (inputMaster == null)
        {
            DontDestroyOnLoad(gameObject);
            inputMaster = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    
}
