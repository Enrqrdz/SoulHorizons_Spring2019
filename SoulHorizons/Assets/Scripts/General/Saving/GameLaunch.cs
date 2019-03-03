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

        List<CardState> startingDeck = gameObject.GetComponent<ScriptableObjectFinder>().GetStartingDeck();
        SaveManager.currentGame.inventory.AddCardToInventory(startingDeck);
        SaveManager.currentGame.inventory.AddCardToDeck(startingDeck);

        scr_SceneManager.globalSceneManager.ChangeScene(SceneNames.REGION);
    }

    public void Continue()
    {
        SaveManager.Load();
        scr_SceneManager.globalSceneManager.ChangeScene(SceneNames.REGION);
    }
}
