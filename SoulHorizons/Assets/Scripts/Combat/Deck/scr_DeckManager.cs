//Colin
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Contains references to all parts of the deck system. Anything outside the deck system should use the public functions in this class for deck information.
/// </summary>
[RequireComponent(typeof(scr_Deck))]
[RequireComponent(typeof(AudioSource))]

public class scr_DeckManager : MonoBehaviour {

	public scr_CardUI[] cardUI; //references to the cards UI
    public GameObject handPanel; //a reference to the parent hand object
    public Animator anim;
	scr_Deck deck_scr;
    [SerializeField] scr_SoulManager soulManager;
    int currentCard = 0;
    public float doublePressWindow = 0.3f; //the window of time the user has to press the same card again and have it register as a double press

    private bool readyToCast = true;

    AudioSource CardChange_SFX;
    public AudioClip cardChange_SFX;

    private bool disabled = false; //primary use is with transforms. The deck system is not usable when this is true

    void Awake()
    {
<<<<<<< HEAD
=======
        //get references
        //Debug.Log("CHECKING DECKS: " + scr_Inventory.deckList.Count);
        Debug.Log("DECK INDEX: " + InventoryManager.currentDeckIndex);
>>>>>>> Engineering
        deck_scr = GetComponent<scr_Deck>();
    }

	void Start ()
    {
        AudioSource SFX_Source = GetComponent<AudioSource>();
        anim = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>();
        CardChange_SFX = SFX_Source;
        UpdateGUI();
	}

    void Update()
    {
        if (!disabled)
        {
            UserInput();
            UpdateGUI();
        }
    }

    bool axisPressed = false;
    /// <summary>
    /// Gets user input.
    /// </summary>
    /// <returns>true if any input was detected, false otherwise.</returns>
    bool UserInput()
    {
        if (scr_InputManager.PlayCard() != -1)
        {
            CardInput();
            return true;
        }

        return false;
    }

    int lastCardPressed = -1; //the last value returned from play card
    float timeSincePressed = 0; //the time since the last card was pressed
    /// <summary>
    /// Handles input for the cards. (TODO? - Determines if a card has been pressed once or double pressed.)
    /// </summary>
    void CardInput()
    {
        bool doublePress = timeSincePressed < doublePressWindow;
        int input = scr_InputManager.PlayCard();

        if (input != -1)
        {
            //play or swap the current card
            PlayOrSwap(input);
        }
    }

    /// <summary>
    /// This will either play a card or swap a card depending on the swap card input.
    /// </summary>
    /// <param name="index"></param>
    private void PlayOrSwap(int index)
    {
        if (scr_InputManager.IsCardSwapPressed() && scr_InputManager.IsDashPressed() && readyToCast) //both triggers are down and the player can cast
        {
            //play the backup slot
            //start cooldown to be able to cast another card
            StartCoroutine(CastCooldown(deck_scr.backupHand[index].cooldown));
            //charge the soul transform
            soulManager.ChargeSoulTransform(deck_scr.backupHand[index].element, deck_scr.backupHand[index].chargeAmount);
            deck_scr.ActivateBackup(index);

        }
        else if (scr_InputManager.IsCardSwapPressed())
        {
            deck_scr.Swap(index);
        }
        else if(readyToCast) //if swap is not pressed and the player is ready to cast
        {
            //start the cooldown animation on the cardUI
            for (int i = 0; i < cardUI.Length; i++)
            {
                cardUI[i].StartCooldown(deck_scr.hand[index].cooldown);
            }

            //start cooldown to be able to cast another card
            StartCoroutine(CastCooldown(deck_scr.hand[index].cooldown));
            //charge the soul transform
            soulManager.ChargeSoulTransform(deck_scr.hand[index].element, deck_scr.hand[index].chargeAmount);
            anim.SetBool("Cast", true);
            deck_scr.Activate(index);
        }
    }

    /// <summary>
    /// Update the deck system GUI.
    /// </summary>
    void UpdateGUI()
    {
        SetCardGraphics();
    }

    /// <summary>
    /// Gets all graphical info from the card object and sets the UI accordingly
    /// </summary>
    void SetCardGraphics()
    {
        for (int i = 0; i < cardUI.Length; i++)
        {
            if (deck_scr.hand[i] != null) //the slot in the hand may not have been refilled if the cooldown is not finished
            {
                cardUI[i].SetName(deck_scr.hand[i].cardName); //set the name
                cardUI[i].SetArt(deck_scr.hand[i].art); //set the card art
                cardUI[i].SetElement(deck_scr.hand[i].element); //set the card element
            }

            //update the backup hand slot
            if (deck_scr.backupHand[i] != null)
            {
                cardUI[i].SetBackupName(deck_scr.backupHand[i].cardName);
            }
            else
            {
                cardUI[i].SetBackupName(null);
            }
        }
    }

    /// <summary>
	/// Called when casting a card to give a cooldown until another card can be cast
	/// </summary>
	/// <returns></returns>
	private IEnumerator CastCooldown(float cooldown)
	{
        Debug.Log("Hello, I am on cooldown");
		readyToCast = false;
		yield return new WaitForSeconds(cooldown);
        Debug.Log("Hello, I am off of cooldown");
        readyToCast = true;
	}

    /// <summary>
    /// The soul manager calls this to disable spells while a transform is active.
    /// </summary>
    public void Disable(bool disable)
    {
        if (disable)
        {
            //deactivate everything
            disabled = true;
            handPanel.SetActive(false);
        }
        else
        {
            //activate everything
            disabled = false;
            handPanel.SetActive(true);
        }
    }
}
