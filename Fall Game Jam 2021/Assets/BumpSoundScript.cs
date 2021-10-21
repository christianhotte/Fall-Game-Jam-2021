using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpSoundScript : MonoBehaviour
{
    public float timer;

    void Start()
    {
        StartCoroutine(DeletThis());
    }

    private IEnumerator DeletThis()
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
