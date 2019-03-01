using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_InvButtons : MonoBehaviour
{
    public void addCard()
    {
        scr_CardUI myCard = gameObject.transform.parent.gameObject.GetComponent<scr_CardUI>();

        SaveManager.currentGame.inventory.AddCardToDeck(myCard.GetCardState());
    }

    public void removeCard()
    {
        scr_CardUI myCard = gameObject.transform.parent.gameObject.GetComponent<scr_CardUI>();

        SaveManager.currentGame.inventory.RemoveCardFromDeck(myCard.GetCardState());
    }
}
