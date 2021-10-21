using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBroadcaster : MonoBehaviour
{
    private enum ColliderType { BugHead, BugBody }
    private ColliderType type;

    private void Awake()
    {
        switch (gameObject.tag)
        {
            case "Head":
                type = ColliderType.BugHead;
                break;
            case "Body":
                type = ColliderType.BugBody;
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Check what other is:
        /*if (collision.rigidbody.CompareTag("Body")) //Dabug hit a bug body
        {
            if (type == ColliderType.BugBody)
            {
                SendMessageUpwards("BugBounce", collision);
            }
            else if (type == ColliderType.BugHead)
            {
                SendMessageUpwards("BugBump", collision);
            }
        }
        else if (collision.rigidbody.CompareTag("Head")) //Dabug hit a bug head
        {
            if (type == ColliderType.BugHead)
            {
                SendMessageUpwards("BugBump", collision);
            }
            else if (type == ColliderType.BugBody)
            {

            }
        }*/
    }
    private void OnTriggerEnter(Collider other)
    {
        //Check what other is:
        if (other.CompareTag("Body")) //Dabug hit a bug body
        {
            if (type == ColliderType.BugBody)
            {
                SendMessageUpwards("BugBounce", other);
            }
            else if (type == ColliderType.BugHead)
            {
                SendMessageUpwards("BugBump", other);
            }
        }
        else if (other.CompareTag("Head")) //Dabug hit a bug head
        {
            if (type == ColliderType.BugHead)
            {
                SendMessageUpwards("BugBump", other);
            }
            else if (type == ColliderType.BugBody)
            {

            }
        }
        else if (other.CompareTag("Sludge"))//dabug hit sludge
        {
            SendMessageUpwards("TouchedSludge");
        }
        else if (other.CompareTag("Web"))//dabug hit sludge
        {
           
            if ((transform.root.gameObject.Equals(other.GetComponent<BaseProjectile>().launchedfrom)))
            {
                print("I hit myself!");
            }
            else
            {
                SendMessageUpwards("Webbed");
                GameObject.Destroy(other.gameObject);
            }
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Sludge"))
        {
            SendMessageUpwards("UnTouchSludge");
        }
    }

    //called on this by sludge puddles when puddle kills itself to free them from slow
    public void PuddleGone()
    {
        SendMessageUpwards("UnTouchSludge");
    }

}
