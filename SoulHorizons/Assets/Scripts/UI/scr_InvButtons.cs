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
        ActionUI myCard = gameObject.transform.parent.gameObject.GetComponent<ActionUI>();
        foreach(KeyValuePair<string, int> pair in InventoryManager.deckList[InventoryManager.currentDeckIndex])
        {
            if(pair.Key == myCard.getName())
            {
                InventoryManager.addCardToDeck(pair.Key);
            }
        }
    }

    public void removeCard()
    {
        ActionUI myCard = gameObject.transform.parent.gameObject.GetComponent<ActionUI>();
        foreach (KeyValuePair<string, int> pair in InventoryManager.deckList[InventoryManager.currentDeckIndex])
        {
            if (pair.Key == myCard.getName())
            {
                InventoryManager.removeCardFromDeck(pair.Key);
            }
        }
    }
}
