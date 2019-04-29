using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour 
{
    [Header("Must Be Set")]
    [SerializeField] private List<ActionUI> cardIcons = new List<ActionUI>();
    public GameObject invPanel;
    public GameObject cardBanner;
    public GameObject BannerSpawn;
    public Canvas c;
    public Font UIFont;
    public Text deckNum;
    [SerializeField] private GameObject regionButton;
    [SerializeField] private GameObject pageNumDisplay;

    [Header("Options")]
    public int minDeckSize = 10;

    private List<GameObject> banners = new List<GameObject>();
    private int currentPageNum = 0;
    private int maxPageNum;

    void Start () 
    {
        UpdateCardIcons();
        SetupCardBanners();
        GUIStyle style = new GUIStyle();
        style.richText = true;
    }

    void Awake()
    {
        List<CardState> cardInventory = SaveManager.currentGame.inventory.GetCardInventory();
        maxPageNum = (cardInventory.Count - 1) / cardIcons.Count;
        SetPageNumDisplay();
    }

    void Update()
    {
        UpdateCardIcons();
        UpdateBanners();

        CheckForInput();
    }

    private void UpdateCardIcons()
    {
        List<CardState> cardInventory = SaveManager.currentGame.inventory.GetCardInventory();

        for (int i = 0; i < cardIcons.Count; i++)
        {
            int cardIndexInInventory = i + (currentPageNum * cardIcons.Count);

            if (cardIndexInInventory < cardInventory.Count)
            {
                cardIcons[i].SetActive();

                ActionData cardData = cardInventory[cardIndexInInventory].GetActionData();
                cardIcons[i].SetActionData(cardData);

                int numberInDeck = 0;
                List<CardState> deck = SaveManager.currentGame.inventory.GetDeck();
               
                for(int j = 0; j < deck.Count; j++)
                {
                    if(deck[j].IsTheSameCard(cardInventory[cardIndexInInventory]))
                    {
                        numberInDeck = deck[j].numberOfCopies;
                    }
                }

                cardIcons[i].SetCountText(numberInDeck + "/" + cardInventory[cardIndexInInventory].numberOfCopies);
            }
            else
            {
                cardIcons[i].SetInactive();
            }
        }
    }

    private void SetupCardBanners()
    {
        float tempX = BannerSpawn.transform.position.x;
        float tempY = BannerSpawn.transform.position.y;
        List<CardState> deck = SaveManager.currentGame.inventory.GetDeck();

        foreach (CardState cardState in deck)
        {
            CreateBanner(tempX, tempY, cardState);
            tempY -= 75;
        }  
    }

    private void CreateBanner(float x, float y, CardState cardState)
    {
        GameObject banner = Instantiate(cardBanner, new Vector3(x, y, 0), Quaternion.identity);
        string tempTxt = "CardOverlay/" + cardState.GetActionData().actionName;
        banner.transform.GetChild(2).GetComponent<Image>().sprite = cardState.GetActionData().art;
        banner.transform.GetChild(3).GetComponent<Text>().text = cardState.GetActionData().actionName + ": " + cardState.numberOfCopies + "\n";
        banner.transform.SetParent(c.transform);         
        banners.Add(banner);
    }

    void UpdateBanners()
    {
        float tempX = BannerSpawn.transform.position.x;
        float tempY = BannerSpawn.transform.position.y;
        List<CardState> deck = SaveManager.currentGame.inventory.GetDeck();

        if(deck.Count > banners.Count)
        {
            for(int i = 0; i < deck.Count - banners.Count; i++)
            {
                CreateBanner(BannerSpawn.transform.position.x, BannerSpawn.transform.position.y - (75 * (i + banners.Count)), deck[0]);
            }
        }

        for(int i = 0; i < banners.Count; i++)
        {
            if(i >= deck.Count)
            {
                Destroy(banners[i]);
                banners.RemoveAt(i);
            }

            string tempTxt = "CardOverlay/" + deck[i].GetActionData().actionName;
            banners[i].transform.GetChild(2).GetComponent<Image>().sprite = deck[i].GetActionData().art;
            banners[i].transform.GetChild(3).GetComponent<Text>().text = deck[i].GetActionData().actionName + ": " + deck[i].numberOfCopies + "\n";
            tempY -= 75;
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

    private void CheckForInput()
    {
        if(Input.GetButtonDown("PlayCard2_Button") || Input.GetKeyDown("i"))
        {
            regionButton.GetComponent<Button>().onClick.Invoke();
        }

        if(Input.GetButtonDown("InventoryPageTurnRight"))
        {
            Debug.Log(currentPageNum);
            if(currentPageNum < maxPageNum)
                currentPageNum++;

            SetPageNumDisplay();
        }
        else if(Input.GetButtonDown("InventoryPageTurnLeft"))
        {
            Debug.Log(currentPageNum);
            if(currentPageNum > 0)
                currentPageNum--;

            SetPageNumDisplay();
        }
    }

    private void SetPageNumDisplay()
    {
        pageNumDisplay.GetComponent<Text>().text = "<- LB     Page " + (currentPageNum + 1) + "/" + (maxPageNum + 1) + "     RB ->";

        GameObject eventSystem = GameObject.Find("EventSystem");
        eventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(regionButton);
    }
}
