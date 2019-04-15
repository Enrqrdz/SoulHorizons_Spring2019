using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCardButton : MonoBehaviour
{
    public GameObject cardInfoPanel;

    public void OnClicked()
    {
        if(Input.GetButton("PlayCard3_Button"))
        {
            addCard();
        }
        else if(Input.GetButton("PlayCard4_Button"))
        {
            removeCard();
        }
        else if(Input.GetButton("PlayCard1_Button"))
        {
            inpectCard();
        }
    }

    public void addCard()
    {
        ActionUI myCard = gameObject.transform.parent.gameObject.GetComponent<ActionUI>();
        CardState cardState = new CardState(myCard.GetCardData(), 1);

        InventoryState inv = SaveManager.currentGame.inventory;

        if(inv.GetAmountOfCardInDeck(cardState) + 1 <= inv.GetAmountOfCardInInventory(cardState))
            SaveManager.currentGame.inventory.AddCardToDeck(cardState);
    }

    public void removeCard()
    {
        ActionUI myCard = gameObject.transform.parent.gameObject.GetComponent<ActionUI>();

        SaveManager.currentGame.inventory.RemoveCardFromDeck(new CardState(myCard.GetCardData(), 1));
    }

    public void inpectCard()
    {

    }
}
