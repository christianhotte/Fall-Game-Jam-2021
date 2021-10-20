using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    //Function: Called on by bugs to get global variables relating to the death of bugs

    //Objects & Components:
    public static DeathHandler deathHandler; //Singleton deathHandler object in game (can be called from anywhere)
    public Transform[] spawnPoints; //Array of all spawnpoints in scene
    private readonly System.Random rnd = new System.Random(); //Get random seed

    private void Awake()
    {
        if (deathHandler == null) deathHandler = this;
        else Destroy(this);
    }

    //DEATHODS:
    public void BugDiedProcedure(PlayerController deadBug)
    {
        //Function: Do everything that needs to be done right when a bug dies

        Debug.Log("Bug death handled");
    }
    public void RespawnAtRandomLocation(InputMaster.Player player, PlayerController deadBug)
    {
        //Function: Respawns a dead bug at a random location (in spawnpoints), with properties of deadBug and controlled by given player
        //Note: Leaves husk of dead bug behind for visual reasons

        //Leave empty (simplified) bug husk:
        GameObject bugHusk = Instantiate(deadBug.gameObject); //Generate new dead bug object at position of dead bug
        PlayerController huskController = bugHusk.GetComponent<PlayerController>(); //Get reference for husk's controller
        Destroy(huskController.headBox); //Destroy unnecessary head collider object
        Destroy(huskController.bodyBox); //Destroy unnecessary body collider object
        Destroy(huskController); //Destroy core playerController on bugHusk

        //Reposition and Respawn Player Character:
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length - 1)]; //Get random location within spawnpoints
        deadBug.transform.position = spawnPoint.position; //Move player to spawn position
        deadBug.transform.rotation = spawnPoint.rotation; //Rotate player to align with spawn (make sure spawns are rotated sensibly)

        //Change Player Control Back to Bug (remove respawn UI):
            //NOTE: -ADD STUFF HERE FOR WHAT UI DOES WHEN IT GETS REMOVED/TIMED OUT
            //      -THIS IS PROLLY WHERE WE WANNA PUT THE THING THAT ACTUALLY ADAPTS THE BUG BASED ON UI CHOICE

        //Finish & Cleanup:
        deadBug.BugResurrect(); //Resurrect bug and get back in the fight (make controllable again and reset temporary stats/vars (also implement adaptational changes maybe)
    }
}
