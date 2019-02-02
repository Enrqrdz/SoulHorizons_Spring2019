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
        scr_InputManager.cannotInput = true;
    }

    private static void DisableInput()
    {
        scr_InputManager.cannotInput = false;
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
