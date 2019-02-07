using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectFinder : MonoBehaviour
{
    public List<EncounterData> encounterPool = new List<EncounterData>();
    public List<CardData> cardPool = new List<CardData>();
    public StartingInventory startingInventory;

    public void Start()
    {
        EncounterPool.AddEncounter(encounterPool);
    }
}
