using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
    public delegate void ButtonPressed();
    public static event ButtonPressed OnPlayPressed;
    //public static event ButtonPressed OnHowPlayPressed;
    //public static event ButtonPressed onQuitPressed;
   public void PlayButtonPressed(){ 
        
        if(OnPlayPressed != null)
        {
            OnPlayPressed();
        }

    }

    public void HowToPlayPressed()
    {
        //transition to the how to play menu

    }
    public void ExitButtonPressed()
    {
        //exit game
    }
}
