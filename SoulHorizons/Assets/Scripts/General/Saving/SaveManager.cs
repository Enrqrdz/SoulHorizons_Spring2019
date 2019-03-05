using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Runtime.Serialization;

public static class SaveManager 
{
    public static GameState currentGame;

    const string saveFileName = "SoulHorizonSave1";
    static string saveFilePath = Application.persistentDataPath + "/"+saveFileName+".gd";

    public static void NewSave()
    {
        currentGame = new GameState();
        Save();
    }

    public static void Save()
    {
        SerializeDataToFile(currentGame, saveFilePath);
    }

    public static void Load()
    {
        if (File.Exists(saveFilePath))
        {
            currentGame = (GameState) DeserializeDataFromFile(saveFilePath);
        }
    }

    private static void SerializeDataToFile(object data, string filePath)
    {
        BinaryFormatter bf = GetBinaryFormatter();
        FileStream file = File.Create(filePath);
        bf.Serialize(file, data);
        file.Close();
    }

    private static object DeserializeDataFromFile(string filePath)
    {
        BinaryFormatter bf = GetBinaryFormatter();
        FileStream file = File.Open(filePath, FileMode.Open);
        object data = bf.Deserialize(file);
        file.Close();
        return data;
    }

    private static BinaryFormatter GetBinaryFormatter()
    {
         BinaryFormatter bf = new BinaryFormatter();
 
         // 1. Construct a SurrogateSelector object
         SurrogateSelector ss = new SurrogateSelector();
         
         Vector3SerializationSurrogate v3ss = new Vector3SerializationSurrogate();
         ss.AddSurrogate(typeof(Vector3), 
                         new StreamingContext(StreamingContextStates.All), 
                         v3ss);
         
         // 2. Have the formatter use our surrogate selector
         bf.SurrogateSelector = ss;

         return bf;
    }
}