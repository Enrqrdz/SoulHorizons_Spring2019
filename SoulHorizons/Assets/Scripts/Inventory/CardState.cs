using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardState
{
    public int numberOfCopies;

    private int cardIndexInPool;

    public CardState(CardState cardState)
    {
        cardIndexInPool = cardState.cardIndexInPool;
        numberOfCopies = cardState.numberOfCopies;
    }

    public CardState(CardData cardData, int numberOfCopies)
    {
        cardIndexInPool = CardPool.getIndexOfCardData(cardData);
        this.numberOfCopies = numberOfCopies;
    }

    public CardData GetCardData()
    {
        return CardPool.GetCardByIndex(cardIndexInPool);
    }

    public bool IsTheSameCard(CardState otherCard)
    {
        return (this.cardIndexInPool == otherCard.cardIndexInPool);
    }
}