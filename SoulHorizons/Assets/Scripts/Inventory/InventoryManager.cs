using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class InventoryManager
{
    public static List<KeyValuePair<CardData, int>> cardInv = new List<KeyValuePair<CardData, int>>();
    public static List<List<KeyValuePair<string, int>>> deckList = new List<List<KeyValuePair<string, int>>>();
    public static int currentDeckIndex = 0;

    public static void addDeck(List<KeyValuePair<string, int>> deck)
    {
        deckList.Add(deck);
    }
    
    public static void addCard(CardData card, int quantity)
    {
        foreach(KeyValuePair<CardData, int> pair in cardInv)
        {
            if(pair.Key.cardName == card.cardName)
            {
                int prevNum = pair.Value;
                cardInv.Remove(pair);
                cardInv.Add(new KeyValuePair<CardData, int>(card, quantity + prevNum));
                return;
            }
        }
        cardInv.Add(new KeyValuePair<CardData, int>(card, quantity));
    }

    public static void addCardToDeck(string cardName)
    {
        foreach (KeyValuePair<string, int> pair in deckList[currentDeckIndex])
        {
            if (pair.Key == cardName)
            {
                int prevNum = pair.Value;
                if(prevNum + 1 > cardInv[getIndexByCardName(cardName)].Value)
                {
                    return;
                }
                deckList[currentDeckIndex].Remove(pair);
                deckList[currentDeckIndex].Add(new KeyValuePair<string, int>(pair.Key, 1 + prevNum));
                return;
            }
        }
    }

    public static void removeCardFromDeck(string cardName)
    {
        foreach (KeyValuePair<string, int> pair in deckList[currentDeckIndex])
        {
            if (pair.Key == cardName)
            {
                int prevNum = pair.Value;
                if(prevNum - 1 < 0)
                {
                    return;
                }
                deckList[currentDeckIndex].Remove(pair);
                deckList[currentDeckIndex].Add(new KeyValuePair<string, int>(pair.Key, prevNum-1));
                return;
            }
        }
    }

    public static List<KeyValuePair<string, int>> getCardInv()
    {
        List<KeyValuePair<string, int>> cardList = new List<KeyValuePair<string, int>>();
        foreach (KeyValuePair<CardData, int> pair in cardInv)
        {
            cardList.Add(new KeyValuePair<string, int>(pair.Key.cardName, pair.Value));
        }
        return cardList;
    }

    public static List<List<KeyValuePair<string, int>>> getDeckList()
    {
        return deckList;
    }

    public static int getIndexByCardName(string cardName)
    {
        for (int i = 0; i < cardInv.Count; i++)
        {
            if (cardInv[i].Key.cardName == cardName)
            {
                return i;
            }
        }
        return -1;
    }
 
    public static int getDeckSize()
    {
        int deckSize = 0;
        foreach (KeyValuePair<string, int> pair in deckList[currentDeckIndex])
        {
            deckSize += pair.Value;
        }
        return deckSize;
    }
}
