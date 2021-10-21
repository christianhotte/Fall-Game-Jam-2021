using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HotteStuff;
using UnityEngine.UI;
using TMPro;

public class DeadBugUI : MonoBehaviour, IControllable
{
    //Function: Player object which shows up when (and where) a bug dies

    //Objects & Components:
    internal InputMaster.Player currentPlayer; //The player controlling this UI
    private BugAdaptations adaptationManager; //The adaptation script on this UI's associated bug
    private Transform canvas; //The UI canvas
    private readonly System.Random rnd = new System.Random(); //Get random seed
    private Transform pointerHolder; //Object which contains the pointer
    private TextMeshPro abilityText; //Text which displays name of selected ability

    //Settings:
    [Header("Settings:")]
    public int numberOfChoices; //Number of ability choices player will be given
    public float pointerSnapSpeed; //How fast pointer follows joystick

    //Memory Vars:
    internal List<int> abilitySelection = new List<int>(); //List to store the abilities this gives the player
    private Vector2 currentJoystick; //Most recent joystick value detected
    private float joystickAngle; //Angle in which joystick is pointing (if any)

    //Handled Vars:
    internal PlayerController deadBug; //The bug this UI is referring to
    internal float timeSinceDeath; //Time (in seconds) since bug died
    internal bool markedForDisposal; //Whether or not this UI element is marked for destruction (when convenient)

    //SEQUENCE METHODS:
    private void Start()
    {
        //Get Objects:
        adaptationManager = deadBug.GetComponent<BugAdaptations>();
        canvas = transform.GetChild(0);
        pointerHolder = canvas.GetChild(3);
        abilityText = canvas.GetChild(4).GetComponent<TextMeshPro>();

        //Populate & Animate:
        GenerateSelection();
        //canvas.LookAt(Camera.main.transform); //Align canvas to camera
    }
    private void Update()
    {
        //Lerp Pointer Size:

    }

    //SELECTION METHODS:
    private void GenerateSelection()
    {
        //Function: Generates selection of abilities and updates UI accordingly

        //Get Selection:
        abilitySelection = adaptationManager.getAbilityChoices(numberOfChoices); //Get random list of abilities

        //Apply Ability Visuals:
        canvas.GetChild(0).GetChild(0).GetComponent<Image>().sprite = DeathHandler.deathHandler.abilityDataList[abilitySelection[2]].sprite; //Set choice's sprite in appropriate slice
        canvas.GetChild(1).GetChild(0).GetComponent<Image>().sprite = DeathHandler.deathHandler.abilityDataList[abilitySelection[0]].sprite; //Set choice's sprite in appropriate slice
        canvas.GetChild(2).GetChild(0).GetComponent<Image>().sprite = DeathHandler.deathHandler.abilityDataList[abilitySelection[1]].sprite; //Set choice's sprite in appropriate slice
        

    }
    public void SelectAbility(int abilityIndex)
    {
        //Function: Chooses given ability and activates player respawn procedure

        //Add Ability to Player:
        adaptationManager.addAbility(abilityIndex); //Add ability to player
        Debug.Log(DeathHandler.deathHandler.abilityDataList[abilityIndex].name + " added");

        //Continue Respawn Process:
        EndUI(); //End this UI
    }
    public void EndUI()
    {
        //Function: Removes this UI from game and spawns player back in

        DeathHandler.deathHandler.RespawnAtRandomLocation(this); //Respawn bug
        markedForDisposal = true; //Indicate that UI is ready to be destroyed
    }

    //ICONTROLLABLE METHODS:
    public void GivePlayer(InputMaster.Player player)
    {
        currentPlayer = player;
    }
    public void ReceiveJoystick(Vector2 input)
    {
        //Function: Called by input manager when sending commands from Player to IControllable pawn (this)

        currentJoystick = input; //Get directional joystick value
        Vector2 modifiedInput = input.normalized; modifiedInput.x = -modifiedInput.x;
        joystickAngle = Vector2.SignedAngle(Vector2.up, modifiedInput).DeNormalizeAngle(); //Get current angle of joystick

        //Check for pointer:
        if (input != Vector2.zero) //Player is pointing in a direction
        {
            //Make Pointer Visible:
            pointerHolder.gameObject.SetActive(true); //Show pointer when directional input is given
            abilityText.gameObject.SetActive(true); //Show ability text when directional input is given

            //Rotate Pointer to Position:
            float currentRotation = pointerHolder.rotation.eulerAngles.z; //Get current pointer angle
            float targetRotation = Vector2.SignedAngle(Vector2.up, input) + 180; //Set target to joystick angle (do calculation to make sure arrow is in right place)
            float newRotation = Mathf.LerpAngle(currentRotation, targetRotation, pointerSnapSpeed * Time.deltaTime);
            Vector3 newEulers = pointerHolder.eulerAngles; newEulers.z = newRotation; //Create euler angles with desired rotation
            pointerHolder.localRotation = Quaternion.Euler(newEulers); //Set new euler angles

            //Edit UI Text:
            abilityText.text = DeathHandler.deathHandler.abilityDataList[GetCurrentAbilitySelection()].name; //Get name of ability
        }
        else //Player is not pointing in a direction
        {
            pointerHolder.gameObject.SetActive(false); //Hide pointer when no directional input is given
            abilityText.gameObject.SetActive(false); //Hide ability text when no directional input is given
        }
    }
    public void ReceiveButton(bool pressed)
    {
        //Function: Called by input manager when sending commands from Player to IControllable pawn (this)

        if (pressed) //If button has been pressed while joystick is pointed in a direction
        {
            if (currentJoystick != Vector2.zero) //Select ability player is pointing to
            {
                if (!deadBug.bugSuicide) SelectAbility(GetCurrentAbilitySelection()); //Deploy selected ability
                else EndUI(); //Don't allow ability to be chosen if bug died
            }
            else //Select random ability
            {
                if (!deadBug.bugSuicide) SelectAbility(abilitySelection[Random.Range(0, abilitySelection.Count - 1)]);
                else EndUI();
            }
        }
    }
    public void DestroyPawn()
    {
        Destroy(gameObject); //Destroy this pawn (NOTE: Maybe add stuff if this causes problems)
    }


    //UTILITY METHODS:
    private int GetCurrentAbilitySelection()
    {
        //Function: Returns index of ability currently selected by player

        //Setup & Safety Check:
        float sliceDegrees = 360 / numberOfChoices; //Get degrees per slice on the input wheel
        
        //Find Slice with Correct Ability:
        for (int i = 0; i < numberOfChoices; i++) //Iterate through all potential choices
        {
            float sliceStart = i * sliceDegrees;     //Get start of slice in degrees
            float sliceEnd = (i + 1) * sliceDegrees; //Get end of slice in degrees
            if (HotteMath.AngleIsBetween(joystickAngle, sliceStart, sliceEnd)) //Joystick has selected this choice
            {
                return abilitySelection[i]; //Return found ability
            }
        }

        //Something Went Wrong:
        Debug.LogError("GetCurrentAbilitySelection failed for some reason");
        return 0;
    }

    //ANIMATION METHODS:
    
}
