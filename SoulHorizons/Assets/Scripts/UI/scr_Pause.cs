//Author:
//Enrique Rodriguez
//Date:
//10/20/2018
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Pause : MonoBehaviour {


    private static bool paused = false;         //Determines if the game is paused. Must be False to work properly

    public GameObject pausePanel;               //Pause Panel when paused

    private void Start()
    {
       
    }

    void Update () {
        pauseControl();
        togglePanel();
	}

    //Controls pause flow
    void pauseControl()
    {
        if (Input.GetButtonDown("Menu_Pause"))
        {
            paused = !paused;
            togglePause();
        }
    }

    //Toggles pause
    void togglePause()
    {
        //If you hit escape, it pauses
        if (paused)
        {
            Debug.Log("You've paused the game");
            //Show pause panel
            Time.timeScale = 0f;
            scr_InputManager.disableInput = true;
        }

        //If you hit escape again, it unpauses
        else
        {
            //Remove pause panel
            Time.timeScale = 1f;
            scr_InputManager.disableInput = false;
        }
    }

    //Get paused value
    public static bool getPaused()
    {
        return paused;
    }

    //Set paused value
    public static void setPaused(bool value)
    {
        paused = value;
    }

    //Toggles pause panel
    void togglePanel()
    {
        pausePanel.SetActive(paused);
    }
}
