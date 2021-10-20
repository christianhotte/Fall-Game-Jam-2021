using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxProcessor : MonoBehaviour
{
    /*private PlayerController pc;
    public bool hurtbox;

    private void Awake()
    {
        pc = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HitBoxProcessor>() != null)
        {
            HitBoxProcessor otherCollider = other.GetComponent<HitBoxProcessor>();
            if (otherCollider.pc == pc) return; //Ignore self hits
            otherCollider.pc.BugBump(true, pc);
            if (!hurtbox)
            {
                pc.BugBump(false, otherCollider.pc);
            }
            else
            {
                pc.BugBump(true, otherCollider.pc);
            }
                
        }
    }*/
}
