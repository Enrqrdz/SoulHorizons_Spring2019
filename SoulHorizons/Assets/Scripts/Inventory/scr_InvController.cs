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

    void LoadInv()
    {
        try
        {
            foreach (CardState cardState in SaveManager.currentGame.GetCardList())
            {
                //attempt to retrieve the object reference from cardMapping
                CardData nextCard = CardPool.GetCardByIndex(cardState.cardIndexInPool);
                if (nextCard == null)
                {
                    continue;
                }
                //add the card and quantity to an inventory card list
                InventoryManager.addCard(nextCard, cardState.numberOfCopies);
            }
        }
        catch (NullReferenceException e)
        {
            Debug.Log("This is a " + e);
        }
    }
}
