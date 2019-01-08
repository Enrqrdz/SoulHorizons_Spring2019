using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_InvButtons : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void addCard()
    {
        scr_CardUI myCard = gameObject.transform.parent.gameObject.GetComponent<scr_CardUI>();
        foreach(KeyValuePair<string, int> pair in scr_Inventory.deckList[scr_Inventory.deckIndex])
        {
            if(pair.Key == myCard.getName())
            {
                scr_Inventory.addCardToDeck(pair.Key);
            }
        }
    }

    public void removeCard()
    {
        scr_CardUI myCard = gameObject.transform.parent.gameObject.GetComponent<scr_CardUI>();
        foreach (KeyValuePair<string, int> pair in scr_Inventory.deckList[scr_Inventory.deckIndex])
        {
            if (pair.Key == myCard.getName())
            {
                scr_Inventory.removeCardFromDeck(pair.Key);
            }
        }
    }
}
