using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public static Deck Instance { get; private set; }

    [SerializeField]
    private List<ActionData> deck = new List<ActionData>();
    [SerializeField]
    private List<ActionData> discard = new List<ActionData>();
    [SerializeField]
	private int deckSize = 10;
    [SerializeField]
    private int handSize = 2;

    public List<ActionData> hand = new List<ActionData>();
    public List<ActionData> backupHand = new List<ActionData>();
    public CardDictionary cardMapping;
    public TextAsset deckTextList;
    public List<KeyValuePair<string, int>> cardList = new List<KeyValuePair<string, int>>();

    public void Awake()
    {
        IntiializeSingleton();
        AllocateHandSlots();
        IntializeDeck();
    }

    private void IntiializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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

            ActionData nextCard = cardMapping.ConvertNameToCard(parsedLine[0]);
            if (nextCard == null)
            {
                continue;
            }

            for(int i = 0; i < quantity; i++)
            {
                deck.Add(nextCard);
            }

            InventoryManager.addCard(nextCard, quantity);
            cardList.Add(new KeyValuePair<string, int>(nextCard.actionName, quantity));
        }

        if (deck.Count != deckSize)
        {
            Debug.Log("DeckSize is " + deckSize + ", but " + deck.Count + " cards were added to the deck");
        }

        ShuffleHelper<ActionData>(deck);
        CheckHandSizeAndDraw();
        InventoryManager.addDeck(cardList);
    }

    public void LoadDeck()
    {
        List<KeyValuePair<string, int>> cards = InventoryManager.deckList[InventoryManager.currentDeckIndex];
        foreach (KeyValuePair<string, int> pair in cards)
        {
            ActionData nextCard = cardMapping.ConvertNameToCard(pair.Key);
            if (nextCard == null)
            {
                continue;
            }

            for (int i = 0; i < pair.Value; i++)
            {
                deck.Add(nextCard);
            }
            cardList.Add(new KeyValuePair<string, int>(nextCard.actionName, pair.Value));
        }

        if (deck.Count != deckSize)
        {
            Debug.Log("DeckSize is " + deckSize + ", but " + deck.Count + " cards were added to the deck");
        }

        ShuffleHelper<ActionData>(deck);
        CheckHandSizeAndDraw();
    }

    public void Shuffle(string list)
    {
        if (list.Equals("deck"))
        {
            ShuffleHelper<ActionData>(deck);
        }
        else if(list.Equals("discard"))
        {
            ShuffleHelper<ActionData>(discard);
            return;
        }
        else if(list.Equals("discard into deck"))
        {
            foreach (ActionData card in discard)
            {
                deck.Add(card);
                discard.Remove(card);
            }
            ShuffleHelper<ActionData>(deck);
        }
        else if(list.Equals("all"))
        {
            foreach (ActionData card in discard)
            {
                deck.Add(card);
                discard.Remove(card);
            }
            foreach (ActionData card in hand)
            {
                deck.Add(card);
                hand.Remove(card);
            }

            ShuffleHelper<ActionData>(deck);
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
        foreach (ActionData item in hand)
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
        if (hand[index].castingTime != 0)
        {
            InputManager.cannotInputAnything = true;
            yield return new WaitForSeconds(hand[index].castingTime);
            InputManager.cannotInputAnything = false;
        }

        hand[index].Activate();
        discard.Add(hand[index]);
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
        ActionData cardToPlay = backupHand[index];
        //wait however long is required
        if (cardToPlay.castingTime != 0)
        {
            //start initial effects and stop player input
            InputManager.canInputCards = false;
            yield return new WaitForSeconds(cardToPlay.castingTime);
        }
        InputManager.canInputCards = true;
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
        ActionData temp = backupHand[index];
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
