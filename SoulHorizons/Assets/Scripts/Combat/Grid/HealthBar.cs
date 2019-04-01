using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 


public class HealthBar : MonoBehaviour {

    float health, maxHealth, shield;

    public GameObject pivot, bluePivot;
    public Entity targetEntity;

    private float shieldThreshold = 100f;
    private float lerpRate = 0.01f; 

	void Start () 
    {
        OnStart();
	}

    public virtual void OnStart(){}
	
	void Update () 
    {
        if(targetEntity != null)
        {
            health = targetEntity._health.hp;
            maxHealth = targetEntity._health.max_hp;
            shield = targetEntity._health.shield;
            pivot.transform.localScale = new Vector3(health/maxHealth, 1,1);

            if(bluePivot != null)
            {
                //TO CAP THE SHIELD AT 100 IF THE NUMBER EXCEEDS 100 
                if(shield >= shieldThreshold)
                {
                    bluePivot.transform.localScale = new Vector3(shieldThreshold / maxHealth, 1, 1);
                }
                else
                {
                    bluePivot.transform.localScale = new Vector3(shield / maxHealth, 1, 1);
                }
            }  
        }      
    }
}
