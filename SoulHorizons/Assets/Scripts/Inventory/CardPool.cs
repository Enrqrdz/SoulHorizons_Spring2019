using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CardPool
{
    private static List<CardData> cardPool;

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
}
