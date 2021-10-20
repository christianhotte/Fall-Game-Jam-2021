using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    // Start is called before the first frame update

    Light ligh;
    void Start()
    {
       ligh = gameObject.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 1000) == 1)
        {
            // do lightning sounds
            ligh.enabled = true;
            StartCoroutine(lightOff());
        }
    }

    IEnumerator lightOff()
    {
        yield return new WaitForSeconds(.16f);
        ligh.enabled = false;
    }
}
