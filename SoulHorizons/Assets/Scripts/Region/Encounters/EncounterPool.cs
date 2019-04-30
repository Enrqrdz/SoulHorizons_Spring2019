using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EncounterPool
{
    private static List<List<EncounterData>> encountersByTier = new List<List<EncounterData>>();
    private static List<EncounterData> bossEncounters = new List<EncounterData>();
    private static bool isLoaded = false;

    public static void AddEncounter(EncounterData newEncounter)
    {
        if(newEncounter.type == EncounterType.Combat)
        {
            if(IsNewTier(newEncounter.tier))
                CreateTier(newEncounter.tier);

            encountersByTier[newEncounter.tier].Add(newEncounter);
        }
        else if(newEncounter.type == EncounterType.Boss)
        {
            bossEncounters.Add(newEncounter);
        }
    }

    public static void AddEncounter(List<EncounterData> newEncounters)
    {
        foreach(EncounterData newEncounter in newEncounters)
        {
            AddEncounter(newEncounter);
        }
    }

    public static EncounterData GetEncounterByTierAndIndex(int encounterTier, int encounterIndex)
    {
        return encountersByTier[encounterTier][encounterIndex];
    }

    public static EncounterData GetBossEncounterByIndex(int encounterIndex)
    {
        return bossEncounters[encounterIndex];
    }

    public static int GetRandomEncounterIndexOfTier(int tier)
    {
        if(tier > encountersByTier.Count)
            return Random.Range(0, encountersByTier[tier].Count);
        else    
            return 0;
    }

    public static int GetRandomBossEncounterIndex()
    {
        return Random.Range(0, bossEncounters.Count);
    }
  
    private static bool IsNewTier(int tier)
    {
        return encountersByTier.Count <= tier;
    }

    private static void CreateTier(int tier)
    {
        int missingTiers = tier - encountersByTier.Count;

        for(int i = 0; i <= missingTiers; i++)
        {
            encountersByTier.Add(new List<EncounterData>());
        }
    }

    public static int GetMaxDifficulty()
    {
        return encountersByTier.Count - 1;
    }

    public static bool IsLoaded()
    {
        return isLoaded;
    }

    public static void SetLoaded(bool loadedStatus)
    {
        isLoaded = loadedStatus;
    }
}