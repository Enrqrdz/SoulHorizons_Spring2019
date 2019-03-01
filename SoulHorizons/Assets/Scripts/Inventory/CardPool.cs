using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardPool
{
    private static List<CardData> cardPool = new List<CardData>();

    public static void AddCard(CardData newCard)
    {
        cardPool.Add(newCard);
    }

    public static void AddCard(List<CardData> newCards)
    {
        cardPool.AddRange(newCards);
    }

    public static CardData GetCardByIndex(int index)
    {
        return cardPool[index];
    }

    public static int getIndexOfCardData(CardData cardData)
    {
        for(int i = 0; i < cardPool.Count; i++)
        {
            if(cardPool[i] == cardData)
            {
                return i;
            }
        }

        throw new System.ArgumentException("cardData is not in cardPool", "cardData");
    }
}
