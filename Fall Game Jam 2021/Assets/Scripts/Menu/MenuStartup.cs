using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuStartup : MonoBehaviour
{
    
    public AudioClip letterCrash;
    [SerializeField]

    string GameTitle;
    private int titleLength;
   // string currentTitle = "";

    public AudioSource ASource;
    public TextMeshPro tmp;
    void Start()
    {
        ASource = GetComponent<AudioSource>();
        ASource.clip = letterCrash;
        tmp = GetComponent<TextMeshPro>();
        titleLength = GameTitle.Length;
        StartCoroutine("LetterStamp");
    }
    IEnumerator LetterStamp()
    {
        print("thumo");
        //wait initial time then slowly clomp in
        yield return new WaitForSeconds(2.0f);
     
        for(int i = 0 ;  i < titleLength; i++)
        {
            yield return new WaitForSeconds(.25f);

            ASource.Play();
            string gs = GameTitle.Substring(0, i);

           // tmp.text = gs;

        }
    }
   

}
