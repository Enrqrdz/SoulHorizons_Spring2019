using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
//Colin 9/15/18

public static class SaveLoad {

    public static GameState[] savedGames = new GameState[3];
    public static GameState currentGame;

    

    /// <summary>
    /// Create a new save file. Limit how many they can have?
    /// </summary>
    public static void NewGame()
    {
        GameState newGame = new GameState();
        currentGame = newGame;
        newGame.lastGamePlayed = true;
        bool gameStateInserted = false;

        //insert the  GameState into an empty spot into the array and make sure none of the others are marked last game played.
        //TODO: What if they click new game and three save files already exist?
        for (int i = 0; i < 3; i++)
        {
            if (savedGames[i] == null && !gameStateInserted)
            {
                savedGames[i] = newGame;
                gameStateInserted = true;
            }
            else if (savedGames[i] != null)
            {
                savedGames[i].lastGamePlayed = false;
            }
        }
        Save();
        scr_EncounterController.globalEncounterController.OnNewGame(); 

    }

    public static void Save()
    {
        try
        {
            currentGame.SaveInventory();

        }
        catch (NullReferenceException e)
        {
            Debug.Log("This is a " + e);
        }


        
        savedGames[0] = currentGame; 
        //TODO:Need to add GameState to list?
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd"); //you can call it anything you want
        bf.Serialize(file, savedGames);
        file.Close();
    }

    /// <summary>
    /// Loads the list from a file and puts it into an object
    /// </summary>
    public static void Load()
    {
        Debug.Log("Load");
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            savedGames = (GameState[])bf.Deserialize(file);
            file.Close();

            //set lastPlayed game to currentGame for the continue option in the menu
            foreach (GameState item in savedGames)
            {
                if (item.lastGamePlayed)
                {
                    currentGame = item;
                    currentGame.LoadInventory();
                    Debug.Log("Loaded game " + currentGame.GetPlayerName());
                    Debug.Log("DUST AMOUNT: " + currentGame.GetDustAmount());
                    scr_SceneManager.globalSceneManager.ChangeScene("sn_LocalMap");
                    return;
                }
            }


        }
        //else no save file exists
        Debug.Log("No save file exists");
    }

    public static void Clear()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            File.Delete(Application.persistentDataPath + "/savedGames.gd");
        }
    }
}


