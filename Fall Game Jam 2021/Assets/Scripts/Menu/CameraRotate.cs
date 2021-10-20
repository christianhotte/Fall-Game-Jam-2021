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
        CameraStartPos = transform.position;
        CameraStartRot = transform.rotation;
            
    }

    // Update is called once per frame
    void Update()
    {
        switch (menuState) { 

        case cameraState.staticPlay:
              transform.position =  Vector3.Slerp(transform.position, CameraStartPos, .01f);
              transform.rotation = Quaternion.Slerp(transform.rotation, CameraStartRot, .01f);
                // if its back to start position then start countdown and send play message
                
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
        //reset camera back to starting position 
       
        //transform.position = CameraStartPos;
        // transform.rotation = CameraStartRot;
 
        menuState = cameraState.staticPlay;

        DisableUi();

        //Needs to rotate back around to it but teleporting is fine for now

        
    }


    public void DisableUi()
    {
        Ui.SetActive(false);
    }





}
