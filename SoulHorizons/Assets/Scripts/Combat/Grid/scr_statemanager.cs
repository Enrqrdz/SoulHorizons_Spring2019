using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_statemanager : MonoBehaviour {

    public GameObject RewardMessage;
    public GameObject DeathMessage;
    private int hp = 100;
    bool endCombat = false;
    GameObject player;
    Entity playerEntity;

    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerEntity = player.GetComponent<Entity>();

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
        if(playerEntity != null)
        {
            UpdateHealth();
        }
        else
        {
            playerEntity = player.GetComponent<Entity>();
        }
        //END OF ENCOUNTER - NO MORE ENEMIES
		if(!endCombat && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {   
            OnVictory();
        }
        if (endCombat)
        {
            //INSERT CODE TO STOP ENEMY AND PLAYER MOVEMENT HERE
            InputManager.cannotMove = true;
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
            player.transform.Find("Shield").gameObject.SetActive(true);
        }
        else
        {
            player.transform.Find("Shield").gameObject.SetActive(false);
        }
        

        if(playerEntity._health.hp <= 0)
        {
            InputManager.cannotInputAnything = true;
            InputManager.cannotMove = true;
            DeathMessage.SetActive(true);
            endCombat = true;
        }
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
