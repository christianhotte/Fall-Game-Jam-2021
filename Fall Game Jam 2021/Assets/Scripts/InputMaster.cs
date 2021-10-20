using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputMaster : MonoBehaviour
{
    //Function: Tracks player instances, spawns new players when new inputs are detected, removes players when inputs are disconnected

    //Classes, Structs & Enums:
    public class Joystick : IInputMethod
    {
        //Joysticks (usually 2 per controller)

        //Objects:
        public Player attachedPlayer;

        //Stats:
        public bool isActive = false;

        //CONNECTION & DISCONNECTION:
        public void GivePlayer(InputMaster.Player player)
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
            //NOTE: put stuff in here
            return Vector2.zero;
        }
        public bool CheckAbilityInput()
        {
            //NOTE: put stuff in here
            return true;
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

        //CONNECTION & DISCONNECTION:
        public void GivePlayer(InputMaster.Player player)
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
        internal PlayerController playerCharacter;

        //Stats & Mem Vars:
        internal float timeSinceLastInput;
        internal bool markedForDisposal;
    }

    //Objects & Components:
    public static InputMaster inputMaster;
    public GameObject playerPrefab;
    public List<Player> players = new List<Player>(); //All active players in scene
    public List<Joystick> connectedJoysticks = new List<Joystick>(); //All active joysticks in scene
    public KeyboardInstance[] keyboardSetups;

    //Settings:
    [Header("Settings:")]
    public float idleKickTime;

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

    private void Update()
    {
        //Check for new players:
        CheckForNewKeyboardPlayers();
        CheckForNewStickPlayers();

        //Call input functions (and maybe delete players):
        CheckPlayerInputs();
    }

    //INPUT:
    private void CheckPlayerInputs()
    {
        //Function: Runs through list of active players and checks all their inputs

        foreach (Player player in players) //Iterate through list of active players
        {
            //Get Input Values:
            bool idle = true; //Initialize condition to check whether or not player has done anything this frame
            Vector2 moveInput = player.inputMethod.CheckMoveInput(); if (moveInput != Vector2.zero) idle = false; //Get input value and check if idle
            bool buttonInput = player.inputMethod.CheckAbilityInput(); if (buttonInput) idle = false; //Get input value and check if idle

            //Send Input Values to PlayerController:
            player.playerCharacter.ReceiveJoystick(moveInput); //Send directional input
            player.playerCharacter.ReceiveButton(buttonInput); //Send button input

            //Check for Idle Kick:
            if (idle) //If player has been idle, increase idle time and check for idle kick condition
            {
                player.timeSinceLastInput += Time.deltaTime; //Add to idle time counter if player didn't do anything
                if (player.timeSinceLastInput >= idleKickTime) player.markedForDisposal = true; //Indicate that player has left if afk for long enough
            }
            else player.timeSinceLastInput = 0; //Reset idle time tracker if player did anything

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
        newPlayer.playerCharacter = Instantiate(playerPrefab).GetComponent<PlayerController>();
        //NOTE: Add something that places character in spawn location

    }
    private void PlayerLeft(Player player)
    {
        //Break Object Dependencies:
        player.inputMethod.RemovePlayer(); //Disconnect player from input source (so that it can be used again)

        //Despawn Player Objects:
        //NOTE: Maybe add some fun destruction/despawn effects
        Destroy(player.playerCharacter.gameObject); //Destroy player character gameObject
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
    //JOYSTICK FUCKERY:
    private void CheckForNewStickPlayers()
    {

    }

}
