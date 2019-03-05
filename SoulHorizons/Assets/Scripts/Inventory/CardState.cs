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

    public CardState(ActionData cardData, int numberOfCopies)
    {
        cardIndexInPool = CardPool.GetIndexOfCardData(cardData);
        this.numberOfCopies = numberOfCopies;
    }

    public ActionData GetActionData()
    {
        return CardPool.GetCardByIndex(cardIndexInPool);
    }

    public bool IsTheSameCard(CardState otherCard)
    {
        return (this.cardIndexInPool == otherCard.cardIndexInPool);
    }
}