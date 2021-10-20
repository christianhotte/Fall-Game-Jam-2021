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

    // This would cast rays only against colliders in layer 8.
    // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
    

    // Update is called once per frame
    void Update()
    {
     //  RaycastHit hit = Physics.Raycast(transform.position, Vector3.down,9)
    }
    RaycastHit hit;
    private void FixedUpdate()
    {
        layerMask = ~layerMask;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.CompareTag("Ground")) { freeFall(); }
        }
    }

    public void freeFall()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
