using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class scr_Inventory{

    public static int dustNum = 0; //How much dust you have
    public static List<KeyValuePair<scr_Card, int>> cardInv = new List<KeyValuePair<scr_Card, int>>(); //Your list of cards
    public static List<List<KeyValuePair<string, int>>> deckList = new List<List<KeyValuePair<string, int>>>(); //Your decks
    public static int deckIndex = 0; //Index of currently equipped deck
    public static int numDecks = 0; //number of decks

    /*public static void addDeck(scr_Deck deck)
    {
        Debug.Log("DECK ADDED");
        deckList.Add(deck);
        Debug.Log("CHECKING DECKS: " + deckList.Count);
        Debug.Log(deckList[0]);
    }*/

    public static void addDeck(List<KeyValuePair<string, int>> deck)
    {
        Debug.Log("DECK ADDED");
        deckList.Add(deck);
        Debug.Log("CHECKING DECKS: " + deckList.Count);
        Debug.Log(deckList[0]);
        numDecks++;
    }
    //ADDS CARD TO INVENTORY
    public static void addCard(scr_Card card, int quantity)
    {
        Debug.Log("ADDING CARD");
        foreach(KeyValuePair<scr_Card, int> pair in cardInv)
        {
            if(pair.Key.cardName == card.cardName)
            {
                int prevNum = pair.Value;
                cardInv.Remove(pair);
                cardInv.Add(new KeyValuePair<scr_Card, int>(card, quantity + prevNum));
                return;
            }
        }
        cardInv.Add(new KeyValuePair<scr_Card, int>(card, quantity));
    }

    //ADDS A SINGLE CARD TO A DECK
    public static void addCardToDeck(string cardName)
    {
        foreach (KeyValuePair<string, int> pair in deckList[deckIndex])
        {
            if (pair.Key == cardName)
            {
                int prevNum = pair.Value;
                if(prevNum + 1 > cardInv[getIndex(cardName)].Value)
                {
                    Debug.Log("CAN'T ADD TO FROM NOTHING");
                    return;
                }
                deckList[deckIndex].Remove(pair);
                deckList[deckIndex].Add(new KeyValuePair<string, int>(pair.Key, 1 + prevNum));
                return;
            }
        }
        Debug.Log("CARD CAN'T BE ADDED, NOT FOUND");
    }

    //REMOVES A SINGLE CARD TO A DECK
    public static void removeCardFromDeck(string cardName)
    {
        foreach (KeyValuePair<string, int> pair in deckList[deckIndex])
        {
            if (pair.Key == cardName)
            {
                int prevNum = pair.Value;
                if(prevNum - 1 < 0)
                {
                    Debug.Log("CAN'T TAKE AWAY FROM NOTHING");
                    return;
                }
                deckList[deckIndex].Remove(pair);
                deckList[deckIndex].Add(new KeyValuePair<string, int>(pair.Key, prevNum-1));
                return;
            }
        }
        Debug.Log("CARD CAN'T BE ADDED, NOT FOUND");
    }

    public static void addDust(int num)
    {
        dustNum += num;
    }

    //returns serializable object of cardInv
    public static List<KeyValuePair<string, int>> getCardInv()
    {
        List<KeyValuePair<string, int>> cardList = new List<KeyValuePair<string, int>>();
        foreach (KeyValuePair<scr_Card, int> pair in cardInv)
        {
            cardList.Add(new KeyValuePair<string, int>(pair.Key.cardName, pair.Value));
        }
        return cardList;
    }
    //returns serializable object of deck
    public static List<List<KeyValuePair<string, int>>> getDeckList()
    {
        /*List<List<KeyValuePair<string, int>>> decks = new List<List<KeyValuePair<string, int>>>();
        foreach(scr_Deck myDeck in deckList)
        {
            decks.Add(myDeck.cardList);
        }*/
        return deckList;
    }

    //gets the index of the card in card inventory
    public static int getIndex(string cardName)
    {
        for (int i = 0; i < cardInv.Count; i++)
        {
            if (cardInv[i].Key.cardName == cardName)
            {
                return i;
            }
        }
        Debug.Log("CAN'T FIND INDEX");
        return -1;
    }
 
    public static int getDeckSize()
    {
        int deckSize = 0;
        foreach (KeyValuePair<string, int> pair in deckList[deckIndex])
        {
            deckSize += pair.Value;
        }
        return deckSize;

    }

}
