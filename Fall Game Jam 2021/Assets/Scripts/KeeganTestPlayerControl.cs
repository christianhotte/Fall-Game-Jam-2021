using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeeganTestPlayerControl : MonoBehaviour
{
    // test control since the one jayden made doesnt do the move for keyboard
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        
        
            
    }

    public void left() { rb.AddForce(Vector3.left); }
           
    public void right()
    {
        rb.AddForce(Vector3.right);

    }
    public void up()
    {
        rb.AddForce(Vector3.up);

    }
    public void down() { rb.AddForce(Vector3.down); }


}
