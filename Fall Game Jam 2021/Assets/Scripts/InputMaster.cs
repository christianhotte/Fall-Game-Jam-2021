using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using TMPro;

public class InputMaster : MonoBehaviour
{
    //Function: Tracks player instances, spawns new players when new inputs are detected, removes players when inputs are disconnected

    //Classes, Structs & Enums:
    [System.Serializable] public class Joystick : IInputMethod
    {
        //Joysticks (usually 2 per controller)

        //Objects:
        public Player attachedPlayer;         //Player instance object this joystick is currently controlling
        public ControllerInstance controller; //Object representing the controller this joystick is attached to

        //Stats:
        public bool isActive = false;
        public bool isLeftJoystick = false; //Determines which inputs this joystick references.  If false, this is the Right joystick

        //CORE METHODS:
        public void GivePlayer(Player player)
        {
            //Called to initialize an instance of this scheme with a player

            attachedPlayer = player; //Establish dependency
            isActive = true; //Indicate that this input method is now being used
        }
        public void RemovePlayer()
        {
            //Called to clear the player from this control scheme

            attachedPlayer = null; //Cut ties
            isActive = false; //Indicate that this input method is no longer being used
        }

        //INPUT METHODS:
        public Vector2 CheckMoveInput()
        {
            if (isLeftJoystick) return controller.currentJoystick1; //Return controller's current left joystick value
            else return controller.currentJoystick2; //Return controller's current right joystick value
        }
        public bool CheckAbilityInput()
        {
            if (isLeftJoystick) return controller.currentButton1; //Return controller's current left ability button value
            else return controller.currentButton2; //Return controller's current right ability button value
        }
    }
    [System.Serializable] public class KeyboardInstance : IInputMethod
    {
        //Keyboard fuckery

        //Buttons for this virtual keyboard controller
        public string[] buttons = new string[5]; //Order goes UP, LEFT, DOWN, RIGHT, ABILITY

        //Objects:
        internal Player attachedPlayer;

        //Stats & Mem Vars:
        public bool isActive = false;

        //CORE METHODS:
        public void GivePlayer(Player player)
        {
            //Called to initialize an instance of this scheme with a player
            
            attachedPlayer = player; //Establish dependency
            isActive = true; //Indicate that this input method is now being used
        }
        public void RemovePlayer()
        {
            //Called to clear the player from this control scheme

            attachedPlayer = null; //Cut ties
            isActive = false; //Indicate that this input method is no longer being used
        }

        //INPUT METHODS:
        public Vector2 CheckMoveInput()
        {
            //Return directional input for given keys
            Vector2 directional = Vector2.zero;
            if (Input.GetKey(buttons[0])) directional.y += 1;
            if (Input.GetKey(buttons[1])) directional.x -= 1;
            if (Input.GetKey(buttons[2])) directional.y -= 1;
            if (Input.GetKey(buttons[3])) directional.x += 1;
            return directional;
        }
        public bool CheckAbilityInput()
        {
            //Return buttonDown input for given key
            return Input.GetKeyDown(buttons[4]);
        }
    }
    [System.Serializable] public class Player
    {
        //Represents one player with an associated bug and control scheme

        //Objects & Components:
        internal IInputMethod inputMethod;
        internal IControllable playerPawn;

        //Stats & Mem Vars:
        internal float timeSinceLastInput;
        internal bool markedForDisposal;
    }

    //Objects & Components:
    public static InputMaster inputMaster;
    public GameObject playerPrefab;
    public List<Player> players = new List<Player>(); //All active players in scene
    public List<ControllerInstance> controllerSetups = new List<ControllerInstance>(); //All connected controllers in game
    public List<Joystick> connectedJoysticks = new List<Joystick>(); //All active joysticks in scene
    public KeyboardInstance[] keyboardSetups; //Array of special objects for connecting and disconnecting keyboard players
    private PlayerInputManager inputManager; //The input manager component on the GameMaster gameObject
    private readonly System.Random rnd = new System.Random(); //Get random seed

    //Settings:
    [Header("Settings:")]
    public float idleKickTime;

    //Status Vars:
    private int playersJoined;

    //LOOP METHODS:
    private void Awake()
    {
        //Singleify:
        if (inputMaster == null) { DontDestroyOnLoad(gameObject); inputMaster = this; }
        else Destroy(gameObject);

        //Get Components:
        inputManager = GetComponent<PlayerInputManager>(); //Get player input manager
    }

    private void Update()
    {
        //Check for disconnected devices:
        CheckForKeyboardDisconnection();
        //Controller disconnection is handled by ControllerInstance objects

        //Check for new players:
        CheckForNewKeyboardPlayers();
        CheckForNewJoystickPlayers();

        //Call input functions (and maybe delete players):
        CheckPlayerInputs();
    }

    //INPUT:
    private void CheckPlayerInputs()
    {
        //Function: Runs through list of active players and checks all their inputs

        //Find and Send Player Inputs:
        foreach (Player player in players) //Iterate through list of active players
        {
            //Get Input Values:
            bool idle = true; //Initialize condition to check whether or not player has done anything this frame
            Vector2 moveInput = player.inputMethod.CheckMoveInput(); if (moveInput != Vector2.zero) idle = false; //Get input value and check if idle
            bool buttonInput = player.inputMethod.CheckAbilityInput(); if (buttonInput) idle = false; //Get input value and check if idle

            //Safety Check:
            if (player.playerPawn == null) continue;

            //Send Input Values to PlayerController:
            player.playerPawn.ReceiveJoystick(moveInput); //Send directional input
            player.playerPawn.ReceiveButton(buttonInput); //Send button input

            //Check for Idle Kick:
            if (idle) //If player has been idle, increase idle time and check for idle kick condition
            {
                player.timeSinceLastInput += Time.deltaTime; //Add to idle time counter if player didn't do anything
                if (player.timeSinceLastInput >= idleKickTime) player.markedForDisposal = true; //Indicate that player has left if afk for long enough
            }
            else player.timeSinceLastInput = 0; //Reset idle time tracker if player did anything
        }

        //Delete Inactive Players:
        for (int i = 0; i <= players.Count - 1;) //Iterate cautiously through list of players
        {
            if (players[i].markedForDisposal) DestroyPlayer(players[i]); //Destroy players marked for disposal
            else i++; //Pass over unmarked players
        }
    }

    //CONNECTION & DISCONNECTION:
    private void NewPlayerJoin(IInputMethod inputMethod)
    {
        //Setup Object Dependencies:
        Player newPlayer = new Player();     //Create new player object
        newPlayer.inputMethod = inputMethod; //Establish connection between player and input method
        inputMethod.GivePlayer(newPlayer);   //Establish connection between input method and player
        players.Add(newPlayer);              //Add new player to list of existing players

        //Create and Place Player Character:
        PlayerController newBug = Instantiate(playerPrefab.GetComponent<PlayerController>());
        newPlayer.playerPawn = newBug.GetComponent<IControllable>();
        newPlayer.playerPawn.GivePlayer(newPlayer); //Give player to pawn
        Transform spawnPoint = DeathHandler.deathHandler.spawnPoints[Random.Range(0, DeathHandler.deathHandler.spawnPoints.Length - 1)]; //Get random location within spawnpoints
        newBug.transform.position = spawnPoint.position;

        //Add Player Tag:
        playersJoined++;
        newBug.name = "P" + playersJoined.ToString();
        newBug.transform.GetChild(1).GetChild(2).GetComponent<TextMeshPro>().text = newBug.name; //Set indicator
        newBug.transform.GetChild(1).GetChild(2).GetComponent<TextMeshPro>().color = new Color(Random.Range(0, 100), Random.Range(0, 100), Random.Range(0, 100));
    }
    private void DestroyPlayer(Player player)
    {
        //Break Object Dependencies:
        player.inputMethod.RemovePlayer(); //Disconnect player from input source (so that it can be used again)

        //Despawn Player Objects:
        //NOTE: Maybe add some fun destruction/despawn effects
        player.playerPawn.DestroyPawn(); //Destroy player character gameObject
        players.Remove(player); //Remove player from list of active players, officially destroying it for good
    }

    //KEYBOARD FUCKERY:
    private void CheckForNewKeyboardPlayers()
    {
        foreach (KeyboardInstance keyboard in keyboardSetups)
        {
            if (keyboard.isActive) continue; //Skip if player is already active
            foreach (string button in keyboard.buttons) //Iterate through buttons looking for one that's pressed
            {
                if (button == "") continue; //Skip null keys (for debug reasons)
                if (Input.GetKey(button))
                {
                    NewPlayerJoin(keyboard); //Join new player under keyboard
                    break; //Break out of scanning this keyboard if a player has used it to join
                }
            }
        }
    }
    private void CheckForKeyboardDisconnection()
    {
        //NOTE: Do this when I'm really bored and have nothing better to do than to make a contingency for something that's never gonna happen
    }


    //JOYSTICK FUCKERY:
    private void CheckForNewJoystickPlayers()
    {
        //Function: Checks all joysticks on all controllers in scene and spawns a player object if joystick becomes active

        foreach (Joystick joystick in connectedJoysticks) //Parse through every connected joystick in game
        {
            if (joystick.isActive) continue; //Ignore joysticks that are already active
            if (joystick.CheckMoveInput() != Vector2.zero || joystick.CheckAbilityInput()) //Input is detected on an inactive joystick
            {
                NewPlayerJoin(joystick); //Join new player under joystick
            }
        }
    }
    public void AddNewController(ControllerInstance newController)
    {
        //Function: Adds a new controller instance to list, allowing InputMaster to pull inputs for up to 2 players (joysticks) from it

        //Add References:
        controllerSetups.Add(newController); //Add new controller to controller list
        connectedJoysticks.Add(newController.joystick1); //Add left joystick to inputmethods
        connectedJoysticks.Add(newController.joystick2); //Add right joystick to inputmethods
    }
    public void RemoveController(ControllerInstance controller)
    {
        //Function: Removes a controller instance from game, deleting both associated joysticks, along with any active player objects and attached pawns

        //Remove Existing Player Pawns:
        if (controller.joystick1.isActive) DestroyPlayer(controller.joystick1.attachedPlayer); //Remove left joystick's player from game
        if (controller.joystick2.isActive) DestroyPlayer(controller.joystick2.attachedPlayer); //Remove right joystick's player from game

        //Remove Object References from Game Lists:
        connectedJoysticks.Remove(controller.joystick1); //Remove left joystick from list
        connectedJoysticks.Remove(controller.joystick2); //Remove right joystick from list
        controllerSetups.Remove(controller); //Remove this controller from list
    }

}
