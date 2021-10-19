using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMaster : MonoBehaviour
{
    //Function: Tracks player instances, spawns new players when new inputs are detected, removes players when inputs are disconnected

    //Classes, Structs & Enums:
    public class Controller
    {
        //One of these will get spawned for every new controller that enters the game

    }
    public class Joystick
    {
        //One of these will be created (under a controller) every time a new joystick gets used

    }

    //Objects & Components:
    public GameObject playerPrefab;
    public List<Controller> activeControllers = new List<Controller>(); //List of all active controllers currently in scene
}
