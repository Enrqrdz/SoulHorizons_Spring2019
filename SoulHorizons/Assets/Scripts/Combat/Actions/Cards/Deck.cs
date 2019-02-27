using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField]
    private List<CardData> deck = new List<CardData>();
    [SerializeField]
    private List<CardData> discard = new List<CardData>();
    [SerializeField]
	private int deckSize = 10;
    [SerializeField]
    private int handSize = 2;

    public List<CardData> hand = new List<CardData>();
    public List<CardData> backupHand = new List<CardData>();
    public CardDictionary cardMapping;
    public TextAsset deckTextList;
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
        if (InventoryManager.deckList.Count <= 0)
        {
            LoadNewDeck();
            SaveManager.Save();
        }
        else
        {
            LoadDeck();
        }
    }

    public void LoadNewDeck()
    {
        StringReader reader = new StringReader(deckTextList.text);
        string strLine;

        while ((strLine = reader.ReadLine()) != null)
        {
            string[] parsedLine = strLine.Split( ':');

            if (parsedLine.Length != 2)
            {
                continue;
            }
            
            parsedLine[1] = parsedLine[1].Trim();
            if(!Regex.IsMatch(parsedLine[1], @"^\d+$"))
            {
                continue;
            }
            int quantity = int.Parse(parsedLine[1]);

            CardData nextCard = cardMapping.ConvertNameToCard(parsedLine[0]);
            if (nextCard == null)
            {
                continue;
            }

            for(int i = 0; i < quantity; i++)
            {
                deck.Add(nextCard);
            }

            InventoryManager.addCard(nextCard, quantity);
            cardList.Add(new KeyValuePair<string, int>(nextCard.spellName, quantity));
        }

        if (deck.Count != deckSize)
        {
            Debug.Log("DeckSize is " + deckSize + ", but " + deck.Count + " cards were added to the deck");
        }

        ShuffleHelper<CardData>(deck);
        CheckHandSizeAndDraw();
        InventoryManager.addDeck(cardList);
    }

    public void LoadDeck()
    {
        List<KeyValuePair<string, int>> cards = InventoryManager.deckList[InventoryManager.currentDeckIndex];
        foreach (KeyValuePair<string, int> pair in cards)
        {
            CardData nextCard = cardMapping.ConvertNameToCard(pair.Key);
            if (nextCard == null)
            {
                continue;
            }

            for (int i = 0; i < pair.Value; i++)
            {
                deck.Add(nextCard);
            }
            cardList.Add(new KeyValuePair<string, int>(nextCard.spellName, pair.Value));
        }

        if (deck.Count != deckSize)
        {
            Debug.Log("DeckSize is " + deckSize + ", but " + deck.Count + " cards were added to the deck");
        }

        ShuffleHelper<CardData>(deck);
        CheckHandSizeAndDraw();
    }

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
            foreach (CardData card in discard)
            {
                deck.Add(card);
                discard.Remove(card);
            }
            ShuffleHelper<CardData>(deck);
        }
        else if(list.Equals("all"))
        {
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

            ShuffleHelper<CardData>(deck);
            CheckHandSizeAndDraw();
        }
    }

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
    
    public void Draw(int index)
    {
        if (deck.Count > 0)
        {
            hand[index] = deck[0];
            deck.RemoveAt(0);         
        }
        else
        {
            Shuffle("discard into deck");
        }
    }

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

        if (cardToPlay.castingTime != 0)
        {
            cardToPlay.StartCastingEffects();
            InputManager.cannotInput = true;
            yield return new WaitForSeconds(cardToPlay.castingTime);
            InputManager.cannotInput = false;
        }

        hand[index].Activate();
        discard.Add(cardToPlay);
        hand[index] = null;
        //make sure hand size is correct
        CheckHandSizeAndDraw();
    }

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
            InputManager.cannotInput = true;
            yield return new WaitForSeconds(cardToPlay.castingTime);
        }
        InputManager.cannotInput = false;
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
