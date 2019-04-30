using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class EncounterState
{
    public int tier;
    public bool isCompleted, isAccessible;
    public EncounterType type = EncounterType.Combat;

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
        if(type == EncounterType.Boss)
            return EncounterPool.GetBossEncounterByIndex(encounterIndexInPool);
        else
            return EncounterPool.GetEncounterByTierAndIndex(tier, encounterIndexInPool);
        
    }

    public void Randomize()
    {
        if(type == EncounterType.Boss)
            encounterIndexInPool = EncounterPool.GetRandomBossEncounterIndex();
        else
            encounterIndexInPool = EncounterPool.GetRandomEncounterIndexOfTier(tier);

        Debug.Log(encounterIndexInPool);
    }
}