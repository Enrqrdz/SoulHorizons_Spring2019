using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_InvButtons : MonoBehaviour
{
    public void addCard()
    {
        ActionUI myCard = gameObject.transform.parent.gameObject.GetComponent<ActionUI>();
        CardState cardState = myCard.GetCardState();

        InventoryState inv = SaveManager.currentGame.inventory;

        if(inv.GetAmountOfCardInDeck(cardState) + 1 <= inv.GetAmountOfCardInInventory(cardState))
            SaveManager.currentGame.inventory.AddCardToDeck(cardState);
    }

    public void removeCard()
    {
        ActionUI myCard = gameObject.transform.parent.gameObject.GetComponent<ActionUI>();

        SaveManager.currentGame.inventory.RemoveCardFromDeck(myCard.GetCardState());
    }
}
