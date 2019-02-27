using System.Collections;
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
    // Use this for initialization

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
    }

    public void DisplayUI()
    {
        if (!invPanel.activeSelf)
        {
            invPanel.SetActive(true);
        }
        else
        {
            if (InventoryManager.getDeckSize() >= minDeckSize)
            {
                invPanel.SetActive(false);
                UpdateBanners();
                SaveManager.Save();
            }
        }
    }

    void SetCardGraphics()
    {
        for (int i = 0; i < cardUI.Length; i++)
        {
            if (i < InventoryManager.cardInv.Count)
            {
                cardUI[i].SetName(InventoryManager.cardInv[i].Key.actionName); //set the name
                cardUI[i].SetArt(InventoryManager.cardInv[i].Key.art); //set the card art
                cardUI[i].SetElement(InventoryManager.cardInv[i].Key.element); //set the card element

                //Find how many of this card are in your current deck 
                List<KeyValuePair<string, int>> myDeck = InventoryManager.deckList[InventoryManager.currentDeckIndex];
                int index = -1;
                for(int j = 0; j < myDeck.Count; j++)
                {
                    if(myDeck[j].Key == InventoryManager.cardInv[i].Key.actionName)
                    {
                        index = j;
                    }
                }
                if (index < 0) Debug.Log("CARD NOT FOUND");
                cardUI[i].SetBackupName(InventoryManager.deckList[InventoryManager.currentDeckIndex][index].Value.ToString() + "/" + InventoryManager.cardInv[i].Value.ToString()); //Set the card amount currently in inventory
            }
            else
            {
                //MAKE CARD NOT SHOW UP
            }

           
            
        }
    }

    void SetDeckText()
    {

        float tempX = BannerSpawn.transform.position.x;
        float tempY = BannerSpawn.transform.position.y;
        foreach (KeyValuePair<string, int> pair in InventoryManager.deckList[InventoryManager.currentDeckIndex])
        {
            GameObject banner = Instantiate(cardBanner, new Vector3(tempX, tempY, 0), Quaternion.identity);
            string tempTxt = "CardOverlay/" + pair.Key;
            banner.transform.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>(tempTxt);
            banner.transform.GetChild(3).GetComponent<Text>().text = pair.Key + ": " + pair.Value + "\n";
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
        foreach (KeyValuePair<string, int> pair in InventoryManager.deckList[InventoryManager.currentDeckIndex])
        {
            string tempTxt = "CardOverlay/" + pair.Key;
            banners[tempCount].transform.GetChild(2).GetComponent<Image>().sprite = Resources.Load<Sprite>(tempTxt);
            banners[tempCount].transform.GetChild(3).GetComponent<Text>().text = pair.Key + ": " + pair.Value + "\n";
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

        if (InventoryManager.getDeckSize() < minDeckSize)
        {
            scr_SceneManager.canSwitch = false;
            deckNum.text = "<color=red>" + InventoryManager.getDeckSize() + "</color> / " + minDeckSize;
        }
        else
        {
            scr_SceneManager.canSwitch = true;
            deckNum.text = InventoryManager.getDeckSize() + " / " + minDeckSize;
        }
    }

}
