using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveBugAdaptations : MonoBehaviour
{
    // adaptations that change that stats or as persistent passive effects
    private PlayerController PC;
    public bool gooTrail,anger,invisiblity = false;

    private int angerHitsCounter = 0;

    private void Start()
    {
        PC = gameObject.GetComponent<PlayerController>();
    }

   // public void harden() { PC.baseWeight += .4f;PC.baseMaxSpeed -= .4f; }

    //speeds you up in every way at the cost of some strength 
    public void shrink() {
        PC.sizeModifier = .8f*PC.baseSize;
        PC.ChangeBugSize(PC.sizeModifier);
        PC.strengthModifier -= PC.baseStrength * .05f;
        PC.accelModifier += .1f*PC.baseAccel;
        PC.speedModifier += .1f*PC.baseMaxSpeed;
        PC.rotationSpeedModifier += .1f*PC.baseMaxRotationSpeed;
    }
    //str up, turnspeed down, size up++ 
    public void Biggify() {
        PC.sizeModifier = 1.4f * PC.baseSize;
        PC.ChangeBugSize(PC.sizeModifier);
        PC.strengthModifier += .02f * PC.baseStrength;
        PC.rotationSpeedModifier -= .3f * PC.baseMaxRotationSpeed;
    }
    //faster bug no down side
    public void Speed() {
        PC.speedModifier += .2f * PC.baseMaxSpeed;
    }
    // strength and knockback resist, slower turn and way slower max speed
    public void heftyify() {
        PC.strengthModifier += .3f * PC.baseStrength;
        PC.knockbackResistModifier -= .1f;
        PC.baseMaxRotationSpeed -= .2f * PC.baseMaxRotationSpeed;
        PC.baseMaxSpeed -= .4f * PC.baseMaxSpeed;
    
    }
    public void gootrail() { gooTrail = true; }
    public void Shrink() { }
    public void NoTouchy() { }

    //severly reduced turn speed +++strength
    public void BigHeadLittleWaist() { }







    //process persistent passive bug effects
    private void Update()
    {
        
    }


}
