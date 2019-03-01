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
        CardPool.AddCard(cardPool);
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
