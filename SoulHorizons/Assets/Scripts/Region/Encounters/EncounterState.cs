using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class EncounterState
{
    public int tier;
    public bool isCompleted, isAccessible;

    private int encounterIndexInPool;

    public EncounterState()
    {
        isCompleted = false;
        isAccessible = false;
        tier = 0;
        encounterIndexInPool = 0;
    }

    public void Clone(EncounterState encounter)
    {
        isCompleted = encounter.isCompleted;
        tier = encounter.tier;
        encounterIndexInPool = encounter.encounterIndexInPool;
    }

    public EncounterData GetEncounterData()
    {
        return EncounterPool.GetEncounterByTierAndIndex(tier, encounterIndexInPool);
    }

    public void Randomize()
    {
        encounterIndexInPool = EncounterPool.GetRandomEncounterIndexOfTier(tier);
    }
}