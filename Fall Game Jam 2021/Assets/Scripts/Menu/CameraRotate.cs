using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    // Ui instantly goes away and the rotation stops the second play button is pressed would love to transition into it
 

    public GameObject stump;
    public GameObject Ui;
    [SerializeField]
    private float rotationSpeed = 20;
   
    enum cameraState
    {
        staticPlay,
        rotate,
        Settings
    }

    cameraState menuState = cameraState.rotate;

    private void Awake()
    {
        MainMenuButton.OnPlayPressed += PlayButtonPressed;
    }

    // Update is called once per frame
    void Update()
    {
        switch (menuState) { 

        case cameraState.staticPlay:
            // do nothing its fricken go time bb
        break;
        case cameraState.Settings:
            
        break;

        case cameraState.rotate:
                transform.RotateAround(stump.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
                break;
 
                }


               
    }

    public void PlayButtonPressed()
    {

        menuState = cameraState.staticPlay;

        DisableUi();
    }


    public void DisableUi()
    {
        Ui.SetActive(false);
    }





}
