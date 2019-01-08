using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Transform_BearUI : MonoBehaviour {

	//TODO: make an interface for the transform UI? Is there any benefit to standardization in this case?

	//a reference to the cooldown overlay script on the ability prefab. Long term the the ability prefab should have a script to control stuff and these
	//variables should be references to those instead, like the card_ui script for the hand.
	public scr_CooldownOverlay abilityX;
	public scr_CooldownOverlay abilityY;
	public scr_CooldownOverlay abilityA;
	public scr_CooldownOverlay abilityB;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//These functions all start the cooldown for their respective abilities
	public void cooldownX(float seconds)
	{
		abilityX.SetTime(seconds);
		abilityX.StartCooldown();
	}

	public void cooldownY(float seconds)
	{
		abilityY.SetTime(seconds);
		abilityY.StartCooldown();
	}

	public void cooldownA(float seconds)
	{
		abilityA.SetTime(seconds);
		abilityA.StartCooldown();
	}

	public void cooldownB(float seconds)
	{
		abilityB.SetTime(seconds);
		abilityB.StartCooldown();
	}

	public void cooldownAll(float seconds)
	{
		cooldownA(seconds);
		cooldownB(seconds);
		cooldownX(seconds);
		cooldownY(seconds);
	}
}
