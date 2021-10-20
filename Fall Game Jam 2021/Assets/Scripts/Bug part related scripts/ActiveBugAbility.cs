using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActiveBugAbility : MonoBehaviour
{

    private PlayerController PC;

    public GameObject webProjectile;
    public GameObject SludgePrefab;

    public float AbilityRechargeTime;//amount of time before you can use active ability again


    private bool AbilityReady = true;
    //EventSystem
    public delegate void abilityUsed();
    public static event abilityUsed OnAbilityUsed;

    private void Start()
    {
        //test cases for abilities
        PC = gameObject.GetComponent<PlayerController>();

        addAbillity(3);
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
            case 2:
                ActiveBugAbility.OnAbilityUsed += grow;
                break;
            case 3:
                ActiveBugAbility.OnAbilityUsed += webShot;
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

    /*
    private void OnFire()
    {
        if (AbilityReady)
        {
            OnAbilityUsed();
            AbilityReady = false;
            StartCoroutine("recharge");
        }
    }
    */

    public void sludge()
    {
        Vector3 spawnposition = transform.position - (2*transform.forward);
        GameObject.Instantiate(SludgePrefab, new Vector3(spawnposition.x, 1.2f, spawnposition.z), Quaternion.Euler(90, 0, 0));
        //instantiae sludge behind player

    }
    public void grow()
    {
        //grow slightly bigger for a short period maybe increase mass?
        
        StartCoroutine(ScaleUp());

    }
    public void jump()
    { 
        //add grounded check

        float launchPower = 300;
        //boost into the air with flea legs
        GetComponent<Rigidbody>().AddForce(Vector3.up* launchPower);
    }
    public void webShot()
    {
        GameObject webBullet = Instantiate(webProjectile, transform.position + Vector3.forward, Quaternion.Euler(transform.rotation.x,transform.rotation.y,transform.rotation.z));
        Vector3 launchDir = (transform.position - (transform.position -Vector3.forward)).normalized;
        webBullet.GetComponent<BaseProjectile>().Setup(launchDir, 6);
    }

    public void leafSheild()
    {
       
    }
    
    public void speedBoost()
    {

    }



    // sets a time to recharge the ability
    IEnumerator recharge()
    {
        yield return new WaitForSeconds(AbilityRechargeTime);
        AbilityReady = true;
    }

    IEnumerator ScaleUp()
    {
        for(int i=0; i<40; i++)
        {
            
            //increase mass here
            yield return new WaitForSeconds(.05f);
            transform.localScale = new Vector3(transform.localScale.x + .01f, transform.localScale.y + .01f, transform.localScale.z + .01f);
        }
        yield return StartCoroutine(returnToSize());
    }
    IEnumerator returnToSize()
    {
        //time you stay large
        yield return new WaitForSeconds(1.0f);

        //reduce mass here
        for (int i = 0; i < 40; i++)
        {
            
            yield return new WaitForSeconds(.05f);
            transform.localScale = new Vector3(transform.localScale.x - .01f, transform.localScale.y - .01f, transform.localScale.z - .01f);
        }
    }


}


    




