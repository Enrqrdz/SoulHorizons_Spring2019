using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectFinder : MonoBehaviour
{
    private static GameObject finder;

    public List<EncounterData> encounterPool = new List<EncounterData>();
    public List<ActionData> cardPool = new List<ActionData>();
    public StartingInventory startingInventory;

    public void Awake()
    {
        if(finder == null)
        {
            finder = gameObject;
            DontDestroyOnLoad(gameObject);
            LoadPools();
        }
        else    
            Destroy(gameObject);
       
    }

    private void LoadPools()
    {
        if(!EncounterPool.IsLoaded())
        {
            EncounterPool.AddEncounter(encounterPool);
            EncounterPool.SetLoaded(true);
        }

        if(!CardPool.IsLoaded())
        {
            CardPool.AddCard(cardPool);
            CardPool.SetLoaded(true);
        }
    }

    public List<CardState> GetStartingDeck()
    {
        List<CardState> startingDeck = new List<CardState>();

        int cardCount = startingInventory.startingInventoryCards.Count;

        for (int i = 0; i < cardCount; i++)
        {
            CardState newCardState = new CardState(startingInventory.startingInventoryCards[i], startingInventory.startingInventoryCardNumbers[i]);
            startingDeck.Add(newCardState);
        }

        return startingDeck;
    }
}
