using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBugUI : MonoBehaviour, IControllable
{
    //Function: Player object which shows up when (and where) a bug dies


    //ICONTROLLABLE METHODS:
    public void ReceiveJoystick(Vector2 input)
    {
        //Function: Called by input manager when sending commands from Player to IControllable pawn (this)

    }
    public void ReceiveButton(bool pressed)
    {
        //Function: Called by input manager when sending commands from Player to IControllable pawn (this)

    }
    public void DestroyPawn()
    {
        Destroy(gameObject); //Destroy this pawn (NOTE: Maybe add stuff if this causes problems)
    }
}
