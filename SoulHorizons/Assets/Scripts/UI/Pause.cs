﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

    private static bool isPaused = false;
    public GameObject pausePanel;

    private void Update ()
    {
        PauseControl();
        ShowPausePanel();
	}

    private void PauseControl()
    {
        if (Input.GetButtonDown("Menu_Pause"))
        {
            TogglePause();

            if (isPaused)
            {
                PauseGame();
            }

            else
            {
                PlayGame();
            }
        }
    }

    public static void TogglePause()
    {
        isPaused = !isPaused;
    }

    private static void PauseGame()
    {
        Time.timeScale = 0f;
        DisableInput();
    }

    private static void PlayGame()
    {
        Time.timeScale = 1f;
        DisableInput();
    }

    private static void DisableInput()
    {
        InputManager.cannotInput = true;
        InputManager.cannotMove = true;
    }

    private static void EnableInput()
    {
        InputManager.cannotInput = false;
        InputManager.cannotMove = false;
    }

    private void ShowPausePanel()
    {
        pausePanel.SetActive(isPaused);
    }

    public static bool GetPaused()
    {
        return isPaused;
    }
}
