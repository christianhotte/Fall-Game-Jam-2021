using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveBugAdaptations : MonoBehaviour
{

    //NOTE: size changing is still slightly weird and needs to be the fullinpit of size 
    private int numBugAbilites = 11;



    // adaptations that change that stats or as persistent passive effects
    private PlayerController PC;

    public bool gooTrail,anger,invisiblity = false;

    private int angerHitsCounter = 0;

    private void Start()
    {
        PC = gameObject.GetComponent<PlayerController>();
        Biggify();
        BulkUp();
        inflate();
    }

    public void addPassive(int abilityID)
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
                break;
            case 12:
                break;
            case 13:
                break;
            case 14:
                break;
                
            default:
                print("no bug ability of that ID");
                break;
        }
    }


    //speeds you up in every way at the cost of some strength 
    public void Shrink() {
        PC.sizeModifier = .8f*PC.baseSize;
        PC.ChangeBugSize(PC.sizeModifier);
        PC.strengthModifier -= PC.baseStrength * .05f;
        PC.accelModifier += .1f*PC.baseAccel;
        PC.speedModifier += .1f*PC.baseMaxSpeed;
        PC.rotationSpeedModifier += .1f*PC.baseMaxRotationSpeed;
    }
    //str up, turnspeed down, size up++ 
    public void Biggify() {
        PC.sizeModifier += .4f * PC.baseSize;
        PC.ChangeBugSize(PC.sizeModifier);
        PC.strengthModifier += .02f * PC.baseStrength;
        PC.rotationSpeedModifier -= .3f * PC.baseMaxRotationSpeed;
    }
    //faster bug no down side
    public void Speed() {
        PC.speedModifier += .2f * PC.baseMaxSpeed;
    }
    // strength and knockback resist, slower turn and way slower max speed
    public void Heftyify() {
        PC.strengthModifier += .3f * PC.baseStrength;
        PC.knockbackResistModifier -= .1f;
        PC.baseMaxRotationSpeed -= .2f * PC.baseMaxRotationSpeed;
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
        PC.knockbackResistModifier -= .25f;
        PC.rotationSpeedModifier -= .3f * PC.baseMaxRotationSpeed;
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
        PC.rotationSpeedModifier -= .2f * PC.baseMaxRotationSpeed;
        PC.strengthModifier += .2f * PC.baseStrength;
    }
    // more strength less size bit slower
    public void densify()
    {
        PC.strengthModifier += .2f * PC.baseStrength;
        PC.sizeModifier = .8f * PC.baseSize;
        PC.speedModifier -= .2f * PC.baseMaxSpeed;
    }

    public void gootrail() { gooTrail = true; }
   // public void Shrink() { }
    public void NoTouchy() { }

    //severly reduced turn speed +++strength
    public void BigHeadLittleWaist() { }




    public int[] getAbilityChoices(int NumChoices)
    {
        List<int> ints = new List<int>();
        //creare list thats num of bug abilites long 
        for (int i = 0; i < numBugAbilites ; i++)
        {
            ints.Add(i);
        }

        List<int> choices = new List<int>();
        for(int i=0; i < 3; i++)
        {
            int RandChoice = Random.Range(0, ints.Count);
            choices.
        }

            return choices;
    }


    


}
