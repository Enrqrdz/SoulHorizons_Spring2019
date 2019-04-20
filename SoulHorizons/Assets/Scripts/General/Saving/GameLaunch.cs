using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLaunch : MonoBehaviour
{
    public void NewGame()
    {
        scr_SceneManager.globalSceneManager.ChangeScene(SceneNames.WORLDMAP);
    }

    public void Continue()
    {
        SaveManager.Load();
        scr_SceneManager.globalSceneManager.ChangeScene(SceneNames.WORLDMAP);
    }
}
