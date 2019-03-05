using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardPool
{
    private static List<ActionData> cardPool = new List<ActionData>();

    public static void AddCard(ActionData newCard)
    {
        cardPool.Add(newCard);
    }

    public static void AddCard(List<ActionData> newCards)
    {
        cardPool.AddRange(newCards);
    }

    public static ActionData GetCardByIndex(int index)
    {
        return cardPool[index];
    }

    public static int getIndexOfCardData(ActionData cardData)
    {
        for(int i = 0; i < cardPool.Count; i++)
        {
            if (cardPool[i] == cardData)
            {
                return i;
            }
        }
        
        throw new System.ArgumentException("cardData is not in cardPool", "cardData");
    }
}
