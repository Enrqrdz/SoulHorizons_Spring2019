using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class scr_Deck : MonoBehaviour {

	public int deckSize = 10;
    public int handSize = 4;
    public scr_NameToCard cardMapping; //maps card name to the scriptable object for that card
    public TextAsset deckList;
    [HideInInspector] public List<CardData> hand = new List<CardData>();
    [HideInInspector] public List<CardData> backupHand = new List<CardData>();
    public List<CardData> mantras = new List<CardData>(2);
    List<CardData> deck = new List<CardData>();
    List<CardData> discard = new List<CardData>();
    public List<KeyValuePair<string, int>> cardList = new List<KeyValuePair<string, int>>();

    public void Awake()
    {
        AllocateHandSlots();
        IntializeDeck();
    }

    private void AllocateHandSlots()
    {
        for (int i = 0; i < handSize; i++)
        {
            hand.Add(null);
            backupHand.Add(null);
        }
    }

    private void IntializeDeck()
    {
        LoadDeck();
    }

    /// <summary>
    /// Load the deck list from the existing inventory 
    /// </summary>
    void LoadDeckList(scr_Deck loadDeck)
    {
        deckSize = loadDeck.deckSize;
        handSize = loadDeck.handSize;
        cardMapping = loadDeck.cardMapping;
        hand = loadDeck.hand;
        deck = loadDeck.deck;
        discard = loadDeck.discard;
        cardList = loadDeck.cardList;

        ShuffleHelper<CardData>(deck);
        CheckHandSizeAndDraw();
    }

    /// <summary>
    /// Load the deck list from the inventory deck
    /// </summary>
    public void LoadDeck()
    {
        List<CardState> newDeck = SaveManager.currentGame.inventory.GetDeck();
        foreach (CardState cardState in newDeck)
        {
            CardData nextCard = cardState.GetCardData();
            if (nextCard == null)
            {
                continue;
            }
            //add that card to the list a number of times equal to the quantity
            for (int i = 0; i < cardState.numberOfCopies; i++)
            {
                deck.Add(nextCard);
            }
            cardList.Add(new KeyValuePair<string, int>(nextCard.cardName, cardState.numberOfCopies));
        }

        if (deck.Count != deckSize)
        {
            Debug.Log("DeckSize is " + deckSize + ", but " + deck.Count + " cards were added to the deck");
        }

        ShuffleHelper<CardData>(deck);
        CheckHandSizeAndDraw();
    }

    /// <summary>
    /// Pass a string telling the method what list to shuffle. Options are "deck", "discard", "discard into deck", and "all".
    /// </summary>
    /// <param name="list"></param>
    public void Shuffle(string list)
    {
        if (list.Equals("deck"))
        {
            ShuffleHelper<CardData>(deck);
        }
        else if(list.Equals("discard"))
        {
            ShuffleHelper<CardData>(discard);
            return;
        }
        else if(list.Equals("discard into deck"))
        {
            //move discard into deck
            foreach (CardData card in discard)
            {
                deck.Add(card);
                discard.Remove(card);
            }
            //shuffle
            ShuffleHelper<CardData>(deck);
        }
        else if(list.Equals("all"))
        {
            //move everything into deck
            foreach (CardData card in discard)
            {
                deck.Add(card);
                discard.Remove(card);
            }
            foreach (CardData card in hand)
            {
                deck.Add(card);
                hand.Remove(card);
            }

            //shuffle
            ShuffleHelper<CardData>(deck);
            //draw a new hand
            CheckHandSizeAndDraw();
        }
    }

    /// <summary>
    /// Shuffles the list provided.
    /// </summary>
    /// <param name="list"></param>
    void ShuffleHelper<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /// <summary>
    /// Make sure that the hand size is correct. Draw cards as necessary.
    /// </summary>
    void CheckHandSizeAndDraw()
    {
        int i = 0;
        foreach (CardData item in hand)
        {
            if(item == null)
            {
                Draw(i);
            }
            i++;
        }
    }

    /// <summary>
    /// Remove the top card from the deck and add it to the hand.
    /// </summary>
    /// <param name="index">The index to put the new card at</param>
    public void Draw(int index)
    {
        if (deck.Count > 0)
        {
            if (index == 0 || index == 1)
            {
                hand[index] = deck[0];
                deck.RemoveAt(0);
            }
            else
            {
                hand[2] = mantras[0];
                hand[3] = mantras[1];
            }           
        }
        else
        {
            //TODO: What do we do when the deck runs out?
            Shuffle("discard into deck");
        }
    }

    /// <summary>
    /// Play the selected card, then move it to the discard pile.
    /// </summary>
    /// <param name="index"></param>
    public void Activate(int index)
    {
        if (hand[index] == null)
        {
            return;
        }

        StartCoroutine(ActivateHelper(index));
    }

    private IEnumerator ActivateHelper(int index)
    {
        CardData cardToPlay = hand[index];
        //wait however long is required
        if (cardToPlay.castingTime != 0)
        {
            //start initial effects and stop player input
            cardToPlay.StartCastingEffects();
            scr_InputManager.cannotInput = true;
            yield return new WaitForSeconds(cardToPlay.castingTime);
            scr_InputManager.cannotInput = false;
        }

        hand[index].Activate();
        discard.Add(cardToPlay);
        hand[index] = null;
        //make sure hand size is correct
        CheckHandSizeAndDraw();
    }

    /// <summary>
    /// Play the selected card, then move it to the discard pile.
    /// </summary>
    /// <param name="index"></param>
    public void ActivateBackup(int index)
    {
        if (backupHand[index] == null)
        {
            return;
        }
        StartCoroutine(ActivateBackupHelper(index));
    }

    private IEnumerator ActivateBackupHelper(int index)
    {
        CardData cardToPlay = backupHand[index];
        //wait however long is required
        if (cardToPlay.castingTime != 0)
        {
            //start initial effects and stop player input
            cardToPlay.StartCastingEffects();
            scr_InputManager.cannotInput = true;
            yield return new WaitForSeconds(cardToPlay.castingTime);
        }
        scr_InputManager.cannotInput = false;
        backupHand[index].Activate();
        discard.Add(cardToPlay);
        //hand.Remove(cardToPlay);
        backupHand[index] = null;
    }

    /// <summary>
    /// Swaps the card at the given index in the hand with the backup slot for that card. If the backup slot is empty,
    /// the card is moved to the backup slot, then a new card is drawn to replace it.
    /// </summary>
    /// <param name="index"></param>
    public void Swap(int index)
    {
        //swap the card at this index with the card in its backup slot
        CardData temp = backupHand[index];
        backupHand[index] = hand[index];
        
        if (temp  == null)
        {
            //there was no card in the backup slot; need to draw a new card at this index in the hand
            Draw(index);
        }
        else
        {
            hand[index] = temp;
        }
    }
}
