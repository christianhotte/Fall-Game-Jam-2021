using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sludge : MonoBehaviour
{
    SpriteRenderer sp;
    List<GameObject> objectsInPool = new List<GameObject>();

    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        StartCoroutine("fadeAway");
    }

    private void OnTriggerEnter(Collider other)
    {
        //getsludge effect applied to player if they are a player
        objectsInPool.Add(other.gameObject);
        

    }
    private void OnTriggerExit(Collider other)
    {
        //remove the sludge from the player when they leave if effected by a sludge effect
        objectsInPool.Remove(other.gameObject);
    }

    IEnumerator fadeAway()
    {
        //wait initial time then slowly fade away and then destroy out sludgy puddle owow
        yield return new WaitForSeconds(3.0f);
        int x = 500;
        while (x > 0)
        {
            yield return new WaitForSeconds(.05f);
            sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, sp.color.a - .015f);
            
            if (sp.color.a <= 0)
            {
                preDeathCheck();
                GameObject.Destroy(this.gameObject);
            }
            x--;
        }
    }

    public void preDeathCheck()
    {
        foreach(GameObject x in objectsInPool)
        {
            if (x.GetComponent<CollisionBroadcaster>())
            {
                x.GetComponent<CollisionBroadcaster>().PuddleGone();
            }
        }
    }
    

}
