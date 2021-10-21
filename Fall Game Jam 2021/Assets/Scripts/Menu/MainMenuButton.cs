using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
    public delegate void ButtonPressed();
    public static event ButtonPressed OnPlayPressed;
    public static event ButtonPressed OnHowPlayPressed;
    public static event ButtonPressed OnBackPressed;
   public void PlayButtonPressed(){

        OnPlayPressed?.Invoke();

    }

    public void HowToPlayPressed()
    {
        //transition to the how to play menu

        OnHowPlayPressed?.Invoke();

    }

    public void BackButtonPressed()
    {
        OnBackPressed?.Invoke();
    }

    public void ExitButtonPressed()
    {
        Application.Quit();
    }
}
