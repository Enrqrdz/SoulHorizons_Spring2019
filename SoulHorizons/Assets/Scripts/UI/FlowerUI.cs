using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowerUI : MonoBehaviour
{
    public GameObject HealthBar;
    public Sprite flower80;
    public Sprite flower60;
    public Sprite flower40;
    public Sprite flower20;
    public Sprite flower0;

    void Update()
    {
        float currHealth = HealthBar.GetComponent<HealthBar>().targetEntity._health.hp;
        float maxHealth = HealthBar.GetComponent<HealthBar>().targetEntity._health.max_hp;
        if (currHealth<= maxHealth*0.8f && currHealth > maxHealth * 0.6f)
        {
            gameObject.GetComponent<Image>().sprite = flower80;
        } else if(currHealth <= maxHealth * 0.6f && currHealth > maxHealth * 0.4f)
        {
            gameObject.GetComponent<Image>().sprite = flower60;
        }
        else if(currHealth <= maxHealth * 0.4f && currHealth > maxHealth * 0.2f)
        {
            gameObject.GetComponent<Image>().sprite = flower40;
        }
        else if(currHealth <= maxHealth * 0.2f && currHealth > maxHealth * 0.0f)
        {
            gameObject.GetComponent<Image>().sprite = flower20;
        }
        else if(currHealth == 0f)
        {
            gameObject.GetComponent<Image>().sprite = flower0;
        }
    }
}
