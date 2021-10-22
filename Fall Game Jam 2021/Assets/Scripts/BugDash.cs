using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugDash : MonoBehaviour
{
    internal bool isDash;
    internal bool isCooldown;
    
    public float dashTime;
    public float dashCooldown;
    public float dashSpeed;
    public float bumpMulti;
    public float spawnWait;
    
    internal PlayerController playCtrl;
    public AudioClip speedSound;

    public GameObject SpeedParticle;

    internal void Start()
    {
        playCtrl = GetComponent<PlayerController>();
        isCooldown = false;
        isDash = false;
    }

    internal IEnumerator Nyoom()
    {
        Vector3 velocity;
        float baseMaxStor;

        partcileEffect();

        isCooldown = true;
        isDash = true;

        //match movement speed cap to speed of dash and store original value
        baseMaxStor = playCtrl.baseMaxSpeed;
        playCtrl.baseMaxSpeed = dashSpeed;

        velocity = (dashSpeed * transform.forward);
        velocity = new Vector3(velocity.x, 0, velocity.y); //Rearrange velocity to fit in world

        GetComponent<AudioSource>().clip = speedSound;
        transform.Translate(velocity * Time.deltaTime); //Move bug by velocity (along x/y axis)
        GetComponent<AudioSource>().Play(0);

        yield return new WaitForSeconds(dashTime);
        playCtrl.baseMaxSpeed = baseMaxStor;
        isDash = false;

        

        yield return new WaitForSeconds(dashCooldown);
        isCooldown = false;
      
    }

    public void partcileEffect()
    {
       
        SpeedParticle.GetComponent<ParticleSystem>().Play();
                
           
          

    }
    public void speedOff()
    {
        SpeedParticle.GetComponent<ParticleSystem>().Pause();
       

    }
}