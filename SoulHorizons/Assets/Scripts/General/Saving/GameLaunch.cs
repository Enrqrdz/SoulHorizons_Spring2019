using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLaunch : MonoBehaviour
{
    public void NewGame()
    {
        SaveManager.NewSave();
        scr_SceneManager.globalSceneManager.ChangeScene(SceneNames.INTRODUCTION);
    }

    public void Continue()
    {
        SaveManager.Load();
        scr_SceneManager.globalSceneManager.ChangeScene(SceneNames.INTRODUCTION);
    }
}
