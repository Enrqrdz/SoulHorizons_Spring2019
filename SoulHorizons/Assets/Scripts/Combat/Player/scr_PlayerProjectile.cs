//Colin
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//OLD, DOES NOT USE GRID

/// <summary>
/// Attached to the projectile the player fires.
/// </summary>
public class scr_PlayerProjectile : MonoBehaviour {
	private float speed;
	private float damage;
	private int chargeLevel;
	private Rigidbody2D rigid2d;

	void Awake()
	{
		rigid2d = GetComponent<Rigidbody2D>();
	}

	/// <summary>
	/// Fires the projectile
	/// </summary>
	/// <param name="damage">damage dealt on impact</param>
	/// <param name="chargeLevel">determines visual effects</param>
	public void Fire(float damage, int chargeLevel, float speed){
		Debug.Log("PlayerProjectile::Fire - damage: " + damage + " chargeLevel: " + chargeLevel + " speed: " + speed);
		LaunchEffects(chargeLevel);
		this.speed = speed;
		this.damage = damage;
		this.chargeLevel = chargeLevel;
		rigid2d.velocity = new Vector2(speed, 0);
	}

	/// <summary>
	/// Initial visual effects
	/// </summary>
	/// <param name="chargeLevel"></param>
    private void LaunchEffects(int chargeLevel)
    {
        if (chargeLevel == 1)
		{
			gameObject.GetComponent<SpriteRenderer>().color = Color.red;
		}
    }
}
