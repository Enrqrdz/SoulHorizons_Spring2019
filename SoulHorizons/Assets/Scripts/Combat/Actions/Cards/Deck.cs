using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private const int handSize = 2;

    private Queue<ActionData> deck = new Queue<ActionData>();
    private List<ActionData> discard = new List<ActionData>();
    public List<ActionData> hand = new List<ActionData>();

    public void Awake()
    {
        AllocateHandSlots();
        LoadDeck();
    }

    private void AllocateHandSlots()
    {
        for (int i = 0; i < handSize; i++)
        {
            hand.Add(null);
        }
    }

    public void LoadDeck()
    {
        List<CardState> newDeck = SaveManager.currentGame.inventory.GetDeck();
        foreach (CardState cardState in newDeck)
        {
            ActionData nextCard = cardState.GetActionData();

            if (nextCard == null)
            {
                continue;
            }

            //add that card to the list a number of times equal to the quantity
            for (int i = 0; i < cardState.numberOfCopies; i++)
            {
                discard.Add(nextCard);
            }
        }

        ShuffleDiscardIntoDeck();
        CheckHandSizeAndDraw();
    }

    public void ShuffleDiscardIntoDeck()
    {
        ShuffleHelper<ActionData>(discard);

        foreach(ActionData card in discard)
        {
            deck.Enqueue(card);
        }

        discard.Clear();
    }

    void ShuffleHelper<T>(List<T> list)
    {
        for( int i = 0; i < list.Count; i ++)
        {
             int j = Random.Range(i, list.Count );
             T temporary = list[i];
             list[i] = list[j];
             list[j] = temporary;
        }
    }

    void CheckHandSizeAndDraw()
    {
        for(int i = 0; i < handSize; i++)
        {
            if(hand[i] == null)
            {
                Draw(i);
            }
        }
    }
    
    public void Draw(int index)
    {
        if (deck.Count > 0)
        {
            hand[index] = deck.Dequeue();        
        }
    }

    public ActionData Activate(int index)
    {
        ActionData activatedCard = hand[index];
        
        if (activatedCard == null)
        {
            ShuffleDiscardIntoDeck();
            CheckHandSizeAndDraw();
        }
        else
        {
            StartCoroutine(ActivateHelper(index));
        }

        return activatedCard;
    }

    private IEnumerator ActivateHelper(int index)
    {
        if (hand[index].castingTime != 0)
        {
            InputManager.canInputCards = false;
            yield return new WaitForSeconds(hand[index].castingTime);
            InputManager.canInputCards = true;
        }

        hand[index].Activate();
        discard.Add(hand[index]);
        hand[index] = null;

        CheckHandSizeAndDraw();
    }

    public IEnumerator GetDeckEnumerator()
    {
        return deck.GetEnumerator();
    }
}
