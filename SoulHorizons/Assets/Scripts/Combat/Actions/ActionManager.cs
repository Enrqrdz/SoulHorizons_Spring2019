//Colin
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Deck))]
[RequireComponent(typeof(AudioSource))]

public class ActionManager : MonoBehaviour {

	public ActionUI[] abilityUI; //references to the ability UI prefabs
    public GameObject handPanel; //a reference to the parent hand object
    public Animator anim;

    [SerializeField]
	private Deck currentDeck;
    [SerializeField]
    private Mantras currentMantras;

    [SerializeField]
    scr_SoulManager soulManager;

    private bool readyToCast = true;

    AudioSource CardChange_SFX;
    public AudioClip cardChange_SFX;
    private bool deckDisabled = false; //primary use is with transforms. The deck system is not usable when this is true

    void Awake()
    {
        currentDeck = GetComponent<Deck>();
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
        if (deckDisabled == false)
        {
            GetUserInput();
            UpdateGUI();
        }
    }

    void GetUserInput()
    {
        if (InputManager.ActionNumber() != -1)
        {
            PlayOrSwap(InputManager.ActionNumber());
        }
    }

    private void PlayOrSwap(int index)
    {
        if (InputManager.IsCardSwapPressed() && InputManager.IsDashPressed() && readyToCast)
        {
            StartCoroutine(CastCooldown(currentDeck.backupHand[index].cooldown));
            soulManager.ChargeSoulTransform(currentDeck.backupHand[index].element, currentDeck.backupHand[index].transformChargeAmount);
            currentDeck.ActivateBackup(index);
        }
        else if (InputManager.IsCardSwapPressed())
        {
            currentDeck.Swap(index);
        }
        else if(readyToCast)
        {
            for (int i = 0; i < abilityUI.Length; i++)
            {
                abilityUI[i].StartCooldown(currentDeck.hand[index].cooldown);
            }
            StartCoroutine(CastCooldown(currentDeck.hand[index].cooldown));
            soulManager.ChargeSoulTransform(currentDeck.hand[index].element, currentDeck.hand[index].transformChargeAmount);
            anim.SetBool("Cast", true);
            currentDeck.Activate(index);
        }
    }

    void UpdateGUI()
    {
        SetCardGraphics();
    }

    void SetCardGraphics()
    {
        for (int i = 0; i < abilityUI.Length; i++)
        {
            if (currentDeck.hand[i] != null) //the slot in the hand may not have been refilled if the cooldown is not finished
            {
                abilityUI[i].SetName(currentDeck.hand[i].spellName); //set the name
                abilityUI[i].SetArt(currentDeck.hand[i].art); //set the card art
                abilityUI[i].SetElement(currentDeck.hand[i].element); //set the card element
            }

            //update the backup hand slot
            if (currentDeck.backupHand[i] != null)
            {
                abilityUI[i].SetBackupName(currentDeck.backupHand[i].spellName);
            }
            else
            {
                abilityUI[i].SetBackupName(null);
            }
        }
    }

	private IEnumerator CastCooldown(float cooldown)
	{
		readyToCast = false;
		yield return new WaitForSeconds(cooldown);
        readyToCast = true;
	}

    public void DisableBasicActions(bool isDisabled)
    {
        if (isDisabled)
        {
            deckDisabled = true;
            handPanel.SetActive(false);
        }
        else
        {
            deckDisabled = false;
            handPanel.SetActive(true);
        }
    }
}
