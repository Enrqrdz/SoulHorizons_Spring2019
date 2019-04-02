using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 


public class HealthBar : MonoBehaviour 
{
    [Header("Must Be Set")]
    public GameObject greenPivot;
    public GameObject bluePivot;
    public Entity targetEntity;

    [Header("Options")]
    public Color flashColor = Color.red;
    public float flashTime = .1f;

    float health, maxHealth, shield;

    private float shieldThreshold = 100f;
    private float lerpRate = 0.01f; 

	void Start () 
    {
        OnStart();
	}

    public virtual void OnStart(){}
	
	void Update () 
    {
        bool healthChanged = false;
         
        if(targetEntity != null)
        {
            healthChanged = (health != targetEntity._health.hp);

            health = targetEntity._health.hp;
            maxHealth = targetEntity._health.max_hp;
            shield = targetEntity._health.shield;
            greenPivot.transform.localScale = new Vector3(health/maxHealth, 1,1);

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

        if(healthChanged)
        {
            StartCoroutine("Flash");
        }
    }

    public IEnumerator Flash()
    {
        SpriteRenderer greenPivotSR = greenPivot.GetComponentInChildren<SpriteRenderer>();
        SpriteRenderer bluePivotSR = bluePivot.GetComponentInChildren<SpriteRenderer>();

        Color greenPivotColor = greenPivotSR.color;
        Color bluePivotColor = bluePivotSR.color;

        greenPivotSR.color = flashColor;
        bluePivotSR.color = flashColor;

        yield return new WaitForSeconds(flashTime);

        greenPivotSR.color = greenPivotColor;
        bluePivotSR.color = bluePivotColor;
    }
}
