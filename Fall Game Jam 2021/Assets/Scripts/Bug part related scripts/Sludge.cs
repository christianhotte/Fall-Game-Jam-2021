using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sludge : MonoBehaviour
{
    SpriteRenderer sp;
    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        StartCoroutine("fadeAway");
    }

    private void OnTriggerEnter(Collider other)
    {
        //getsludge effect applied to player if they are a player
    }
    private void OnTriggerExit(Collider other)
    {
        //remove the sludge from the player when they leave if effected by a sludge effect
    }

    IEnumerator fadeAway()
    {
        //wait initial time then slowly fade away and then destroy out sludgy puddle owow
        yield return new WaitForSeconds(2.0f);
        int x = 50;
        while (x > 0)
        {
            yield return new WaitForSeconds(.15f);
            sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, sp.color.a - .05f);
            print(sp.color.a);
            if (sp.color.a <= 0)
            {
                GameObject.Destroy(this.gameObject);
            }
            x--;
        }
    }
}
