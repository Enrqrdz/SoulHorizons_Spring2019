using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectFinder : MonoBehaviour
{
    public List<EncounterData> encounterPool = new List<EncounterData>();
    public List<ActionData> cardPool = new List<ActionData>();
    public StartingInventory startingInventory;

    public void Start()
    {
        EncounterPool.AddEncounter(encounterPool);
    }
}
