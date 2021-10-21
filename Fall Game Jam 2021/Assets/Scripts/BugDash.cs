using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugDash : MonoBehaviour
{
    public bool isDash;
    public float dashTime;
    public float dashSpeed;
    public float bumpMulti;
    
    internal PlayerController playCtrl;

    internal void Start()
    {
        playCtrl = GetComponent<PlayerController>();
        isDash = false;
    }

    internal IEnumerator Nyoom()
    {
        Vector3 velocity;
        float baseMaxStor;

        isDash = true;

        //match movement speed cap to speed of dash and store original value
        baseMaxStor = playCtrl.baseMaxSpeed;
        playCtrl.baseMaxSpeed = dashSpeed;

        velocity = (dashSpeed * transform.forward);
        velocity = new Vector3(velocity.x, 0, velocity.y); //Rearrange velocity to fit in world
        
        transform.Translate(velocity * Time.deltaTime); //Move bug by velocity (along x/y axis)

        yield return new WaitForSeconds(dashTime);
        playCtrl.baseMaxSpeed = baseMaxStor;
        isDash = false;
    }
}