using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    Vector3 travelDirection;

    public GameObject launchedfrom;
    float projectileSpeed;
    public void Setup( Vector3 dir, float projspeed, GameObject launcher)
    {
        travelDirection = dir;
        projectileSpeed = projspeed;
        launchedfrom = launcher;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += travelDirection *projectileSpeed* Time.deltaTime;
       
        //move forward in direction vevtor based on direction player was facing

    }
}
