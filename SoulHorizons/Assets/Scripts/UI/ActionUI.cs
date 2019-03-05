using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This script is attached to each card in the UI. The deck manager script has a reference to this and uses it to update the UI
/// </summary>
[RequireComponent(typeof(scr_CooldownOverlay))]
public class ActionUI : MonoBehaviour {
	//--UI Components--
	[SerializeField] private TextMeshProUGUI cardName;
	[SerializeField] private TextMeshProUGUI description;
	[SerializeField] private Image cardArt;
	[SerializeField] private Image cardElement;
	[SerializeField] private TextMeshProUGUI backupName;
	private Element element;

	//--Color Settings--
	[SerializeField] private Color earthColor;
	[SerializeField] private Color soulColor;
	[SerializeField] private Color sunColor;
	[SerializeField] private Color voidColor;
	[SerializeField] private Color windColor;

	[SerializeField] private Color selectedColor;
	[SerializeField] private Color notSelectedColor;
	private scr_CooldownOverlay cooldownOverlay;

	private bool selected = false; //whether this card is currently selected or not

	private CardState cardState;
	
	void Awake ()
    {
		cooldownOverlay = GetComponent<scr_CooldownOverlay>();
	}

	/// <summary>
	/// Sets the name for the 
	/// </summary>
	/// <param name="name"></param>
	public void SetBackupName(string name)
	{
		backupName.text = name;
	}

	public void SetName(string name)
	{
		cardName.text = name;
	}

    public string getName()
    {
        return cardName.text;
    }

	public void SetDescription(string description)
	{
		this.description.text = description;
	}

	/// <summary>
	/// Use to set color elements for cards
	/// </summary>
	/// <param name="element"></param>
	public void SetElement(Element element)
	{
		this.element = element;
		//change the color to match
		switch (element)
		{
			case Element.Earth:
				cardElement.color = earthColor;
				break;
			case Element.Soul:
				cardElement.color = soulColor;
				break;
			case Element.Sun:
				cardElement.color = sunColor;
				break;
			case Element.Void:
				cardElement.color = voidColor;
				break;
			case Element.Wind:
				cardElement.color = windColor;
				break;
		}
	}

	public void SetArt(Sprite art)
	{
		cardArt.sprite = art;
	}

	public void  SetSelected(bool selected)
	{
		if (this.selected == selected)
		{
			return;
		}
		//if we're proceeding, then that means a change is occurring

		if (selected)
		{
			//the card is now selected
			cardName.color = selectedColor;
		}
		else
		{
			//the card is no longer selected
			cardName.color = notSelectedColor;
		}
		
		this.selected = selected;
	}

	public void StartCooldown(float seconds)
	{
        cooldownOverlay.SetTime(seconds);
		cooldownOverlay.StartCooldown();
	}

	public void SetCardState(CardState newCardState)
	{
		cardState = newCardState;
	}

    public CardState GetCardState()
    {
        return cardState;
    }
}
