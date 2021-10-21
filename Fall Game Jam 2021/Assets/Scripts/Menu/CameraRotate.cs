using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    // Ui instantly goes away and the rotation stops the second play button is pressed would love to transition into it

    private Vector3 CameraStartPos;
        private Quaternion CameraStartRot;
    public GameObject stump;


    public GameObject Ui;
    public GameObject UiTitle,MainMenuButtons,UiBackButton;
    

    public Transform cameraSettingsPosition;
   
    private float rotationSpeed = 20;
   
    enum cameraState
    {
        staticPlay,
        rotate,
        Settings,
        backButtonTransition
    }

    cameraState menuState = cameraState.rotate;

    private void Awake()
    {
        //subscribing to the menu button pressed int he mainmenu button classes
        MainMenuButton.OnPlayPressed += PlayButtonPressed;
        MainMenuButton.OnHowPlayPressed += SettingsButtonPressed;
        MainMenuButton.OnBackPressed += backButtonPressed;

        CameraStartPos = transform.position;
        CameraStartRot = transform.rotation;
            
    }

    // Update is called once per frame
    void Update()
    {
        switch (menuState) { 

        case cameraState.staticPlay:
              transform.position =  Vector3.Slerp(transform.position, CameraStartPos, 1.5f*Time.deltaTime);
              transform.rotation = Quaternion.Slerp(transform.rotation, CameraStartRot, 1.5f*Time.deltaTime);
                // if its back to start position then start countdown and send play message
                
        break;
        case cameraState.Settings:
                transform.position = Vector3.Slerp(transform.position, cameraSettingsPosition.position, 1.5f * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, cameraSettingsPosition.rotation, 1.5f * Time.deltaTime);
                break;

        case cameraState.rotate:
                transform.RotateAround(stump.transform.position, Vector3.up, rotationSpeed * Time.deltaTime);
                break;
        case cameraState.backButtonTransition:
                print("in rotat");
                transform.position = Vector3.Slerp(transform.position, CameraStartPos, 1.5f * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, CameraStartRot, 1.5f * Time.deltaTime);
                if (transform.position == CameraStartPos) menuState = cameraState.rotate;
                break;

        }


               
    }

    public void PlayButtonPressed()
    {
        //reset camera back to starting position 
       
        //transform.position = CameraStartPos;
        // transform.rotation = CameraStartRot;
 
        menuState = cameraState.staticPlay;

        DisableUi();

        //Needs to rotate back around to it but teleporting is fine for now

        
    }

    public void SettingsButtonPressed()
    {
        menuState = cameraState.Settings;
        UiTitle.SetActive(false);
        MainMenuButtons.SetActive(false);
        UiBackButton.SetActive(true);
    }

    public void backButtonPressed()
    {
        menuState = cameraState.backButtonTransition;
        UiTitle.SetActive(true);
        MainMenuButtons.SetActive(true);
        UiBackButton.SetActive(false);
    }


    public void DisableUi()
    {
        Ui.SetActive(false);
    }





}
