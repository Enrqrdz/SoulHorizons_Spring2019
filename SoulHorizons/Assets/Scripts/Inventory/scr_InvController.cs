using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class scr_InvController : MonoBehaviour {

    public scr_NameToCard cardMapping;
    public static scr_InvController invController;
    public TextAsset deckList;


    private void Awake()
    {
        if (invController != null && invController != this)
        {
            Destroy(gameObject);
        }
        else if(invController == null)
        {
            invController = this;
            DontDestroyOnLoad(this.gameObject);
            LoadInv();
        }
      
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void LoadInv()
    {

        Debug.Log("Loading Inventory");
        //LOAD CARD LIST
        try
        {
            foreach (KeyValuePair<string, int> pair in SaveLoad.currentGame.GetCardList())
            {
                //attempt to retrieve the object reference from cardMapping
                scr_Card nextCard = cardMapping.ConvertNameToCard(pair.Key);
                if (nextCard == null)
                {
                    continue;
                }
                //add the card and quantity to an inventory card list
                scr_Inventory.addCard(nextCard, pair.Value);
            }
        }
        catch (NullReferenceException e)
        {
            Debug.Log("This is a " + e);
        }

        //MAKES A NEW DECK IF NEW GAME
        /*if (scr_Inventory.numDecks == 0)
        {
            Debug.Log("NEW DECK INCOMING");
            scr_Deck newDeck = new scr_Deck();
            /*newDeck.deckList = deckList;
            newDeck.cardMapping = cardMapping;
            newDeck.LoadNewDeck();
            scr_Inventory.numDecks++;
            SaveLoad.Save();
        }
        //ELSE LOADS DECKS
        else
        {
            Debug.Log("LOADING EXISTING DECKS");
            foreach (List<KeyValuePair<string, int>> myDeck in SaveLoad.currentGame.GetDeckList())
            {
                scr_Deck newDeck = new scr_Deck();
                newDeck.cardMapping = cardMapping;
                newDeck.LoadDeck(myDeck);
               
            }
        }*/

    }
}
