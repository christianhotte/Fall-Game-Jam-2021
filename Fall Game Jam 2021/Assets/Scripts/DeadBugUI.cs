using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBugUI : MonoBehaviour, IControllable
{
    //Function: Player object which shows up when (and where) a bug dies

    //Handled Vars:
    internal PlayerController deadBug; //The bug this UI is referring to
    internal float timeSinceDeath; //Time (in seconds) since bug died
    internal bool markedForDisposal; //Whether or not this UI element is marked for destruction (when convenient)

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
