using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    private static bool isPaused = false;
    private static bool pausePressed = false;
    public GameObject pausePanel;

    private void Update ()
    {
        PauseControl();

        if (isPaused)
        {
            PauseGame();
        }

        else
        {
            PlayGame();
        }

        ShowPausePanel();
        
    }

    private void PauseControl()
    {
        if (Input.GetButtonDown("Menu_Pause"))
        {
            TogglePausePressed();
            TogglePause();
        }
    }

    public static void TogglePausePressed()
    {
        pausePressed = !pausePressed;
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
        EnableInput();
    }

    private static void DisableInput()
    {
        InputManager.cannotInputAnything = true;
        InputManager.cannotMove = true;
    }

    private static void EnableInput()
    {
        InputManager.cannotInputAnything = false;
        InputManager.cannotMove = false;
    }

    private void ShowPausePanel()
    {
        pausePanel.SetActive(pausePressed);
    }

    public static bool GetPaused()
    {
        return isPaused;
    }
}
