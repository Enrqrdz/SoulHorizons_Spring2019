using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionUI : MonoBehaviour 
{
	[Header("Must Be Set")]
	[SerializeField] private TextMeshProUGUI cardName;
	[SerializeField] private TextMeshProUGUI countText;
	[SerializeField] private Image cardArt;

	private bool isSelected = false;

	private ActionData cardData;

	public void SetCountText(string name)
	{
		countText.text = name;
	}

	public void SetActionData(ActionData newActionData)
	{
		cardData = newActionData;
		cardName.text = cardData.actionName;
		cardArt.sprite = cardData.art;
	}

    public ActionData GetCardData()
    {
        return cardData;
    }

	public void SetInactive()
	{
		gameObject.SetActive(false);
	}

	public void SetActive()
	{
		gameObject.SetActive(true);
	}
}
