using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLaunch : MonoBehaviour 
{
    public RegionGenerator regionGenerator;

    public void NewGame()
    {
        SaveManager.NewSave();
        SaveManager.currentGame.SetRegion(regionGenerator.GenerateRegion());
        scr_SceneManager.globalSceneManager.ChangeScene(SceneNames.REGION);
    }

    public void Continue()
    {
        SaveManager.Load();
        scr_SceneManager.globalSceneManager.ChangeScene(SceneNames.REGION);
    }
}
