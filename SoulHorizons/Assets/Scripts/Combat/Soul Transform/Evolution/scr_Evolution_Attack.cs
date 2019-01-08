using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Evolution_Attack : MonoBehaviour {

	//Have a general cooldown to check in Update, then specific attack cooldowns depending on what the attack does?
	//Want to encourage the player to vary their
	private int meleeDamage = 12;
	private float meleeCooldown = 0.4f; //have these on separate cooldowns, so you can melee attack with the projectile in motion
	private int spiritDamage = 18;

	void Start () {
        Debug.Log("Evolution attack added");
	}
	
	void Update () {
		int input = scr_InputManager.PlayCard();

		switch (input)
		{
		    case 0:
		    	MeleeAttack();
			break;
		    case 1:
		    	LaunchSpirit();
		    	break;
		    case 2:
		    	SpiritAttack();
		    	break;
		    case 3:
		    	DashNSlash();
		    	break;
		}
	}

	/// <summary>
	/// Attack the space in front of the player
	/// </summary>
	private void MeleeAttack()
	{

	}

	private void LaunchSpirit()
	{

	}

	private void SpiritAttack()
	{

	}

	private void DashNSlash()
	{

	}
}
