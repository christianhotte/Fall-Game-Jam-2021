using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;





public class ActiveBugAbility : MonoBehaviour
{
    /// <summary>
    /// recharge timer 
    /// bug where the first sludge spawns as a child of the spawner
    /// 
    /// 
    /// </summary>
    /// 
    public GameObject SludgePrefab;

    private bool AbilityReady = true;

    public delegate void abilityUsed();
    public static event abilityUsed OnAbilityUsed;

    private void Start()
    {
        addAbillity(1);
    }
    public void addAbillity(int abilityID)
    {
        switch (abilityID)
        {
            case 0:
                ActiveBugAbility.OnAbilityUsed += sludge;
                break;

            case 1:
                ActiveBugAbility.OnAbilityUsed += jump;
                break;
            default:
                print("no bug ability of that ID");
                break;
        }

    }
    public void removeAbillity(int abilityID)
    {
        if (abilityID == 0)
        {
            ActiveBugAbility.OnAbilityUsed -= sludge;
        }
    }

    private void OnFire()
    {
        if (AbilityReady)
        {
            OnAbilityUsed();
            AbilityReady = false;
            StartCoroutine("recharge");
        }
    }

    public void sludge()
    {
        Vector3 spawnposition = transform.position - transform.forward;
        GameObject.Instantiate(SludgePrefab, new Vector3(spawnposition.x, 1.2f, spawnposition.z), Quaternion.Euler(90, 0, 0));
        //instantiae sludge behind player

    }

    public void grow()
    {

    }

    public void jump()
    { 
        //add grounded check

        float launchPower = 300;
        //boost into the air with flea legs
        GetComponent<Rigidbody>().AddForce(Vector3.up* launchPower);
    }
        

    public void leafSheild()
    {

    }


    // sets a time to recharge the ability
    IEnumerator recharge()
    {
        yield return new WaitForSeconds(4.0f);
        AbilityReady = true;
    }
    


}


    




