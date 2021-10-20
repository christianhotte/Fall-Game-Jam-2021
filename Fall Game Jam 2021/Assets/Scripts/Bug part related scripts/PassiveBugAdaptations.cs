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

    public void harden() { PC.baseWeight += .4f;PC.baseMaxSpeed -= .4f; }
    public void shrink() { 
        transform.localScale = new Vector3(transform.localScale.x - .20f, transform.localScale.y - .20f, transform.localScale.z - .20f);
        PC.baseWeight -= .3f;
        PC.baseAccel += .2f;
        PC.baseMaxSpeed += .2f;
        PC.baseMaxRotationSpeed += .2f;
    }
    //str up,turnspeed down,size up++ 
    public void Biggify() {}
    public void Speed() { }
    public void heftyify() { }
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
