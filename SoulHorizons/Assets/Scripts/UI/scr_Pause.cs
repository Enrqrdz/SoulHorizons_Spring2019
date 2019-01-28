//Author:
//Enrique Rodriguez
//Date:
//10/20/2018
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Pause : MonoBehaviour {

    private static bool paused = false;
    public GameObject pausePanel;

    private void Update () {
        PauseControl();
        ShowPausePanel();
	}

    private void PauseControl()
    {
        if (Input.GetButtonDown("Menu_Pause"))
        {
            paused = !paused;
            TogglePause();
        }
    }

    public static void TogglePause()
    {
        if (paused)
        {
            Time.timeScale = 0f;
            EnableInput();
        }

        else
        {
            Time.timeScale = 1f;
            DisableInput();
        }
    }

    private static void EnableInput()
    {
        scr_InputManager.disableInput = true;
    }

    private static void DisableInput()
    {
        scr_InputManager.disableInput = false;
    }

    private void ShowPausePanel()
    {
        pausePanel.SetActive(paused);
    }

    public static bool GetPaused()
    {
        return paused;
    }
}
