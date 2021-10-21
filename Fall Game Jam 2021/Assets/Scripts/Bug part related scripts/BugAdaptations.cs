using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugAdaptations : MonoBehaviour
{
    
    private int numBugAbilites = 15;
    private PlayerController PC;

    public GameObject webProjectile;
    public GameObject SludgePrefab;

    public float AbilityRechargeTime;//amount of time before you can use active ability again


    private bool AbilityReady = true;

    //EventSystem
    public delegate void abilityUsed();
    //public static event abilityUsed OnAbilityUsed;
    abilityUsed useAbility;

    //boolean index of what abilities are enabled - number in index matches abilityID 
    public bool[] adaptOn = new bool[14];
    internal GameObject adaptParent;

    private void Start()
    {
        //retrieves attached script PlayerController
        PC = gameObject.GetComponent<PlayerController>();
        
        //finds child AbilityMods, which is itself a child of Body
        adaptParent = gameObject.transform.Find("Body").gameObject;
        adaptParent = adaptParent.transform.Find("AbilityMods").gameObject;

        //disables all children of AbilityMods by default
        for (int i = 0; i < adaptParent.transform.childCount; i++)
        {
            var child = adaptParent.transform.GetChild(i).gameObject;
            if (child != null)
                child.SetActive(false);
        }
       
        //sets booleans for each adaptation to false
        for(int i = 0; i < adaptOn.Length; i++)
            adaptOn[i] = false;

     

    }

    public void addAbility(int abilityID)
    {
        adaptOn[abilityID] = true;

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
                useAbility += sludge;
                break;
            case 12:
                useAbility += grow;
                break;
            case 13:
                useAbility += webShot;
                break;
            case 14:
                antify();
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




    //speeds you up in every way at the cost of some strength 
    public void Shrink()
    {
        
        PC.ChangeBugSize(-.2f);
        PC.strengthModifier -= PC.baseStrength * .10f;
        PC.accelModifier += .2f * PC.baseAccel;
        PC.speedModifier += .2f * PC.baseMaxSpeed;
        PC.rotationSpeedModifier += .15f * PC.baseMaxRotationSpeed;
    }

    //str up, turnspeed down, size up++ 
    public void Biggify()
    {
        
        PC.ChangeBugSize(.4f);
        PC.strengthModifier += .2f * PC.baseStrength;
        PC.rotationSpeedModifier -= .2f * PC.baseMaxRotationSpeed;
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
        PC.knockbackResistModifier -= .05f;
        PC.baseMaxRotationSpeed -= .15f * PC.baseMaxRotationSpeed;
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
        PC.rotationSpeedModifier += .2f * PC.baseMaxRotationSpeed;
        PC.accelModifier += .2f * PC.baseAccel;
    }

    // increase your knock back resist but at the cost of turnspeed and speed
    public void Harden()
    {
        PC.knockbackResistModifier -= .15f;
        PC.rotationSpeedModifier -= .3f * PC.baseMaxRotationSpeed;
        PC.speedModifier -= .3f * PC.baseMaxSpeed;
    }

    // big size increase little strength increase;
    public void inflate()
    {
        PC.ChangeBugSize(.6f);
        PC.strengthModifier += .1f * PC.baseStrength;
    }

    // faster acceleration
    public void turboBuggo()
    {
        PC.accelModifier += .25f * PC.baseAccel;
        PC.speedModifier += .1f * PC.baseMaxSpeed;
    }

    //gain strength lose turn speed
    public void bigHeadNoNeck()
    {
        PC.rotationSpeedModifier -= .15f * PC.baseMaxRotationSpeed;
        PC.strengthModifier += .2f * PC.baseStrength;
    }

    // more strength less size bit slower
    public void densify()
    {
        PC.strengthModifier += .2f * PC.baseStrength;
        PC.ChangeBugSize(-.3f);
        PC.speedModifier -= .2f * PC.baseMaxSpeed;
    }

    // grow a lot smaller at the cost of nothing
    public void antify()
    {
        PC.ChangeBugSize(-.5f);
    }




    public void sludge()
    {
       

        Vector3 spawnposition = transform.position - transform.GetChild(0).transform.forward * 2;

        GameObject.Instantiate(SludgePrefab, new Vector3(spawnposition.x, 1.2f, spawnposition.z), Quaternion.Euler(90, 0, 0));
        //instantiae sludge behind player

    }
    public void grow()
    {
        //grow slightly bigger for a short period maybe increase mass?
        StartCoroutine(ScaleUp());

    }

    public void webShot()
    {
        Vector3 localForward = transform.GetChild(0).transform.forward * -1;
        GameObject webBullet = Instantiate(webProjectile, transform.position , Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z));
        Vector3 launchDir = localForward;
        
        webBullet.GetComponent<BaseProjectile>().Setup(launchDir, 6, this.gameObject);
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
        for (int i = 0; i <= NumChoices; i++)
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
            PC.strengthModifier += .2f * PC.baseStrength;
            yield return new WaitForSeconds(.05f);
            transform.localScale = new Vector3(transform.localScale.x + .01f, transform.localScale.y + .01f, transform.localScale.z + .01f);
        }
        yield return StartCoroutine(returnToSize());
    }
    IEnumerator returnToSize()
    {
        //time you stay large
        yield return new WaitForSeconds(1.5f);

        //reduce mass here
        PC.strengthModifier -= .2f * PC.baseStrength;
        for (int i = 0; i < 40; i++)
        {

            yield return new WaitForSeconds(.05f);
            transform.localScale = new Vector3(transform.localScale.x - .01f, transform.localScale.y - .01f, transform.localScale.z - .01f);
        }
    }

  
}
