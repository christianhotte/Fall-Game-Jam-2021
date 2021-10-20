using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    Vector3 travelDirection;
    
    
    float projectileSpeed;
    public void Setup( Vector3 dir, float projspeed)
    {
        travelDirection = dir;
        projectileSpeed = projspeed;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += travelDirection *projectileSpeed* Time.deltaTime;
       
        //move forward in direction vevtor based on direction player was facing

    }
}
