using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugBumpinator : MonoBehaviour
{
    private Collider bugBox;
    private PlayerController pc;

    private void Awake()
    {
        bugBox = GetComponent<Collider>();
        pc = GetComponentInParent<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //pc.BugBump(collision);
    }
}
