using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryState
{
    private List<CardState> cardInv = new List<CardState>();
    private List<CardState> deck = new List<CardState>();

    public void AddCardToInventory(List<CardState> newCardStates)
    {
        foreach(CardState cardState in  newCardStates)
        {
            AddCardToInventory(cardState);
        }
    }

    public void AddCardToInventory(CardState newCardState)
    {
        foreach(CardState cardState in cardInv)
        {
            if(cardState.IsTheSameCard(newCardState))
            {
                cardState.numberOfCopies += newCardState.numberOfCopies;
                return;
            }
        }

        cardInv.Add(new CardState(newCardState));
    }

    public void AddCardToDeck(List<CardState> newCardStates)
    {
        foreach(CardState cardState in newCardStates)
        {
            AddCardToDeck(cardState);
        }
    }

    public void AddCardToDeck(CardState newCardState)
    {
        foreach(CardState cardState in deck)
        {
            if(cardState.IsTheSameCard(newCardState))
            {
                cardState.numberOfCopies += newCardState.numberOfCopies;
                return;
            }
        }

        deck.Add(new CardState(newCardState));
    }

    public void RemoveCardFromDeck(CardState newCardState)
    {
        foreach(CardState cardState in deck)
        {
            if(cardState.IsTheSameCard(newCardState))
            {
                cardState.numberOfCopies -= newCardState.numberOfCopies;

                if(cardState.numberOfCopies <= 0)
                    deck.Remove(cardState);

                return;
            }
        }
    }

    public List<CardState> GetCardInventory()
    {
        return cardInv;
    }

    public List<CardState> GetDeck()
    {
        return deck;
    }

    public int GetDeckLength()
    {
        int count = 0;

        foreach(CardState cardState in deck)
        {
            count += cardState.numberOfCopies;
        }

        return count;
    }
}

