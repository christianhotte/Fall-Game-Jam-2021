using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugDash : MonoBehaviour
{
    public bool isDash;
    public float dashTime;
    public float dashSpeed;

    internal float DashMaxVel;
    //add code that sets an alt max velocity during dash to keep movement from acheiving dash speeds but also avoid something from knocking a dashing bug to go EVEN FURTHER BEYOND


    internal void Start()
    {
        isDash = false;
    }

    internal IEnumerator Nyoom()
    {
        //add force to vector 2 velocity, not add force
         
        isDash = true;
        Vector2 velocity = (dashSpeed * transform.forward);
        Debug.Log("velocity: " + velocity);
        Vector3 realVelocity = new Vector3(velocity.x, 0, velocity.y); //Rearrange velocity to fit in world
        Debug.Log("real velocity: " + realVelocity);
        transform.Translate(realVelocity * Time.deltaTime); //Move bug by velocity (along x/y axis)

        yield return new WaitForSeconds(dashTime);
        isDash = false;
    }
}