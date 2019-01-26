using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public static class SaveManager 
{
    public static GameState currentGame;

    const string saveFileName = "SoulHorizonSave1";
    static string saveFilePath = Application.persistentDataPath + "/"+saveFileName+".gd";

    public static void NewSave()
    {
        currentGame = new GameState();
        currentGame.isLastGamePlayed = true;
       
        Save();
    }

    public static void Save()
    {
        currentGame.SaveInventory();

        SerializeDataToFile(currentGame, saveFilePath);
    }

    public static void Load()
    {
        if (File.Exists(saveFilePath))
        {
            currentGame = (GameState) DeserializeDataFromFile(saveFilePath);

            currentGame.LoadInventory();
        }
    }

    private static void SerializeDataToFile(object data, string filePath)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filePath);
        bf.Serialize(file, data);
        file.Close();
    }

    private static object DeserializeDataFromFile(string filePath)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(filePath, FileMode.Open);
        object data = bf.Deserialize(file);
        file.Close();
        return data;
    }
}