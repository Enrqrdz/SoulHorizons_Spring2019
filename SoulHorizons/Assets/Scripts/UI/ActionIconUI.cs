using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionIconUI : MonoBehaviour
{
    [SerializeField] private Image cardArt;
    private scr_CooldownOverlay cooldownOverlay;
    private CardState cardState;

    void Awake ()
    {
		cooldownOverlay = GetComponent<scr_CooldownOverlay>();
	}

    public void SetArt(Sprite art)
	{
		cardArt.sprite = art;
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

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
