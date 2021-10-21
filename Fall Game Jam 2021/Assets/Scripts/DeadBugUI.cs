using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HotteStuff;

public class DeadBugUI : MonoBehaviour, IControllable
{
    //Function: Player object which shows up when (and where) a bug dies

    //Objects & Components:
    private BugAdaptations adaptationManager; //The adaptation script on this UI's associated bug

    //Settings:
    [Header("Settings:")]
    public int numberOfChoices; //Number of ability choices player will be given

    //Memory Vars:
    internal List<int> abilitySelection = new List<int>(); //List to store the abilities this gives the player
    private Vector2 currentJoystick; //Most recent joystick value detected

    //Handled Vars:
    internal PlayerController deadBug; //The bug this UI is referring to
    internal float timeSinceDeath; //Time (in seconds) since bug died
    internal bool markedForDisposal; //Whether or not this UI element is marked for destruction (when convenient)

    //SEQUENCE METHODS:
    private void Start()
    {
        adaptationManager = deadBug.GetComponent<BugAdaptations>();
        GenerateSelection();
    }

    //SELECTION METHODS:
    private void GenerateSelection()
    {
        //Function: Generates selection of abilities and updates UI accordingly

        //Get Selection:
        abilitySelection = adaptationManager.getAbilityChoices(numberOfChoices); //Get random list of abilities
    }
    public void SelectAbility(int abilityIndex)
    {
        //Function: Chooses given ability and activates player respawn procedure

        //Add Ability to Player:
        adaptationManager.addAbility(abilityIndex); //Add ability to player
        Debug.Log("Ability added " + abilityIndex);

        //Continue Respawn Process:
        DeathHandler.deathHandler.RespawnAtRandomLocation(this); //Respawn bug
        markedForDisposal = true; //Indicate that UI is ready to be destroyed
    }

    //ICONTROLLABLE METHODS:
    public void ReceiveJoystick(Vector2 input)
    {
        //Function: Called by input manager when sending commands from Player to IControllable pawn (this)

        currentJoystick = input; //Directly log input vector
        //NOTE: ADD THING HERE THAT MOVES UI WHEEL ELEMENT AROUND
    }
    public void ReceiveButton(bool pressed)
    {
        //Function: Called by input manager when sending commands from Player to IControllable pawn (this)

        if (pressed && currentJoystick != Vector2.zero) //If button has been pressed while joystick is pointed in a direction
        {
            //Determine which ability to select:
            float sliceDegrees = 360 / numberOfChoices; //Get degrees per slice on the input wheel
            float joystickAngle = Vector2.Angle(Vector2.up, currentJoystick); //Get current angle of joystick
            for (int i = 0; i < numberOfChoices; i++) //Iterate through all potential choices
            {
                float sliceStart = i * sliceDegrees;     //Get start of slice in degrees
                float sliceEnd = (i + 1) * sliceDegrees; //Get end of slice in degrees
                //if (HotteMath.)
            }
        }
    }
    public void DestroyPawn()
    {
        Destroy(gameObject); //Destroy this pawn (NOTE: Maybe add stuff if this causes problems)
    }
}
