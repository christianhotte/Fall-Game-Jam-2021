using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugAdaptations : MonoBehaviour
{
    
    private int numBugAbilites = 14;
    private PlayerController PC;

    public GameObject webProjectile;
    public GameObject SludgePrefab;

    public float AbilityRechargeTime;//amount of time before you can use active ability again


    private bool AbilityReady = true;

    //EventSystem
    public delegate void abilityUsed();
    //public static event abilityUsed OnAbilityUsed;
    abilityUsed useAbility;


    private void Start()
    {
        
        print("goo Add");
        
        PC = gameObject.GetComponent<PlayerController>();
        
    }

    public void addAbility(int abilityID)
    {
        switch (abilityID)
        {
            case 0:
                Shrink();
                break;
            case 1:
                Biggify();
                break;
            case 2:
                Speed();
                break;
            case 3:
                Heftyify();
                break;
            case 4:
                BulkUp();
                break;
            case 5:
                OilUpThoseLegJoints();
                break;
            case 6:
                Harden();
                break;
            case 7:
                inflate();
                break;
            case 8:
                turboBuggo();
                break;
            case 9:
                bigHeadNoNeck();
                break;
            case 10:
                densify();
                break;
            case 11:
                print("Iwork");
                useAbility += sludge;
                break;
            case 12:
                useAbility += grow;
                break;
            case 13:
                useAbility += webShot;
                break;
            case 14:
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


    public void UseBugAbility()
    {
        if (AbilityReady)
        {

            useAbility?.Invoke();
            AbilityReady = false;
            StartCoroutine("recharge");
        }
    }

    //Note: Sludge effects are part of the sludge object



    //speeds you up in every way at the cost of some strength 
    public void Shrink()
    {
        PC.sizeModifier = .8f * PC.baseSize;
        PC.ChangeBugSize(PC.sizeModifier);
        PC.strengthModifier -= PC.baseStrength * .05f;
        PC.accelModifier += .1f * PC.baseAccel;
        PC.speedModifier += .1f * PC.baseMaxSpeed;
        PC.rotationSpeedModifier += .025f * PC.baseMaxRotationSpeed;
    }

    //str up, turnspeed down, size up++ 
    public void Biggify()
    {
        PC.sizeModifier += .4f * PC.baseSize;
        PC.ChangeBugSize(PC.sizeModifier);
        PC.strengthModifier += .02f * PC.baseStrength;
        PC.rotationSpeedModifier -= .0f * PC.baseMaxRotationSpeed;
    }

    //faster bug no down side
    public void Speed()
    {
        PC.speedModifier += .2f * PC.baseMaxSpeed;
    }

    // strength and knockback resist, slower turn and way slower max speed
    public void Heftyify()
    {
        PC.strengthModifier += .3f * PC.baseStrength;
        PC.knockbackResistModifier -= .1f;
        PC.baseMaxRotationSpeed -= .05f * PC.baseMaxRotationSpeed;
        PC.baseMaxSpeed -= .4f * PC.baseMaxSpeed;

    }

    // gain more knockback strength 
    public void BulkUp()
    {
        PC.strengthModifier += .2f * PC.baseStrength;
    }

    // Faster turning and acceleration those are some wet joints
    public void OilUpThoseLegJoints()
    {
        PC.rotationSpeedModifier += .05f * PC.baseMaxRotationSpeed;
        PC.accelModifier += .2f * PC.baseAccel;
    }

    // increase your knock back resist but at the cost of turnspeed and speed
    public void Harden()
    {
        PC.knockbackResistModifier -= .05f;
        PC.rotationSpeedModifier -= .06f * PC.baseMaxRotationSpeed;
        PC.speedModifier -= .3f * PC.baseMaxSpeed;
    }

    // big size increase little strength increase;
    public void inflate()
    {
        PC.sizeModifier += .4f * PC.baseSize;
        PC.strengthModifier += .1f * PC.baseStrength;
    }

    // faster acceleration
    public void turboBuggo()
    {
        PC.accelModifier += .25f * PC.baseAccel;
    }

    //gain strength lose turn speed
    public void bigHeadNoNeck()
    {
        PC.rotationSpeedModifier -= .05f * PC.baseMaxRotationSpeed;
        PC.strengthModifier += .2f * PC.baseStrength;
    }

    // more strength less size bit slower
    public void densify()
    {
        PC.strengthModifier += .2f * PC.baseStrength;
        PC.sizeModifier = .8f * PC.baseSize;
        PC.speedModifier -= .2f * PC.baseMaxSpeed;
    }
    public void sludge()
    {
        print("added");

        Vector3 spawnposition = transform.position - (2 * transform.forward);

        GameObject.Instantiate(SludgePrefab, new Vector3(spawnposition.x, 1.2f, spawnposition.z), Quaternion.Euler(90, 0, 0));
        //instantiae sludge behind player

    }
    public void grow()
    {
        //grow slightly bigger for a short period maybe increase mass?
        StartCoroutine(ScaleUp());

    }

    // webshot web effect needs to be built
    public void webShot()
    {
        GameObject webBullet = Instantiate(webProjectile, transform.position + Vector3.forward, Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z));
        Vector3 launchDir = (transform.position - (transform.position - Vector3.forward)).normalized;
        webBullet.GetComponent<BaseProjectile>().Setup(launchDir, 6);
    }


    /// <summary>
    /// how many random bug abilites do you want it will return that many
    /// </summary>
    /// <param name="NumChoices"></param>
    /// <returns></returns>
    public List<int> getAbilityChoices(int NumChoices)
    {

        List<int> ints = new List<int>();

        // creat list of all possible abilites
        for (int i = 0; i < numBugAbilites; i++)
        {
            ints.Add(i);
        }

        List<int> choices = new List<int>();
        //parse through numCHoies of times and send them back
        for (int i = 0; i < NumChoices; i++)
        {
            int RandChoice = Random.Range(0, ints.Count);
            choices.Add(RandChoice);
            ints.Remove(RandChoice);
        }

        return choices;
    }


    // sets a time to recharge the ability
    IEnumerator recharge()
    {
        yield return new WaitForSeconds(AbilityRechargeTime);
        AbilityReady = true;
    }

    IEnumerator ScaleUp()
    {
        for (int i = 0; i < 40; i++)
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
