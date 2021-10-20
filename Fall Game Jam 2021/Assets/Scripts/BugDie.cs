using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class BugDie : MonoBehaviour
{
    void Start()
    {
        
    }
    int layerMask = 1 << 8;

    RaycastHit hit;
    private void FixedUpdate()
    {
        layerMask = ~layerMask;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 5, layerMask))
        {
            if (hit.collider.CompareTag("Death"))
                freeFall();

        }
    }
    
    public void freeFall()
    {
        foreach (Transform child in transform)
        {
            print(child.name);
           if(child.gameObject.name.Equals("HeadCollider")||child.gameObject.name.Equals("BodyCollider"))
            child.gameObject.SetActive(false);
        }
            
        callBugDie();

        GetComponent<Rigidbody>().isKinematic = false;
       
        GetComponent<CapsuleCollider>().enabled = true;
        
        GetComponent<Rigidbody>().AddForce(new Vector3(4, 5, 1));

    }

    // add a respawn in so the player can get control back over a bug

    public void callBugDie()
    {
        GetComponent<PlayerController>().BugDie();
    }

}
