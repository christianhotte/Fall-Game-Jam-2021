using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugDie : MonoBehaviour
{
    // Start is called before the first frame update
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

        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce(new Vector3(4, 5, 1));
    }

    public void callBugDie()
    {
        GetComponent<PlayerController>().BugDie();
    }

}
