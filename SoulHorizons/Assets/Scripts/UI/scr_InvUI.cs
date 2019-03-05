﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_InvUI : MonoBehaviour {

    public ActionUI[] cardUI;
    public List<GameObject> banners;
    public GameObject invPanel;
    public GameObject cardBanner;
    public GameObject BannerSpawn;
    public Canvas c;
    public Font UIFont;
    public int minDeckSize = 10;
    public Text deckNum;
    public float deckTextX = 600;
    public float deckTextY = 400;
    
    [SerializeField]
    private GameObject regionButton;

    void Start () {
        SetDeckText();
        UpdateBanners();
        GUIStyle style = new GUIStyle();
        style.richText = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (invPanel.activeSelf)
        {
            SetCardGraphics();
            UpdateBanners();
        }

        CheckForRegionInput();
    }

    public void DisplayUI()
    {
        if (!invPanel.activeSelf)
        {
            invPanel.SetActive(true);
        }
        else
        {
            if (SaveManager.currentGame.inventory.GetDeckLength() >= minDeckSize)
            {
                invPanel.SetActive(false);
                UpdateBanners();
                SaveManager.Save();
            }
        }
    }

    void SetCardGraphics()
    {
        List<CardState> cardInventory = SaveManager.currentGame.inventory.GetCardInventory();

        for (int i = 0; i < cardUI.Length; i++)
        {
            if (i < cardInventory.Count)
            {
                ActionData cardData = cardInventory[i].GetActionData();
                cardUI[i].SetCardState(new CardState(cardData, 1));
                cardUI[i].SetName(cardData.actionName);
                cardUI[i].SetArt(cardData.art);
                cardUI[i].SetElement(cardData.element);

                int numberInDeck = 0;
                List<CardState> deck = SaveManager.currentGame.inventory.GetDeck();
               
                for(int j = 0; j < deck.Count; j++)
                {
                    if(deck[j].IsTheSameCard(cardInventory[i]))
                    {
                        numberInDeck = deck[j].numberOfCopies;
                    }
                }

                cardUI[i].SetBackupName(numberInDeck + "/" + cardInventory[i].numberOfCopies);
            }
        }
    }

    void SetDeckText()
    {
        float tempX = BannerSpawn.transform.position.x;
        float tempY = BannerSpawn.transform.position.y;
        List<CardState> deck = SaveManager.currentGame.inventory.GetDeck();

        foreach (CardState cardState in deck)
        {
            GameObject banner = Instantiate(cardBanner, new Vector3(tempX, tempY, 0), Quaternion.identity);
            string tempTxt = "CardOverlay/" + cardState.GetActionData().actionName;
            banner.transform.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>(tempTxt);
            banner.transform.GetChild(3).GetComponent<Text>().text = cardState.GetActionData().actionName + ": " + cardState.numberOfCopies + "\n";
            banner.transform.SetParent(c.transform);         
            banners.Add(banner);
            tempY -= 75;
        }
        
    }

    void UpdateBanners()
    {
        float tempX = BannerSpawn.transform.position.x;
        float tempY = BannerSpawn.transform.position.y;
        int tempCount = 0;
        List<CardState> deck = SaveManager.currentGame.inventory.GetDeck();

        foreach (CardState cardState in deck)
        {
            string tempTxt = "CardOverlay/" + cardState.GetActionData().actionName;
            banners[tempCount].transform.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>(tempTxt);
            banners[tempCount].transform.GetChild(3).GetComponent<Text>().text = cardState.GetActionData().actionName + ": " + cardState.numberOfCopies + "\n";
            tempY -= 75;
            if (invPanel.activeSelf)
            {
                banners[tempCount].SetActive(true);
            }
            else
            {
                banners[tempCount].SetActive(false);
            }
            tempCount++;
        }

        if (SaveManager.currentGame.inventory.GetDeckLength() < minDeckSize)
        {
            scr_SceneManager.canSwitch = false;
            deckNum.text = "<color=red>" + SaveManager.currentGame.inventory.GetDeckLength() + "</color> / " + minDeckSize;
        }
        else
        {
            scr_SceneManager.canSwitch = true;
            deckNum.text = SaveManager.currentGame.inventory.GetDeckLength() + " / " + minDeckSize;
        }
    }

    public void CheckForRegionInput()
    {
        if(Input.GetButtonDown("PlayCard2_Button") || Input.GetKeyDown("i"))
        {
            regionButton.GetComponent<Button>().onClick.Invoke();
        }
    }
}
