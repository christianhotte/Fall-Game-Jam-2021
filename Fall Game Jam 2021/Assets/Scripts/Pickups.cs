using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    public bool isSticky;
    [SerializeField] private bool isStuck;


    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isSticky == true && isStuck == false)
        {
            transform.SetParent(other.transform, true);
            isStuck = true;
            GetComponent<SphereCollider>().isTrigger = false;
            Destroy(GetComponent<Rigidbody>());
        }

        // When object that is already stuck to the player collides with another Pickup
        if (other.gameObject.CompareTag("Pickup") && isStuck == true && other.GetComponent<Pickups>().isStuck == false)
        {
            other.transform.SetParent(transform, true);
        }
    }
}
