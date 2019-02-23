using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 


public class scr_HealthBar : MonoBehaviour {

    float _health;
    float _maxHp;
    float _shield;

    public GameObject pivot;
    public GameObject bluePivot;
    public TextMeshProUGUI healthDisplay;
    public TextMeshProUGUI shieldDisplay;

    private float shieldThreshold = 100f;
    private float lerpRate = 0.01f; 
    

	void Start () {
        _health = GetComponentInParent<Entity>()._health.hp;
        _shield = GetComponentInParent<Entity>()._health.shield; 
	}
	
	void Update () {
        _health = GetComponentInParent<Entity>()._health.hp;
        _maxHp = GetComponentInParent<Entity>()._health.max_hp;
        _shield = GetComponentInParent<Entity>()._health.shield;
        pivot.transform.localScale = new Vector3(_health/_maxHp, 1,1);

        if(bluePivot != null)
        {
            healthDisplay.text = _health.ToString();
            shieldDisplay.text = _shield.ToString();

            //TO MAKE SURE THE NUMBER IS NOT SHOWN WHEN THE PLAYER HAS NO SHIELDS 
            if(_shield == 0)
            {
                shieldDisplay.enabled = false; 
            }
            else
            {
                shieldDisplay.enabled = true; 
            }

            //TO CAP THE SHIELD AT 100 IF THE NUMBER EXCEEDS 100 
            if(_shield >= shieldThreshold)
            {
                bluePivot.transform.localScale = new Vector3(shieldThreshold / _maxHp, 1, 1);
            }
            else
            {
                bluePivot.transform.localScale = new Vector3(_shield / _maxHp, 1, 1);
            }
        }

        
    }



    /*OKAY WHAT I THINK NEEDS TO HAPPEN IS: 
     * 
     * NEED TO COME UP WITH A VARIABLE THAT IS A COMBO OF HEALTH + SHIELD
     * LERP FROM THIS VALUE TO CURRENT VALUE
     * IF THIS VALUE > HEALTH , LERP SHIELD, THEN LERP HEALTH
     */


    private float LerpHealth(float _startingHP, float _endingHP)
    {
        return 0; 

    }
    private float LerpShield(float _start, float _end)
    {
        return 0; 
    }
}
