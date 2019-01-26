using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLaunch : MonoBehaviour 
{
    public void NewGame()
    {
        SaveManager.NewSave();
        EncounterController.globalEncounterController.OnNewGame();
        scr_SceneManager.globalSceneManager.ChangeScene("LocalMap");
    }

    public void Continue()
    {
        SaveManager.Load();
        scr_SceneManager.globalSceneManager.ChangeScene("LocalMap");
    }
}
