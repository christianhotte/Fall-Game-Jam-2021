using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proboscis : MonoBehaviour
{
    GameObject held;
    private bool canGrab = true;
    private void OnTriggerEnter(Collider other)
    {
        if (canGrab)
        {
            print("trig");
            held = other.gameObject;
            // gets the player that the probuscuis is parented too and sets the other player as a child for a short while
            other.transform.parent = this.transform.parent.transform;
            StartCoroutine("freeTheChild");
        }
        canGrab = false;
    }

     IEnumerator freeTheChild()
    {
        yield return new WaitForSeconds(1.5f);
       
        held.transform.parent = null;
        held = null;
        StartCoroutine("recharge");
    }
    IEnumerator recharge()
    {
        yield return new WaitForSeconds(3.0f);
        canGrab = true;
    }
}
