using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILookAtCamera : MonoBehaviour
{
    public Canvas canvas;
    public Camera UIcam;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        UIcam = GameObject.Find("UI Camera").GetComponent<Camera>();
        canvas.worldCamera = UIcam;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(Camera.main.transform);
    }
}
