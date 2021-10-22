using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugDash : MonoBehaviour
{
    internal bool isDash;
    internal bool isCooldown;
    
    [Header("Dash Stats")]
    public float dashTime;
    public float dashCooldown;
    public float dashSpeed;
    public float bumpMulti;
    public float spawnWait;
    
    internal PlayerController playCtrl;
    
    [Header("Dash Sounds")]
    public AudioClip speedSound1;
    public AudioClip speedSound2;

    [Header("Dash Particles")]
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

        particleEffect();

        isCooldown = true;
        isDash = true;

        //match movement speed cap to speed of dash and store original value
        baseMaxStor = playCtrl.baseMaxSpeed;
        playCtrl.baseMaxSpeed = dashSpeed;

        velocity = (dashSpeed * transform.forward);
        velocity = new Vector3(velocity.x, 0, velocity.y); //Rearrange velocity to fit in world

        //Determine dash sound
        int chance = Random.Range(1, 51); //generate a random number between 1 and 50
        if (chance == 50) GetComponent<AudioSource>().clip = speedSound2; //play secret sound if result is 50
        else GetComponent<AudioSource>().clip = speedSound1; //play normal sound otherwise
        
        GetComponent<AudioSource>().clip = speedSound1;
        transform.Translate(velocity * Time.deltaTime); //Move bug by velocity (along x/y axis)
        GetComponent<AudioSource>().Play(0);

        yield return new WaitForSeconds(dashTime);
        playCtrl.baseMaxSpeed = baseMaxStor;
        isDash = false;

        

        yield return new WaitForSeconds(dashCooldown);
        isCooldown = false;
      
    }

    public void particleEffect()
    {
       
        SpeedParticle.GetComponent<ParticleSystem>().Play();
                
           
          

    }
    public void speedOff()
    {
        SpeedParticle.GetComponent<ParticleSystem>().Pause();
       

    }
}