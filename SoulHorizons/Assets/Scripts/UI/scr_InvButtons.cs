using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_InvButtons : MonoBehaviour
{
    public void addCard()
    {
        ActionUI myCard = gameObject.transform.parent.gameObject.GetComponent<ActionUI>();

        SaveManager.currentGame.inventory.AddCardToDeck(myCard.GetCardState());
    }

    public void removeCard()
    {
        ActionUI myCard = gameObject.transform.parent.gameObject.GetComponent<ActionUI>();

        SaveManager.currentGame.inventory.RemoveCardFromDeck(myCard.GetCardState());
    }
}
