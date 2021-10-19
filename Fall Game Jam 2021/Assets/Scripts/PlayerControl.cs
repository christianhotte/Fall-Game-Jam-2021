using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public float speed = 0;
    [SerializeField] float killY = -20f;

    private Rigidbody rb;

    private float movementX;
    private float movementY;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }
    private void Update()
    {
        if (rb.position.y <= killY) 
        {
            Debug.Log("WHOA holy fuck bro you despawned, maybe change killY if you keep dying lol");
            rb.position = new Vector3(0, 2, 0);
            rb.velocity = new Vector3(0, 0, 0);
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);
    }
}
