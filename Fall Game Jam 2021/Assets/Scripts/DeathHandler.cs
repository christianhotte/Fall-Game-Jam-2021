using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    //Function: Called on by bugs to get global variables relating to the death of bugs

    //Objects & Components:
    public static DeathHandler deathHandler; //Singleton deathHandler object in game (can be called from anywhere)
    public GameObject deadBugUIPrefab; //UI pawn that gets spawned where and when a bug dies
    public Transform[] spawnPoints; //Array of all spawnpoints in scene (may need to be set individually for scenes)
    private readonly System.Random rnd = new System.Random(); //Get random seed
    private List<DeadBugUI> deathInstances = new List<DeadBugUI>(); //List of all DeadBugUI instances in scene
    private List<GameObject> husks = new List<GameObject>(); //List of all dead bug husks in scene

    //Settings:
    [Header("Settings:")]
    public float deathUILifetime; //Time (in seconds) before player respawns automatically (with auto-selected adaptation)
    public int bugBodyCap;        //The most bug bodies allowed in the scene (before old bodies start de-spawning)
    public bool idleGiveAbility; //If checked, a random ability will be granted when dead player times out

    //SEQUENCE METHODS:
    private void Awake()
    {
        //Safety Checks:
        if (deathHandler == null) deathHandler = this;
        else Destroy(this);
        if (spawnPoints.Length == 0) Debug.LogError("Assign your spawnpoints");
    }
    private void Update()
    {
        CheckUILifetime();
        CullHusks();
    }

    //DEATH HANDLER METHODS:
    public void BugDiedProcedure(PlayerController deadBug)
    {
        //Function: Do everything that needs to be done right when a bug dies

        //Generate New UI Object:
        DeadBugUI newUI = Instantiate(deadBugUIPrefab).GetComponent<DeadBugUI>(); //Instantiate new UI prefab
        deathInstances.Add(newUI); //Add new death instance to working list

        //Configure UI Object:
        newUI.deadBug = deadBug; //Have UI remember its bug
        newUI.transform.position = deadBug.transform.position; //Move UI element to position where bug has died (NOTE: may need adjustment)

        //Update Player Connection:
        InputMaster.Player player = deadBug.currentPlayer; //Get deadBug's player
        player.playerPawn = newUI; //Set player up so that it now controls UI object
        newUI.GivePlayer(player); //Set up UI so that it knows this player is controlling it
    }
    public void RespawnAtRandomLocation(DeadBugUI deathInstance)
    {
        //Function: Respawns a dead bug at a random location (in spawnpoints), with properties of deadBug and controlled by given player
        //Note: Leaves husk of dead bug behind for visual reasons

        //Establish Dependencies:
        InputMaster.Player player = deathInstance.currentPlayer; //Get player controlling bug
        PlayerController bug = deathInstance.deadBug; //Get bug object
        player.playerPawn = bug; //Move player control back to bug

        //Leave empty (simplified) bug husk:
        GameObject bugHusk = Instantiate(bug.gameObject); //Generate new dead bug object
        PlayerController huskController = bugHusk.GetComponent<PlayerController>(); //Get reference for husk's controller
        //Destroy(bugHusk.GetComponent<BugAdaptations>()); //Destroy adaptation component
        Destroy(huskController.headBox); //Destroy unnecessary head collider object
        Destroy(huskController.bodyBox); //Destroy unnecessary body collider object
        Destroy(huskController); //Destroy core playerController on bugHusk
        bugHusk.transform.position = bug.transform.position; //Position husk to match real bug
        bugHusk.transform.rotation = bug.transform.rotation; //Rotate husk to match real bug
        bugHusk.transform.localScale = bug.transform.localScale; //Scale husk to match real bug
        husks.Add(bugHusk); //Add husk to running list

        //Reposition and Respawn Player Character:
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length - 1)]; //Get random location within spawnpoints
        bug.transform.position = spawnPoint.position; //Move player to spawn position
        bug.transform.rotation = spawnPoint.rotation; //Rotate player to align with spawn (make sure spawns are rotated sensibly)

        //Finish & Cleanup:
        bug.BugResurrect(); //Resurrect bug and get back in the fight (make controllable again and reset temporary stats/vars (also implement adaptational changes maybe)
    }

    //INTERNAL HANDLER METHODS:
    private void CheckUILifetime()
    {
        //Parse through all instances of death UIs:
        foreach (DeadBugUI deathInstance in deathInstances)
        {
            if (deathInstance.markedForDisposal) continue; //Skip instances marked for deletion
            deathInstance.timeSinceDeath += Time.deltaTime; //Increment lifetime tracker
            if (deathInstance.timeSinceDeath >= deathUILifetime) //UI timed out, automatically pick adaptation
            {
                //NOTE: SelectAbility marks deathInstance for deletion
                if (idleGiveAbility && !deathInstance.deadBug.bugSuicide) deathInstance.SelectAbility(deathInstance.abilitySelection[0]); //Default-pick first ability in list
                else deathInstance.EndUI();
            }
        }

        //Look for instances to delete:
        for (int i = 0; i < deathInstances.Count;)
        {
            if (deathInstances[i].markedForDisposal) //Safely destroy instances marked for disposal
            {
                DeadBugUI markedInstance = deathInstances[i]; //Get reference to instance
                deathInstances.Remove(markedInstance); //Remove instance from list
                Destroy(markedInstance.gameObject); //Destroy instance
            }
            else i++; //Pass over unmarked instances
        }
    }
    private void CullHusks()
    {
        //Function: Removes husks when there are too many in the scene

        //Check husk list for overpopulation:
        while (husks.Count > bugBodyCap && husks.Count > 0)
        {
            GameObject husk = husks[0]; //Get oldest husk from list
            husks.RemoveAt(0); //Remove husk from list
            Destroy(husk); //Destroy removed husk
        }
    }
}
