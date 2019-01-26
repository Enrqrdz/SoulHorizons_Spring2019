using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EncounterPool
{
    private static List<List<Encounter>> encountersByTier = new List<List<Encounter>>();

    public static void AddEncounter(Encounter newEncounter)
    {
        if(IsNewTier(newEncounter.tier))
            CreateTier(newEncounter.tier);

        encountersByTier[newEncounter.tier].Add(newEncounter);
    }

    public static void AddEncounter(List<Encounter> newEncounters)
    {
        foreach(Encounter newEncounter in newEncounters)
        {
            if(IsNewTier(newEncounter.tier))
                CreateTier(newEncounter.tier);

            encountersByTier[newEncounter.tier].Add(newEncounter);
        }
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
            encountersByTier.Add(new List<Encounter>());
        }
    }

    public static Encounter GetEncounterByTierAndIndex(int encounterTier, int encounterIndex)
    {
        return encountersByTier[encounterTier][encounterIndex];
    }

    public static int GetRandomEncounterIndexOfTier(int tier)
    {
        return Random.Range(0, encountersByTier[tier].Count);
    }
}
