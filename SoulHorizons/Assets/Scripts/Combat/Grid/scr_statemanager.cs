using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class scr_statemanager : MonoBehaviour {

    public GameObject RewardMessage;
    public GameObject DeathMessage;
    public Image rewardPanel; 
    //public Text PlayerHealth;
    public Text Shield;
    public Text EffectText;
    private int hp = 100;
    bool endCombat = false;
    bool showEffect = false;
    string EffectString;
    GameObject player;
    Entity playerEntity;
    scr_PlayerMovement playerMovement;

    public int currentEncounterIndex;

    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        EffectText.enabled = false;
        if (player != null)
        {
            playerEntity = player.GetComponent<Entity>();
            playerMovement = player.GetComponent<scr_PlayerMovement>();

            try
            {
                hp = SaveManager.currentGame.GetPlayerHealth();
            }
            catch (NullReferenceException e)
            {
                Debug.Log("This is a " + e);
            }


            if (hp < 0)
            {
                hp = 0;
            }

            playerEntity._health.hp = hp;

            InputManager.cannotInputAnything = false;
            InputManager.cannotMove = false;
        }

        else
        {
            Debug.Log("PLAYER NOT FOUND");
        }
    }
	
	void Update ()
    {
        UpdateHealth();
        UpdateEffects();
        //END OF ENCOUNTER - NO MORE ENEMIES
		if(!endCombat && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {   
            OnVictory();
        }
        if (endCombat)
        {
            //INSERT CODE TO STOP ENEMY AND PLAYER MOVEMENT HERE
            if (Input.GetButton("Menu_Select") || Input.GetButton("Menu_Back"))
            {
                Debug.Log("Switching Scenes");
                SaveManager.Save();
                SceneManager.LoadScene(SceneNames.REGION);
            }
        }
	}

    public void UpdateHealth()
    {
        if (playerEntity._health.shield > 0)
        {
            Shield.enabled = true;
            Shield.color = Color.yellow;
            player.transform.Find("Shield").gameObject.SetActive(true);
        }
        else
        {
            Shield.enabled = false;
            player.transform.Find("Shield").gameObject.SetActive(false);
        }
        Shield.text = "(+" + playerEntity._health.shield + ")";

        if(playerEntity._health.hp <= 0)
        {
            InputManager.cannotInputAnything = true;
            InputManager.cannotMove = true;
            DeathMessage.SetActive(true);
            endCombat = true;
        }
    }

    public void UpdateEffects()
    {
        if (showEffect)
        {
            EffectText.text = EffectString;
            EffectText.enabled = true;
        }
        else EffectText.enabled = false;
    }

    public void ChangeEffects(string text, float duration)
    {
        Debug.Log("NEW EFFECT");
        showEffect = true;
        EffectString = text;
        StartCoroutine(EffectTime(duration));
    }

    private IEnumerator EffectTime(float time)
    {
        yield return new WaitForSeconds(time);
        showEffect = false;
    }

    private void OnVictory()
    {
        CardState newCard = CardPool.GetRandomCard();
        SaveManager.currentGame.inventory.AddCardToInventory(newCard);

        RewardMessage.SetActive(true);

        endCombat = true;

        try
        {
            SaveManager.currentGame.SetPlayerHealth(playerEntity._health.hp);
        }
        catch (NullReferenceException e)
        {
            Debug.Log("This is a " + e);
        }

        SaveManager.currentGame.SetCurrentEncounterCompleteToTrue();
    }
}
