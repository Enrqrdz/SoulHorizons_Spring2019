using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance { get; private set; }
	public ActionIconUI[] abilityUI;
    public ActionIconUI[] mantraUI;
    public ActionIconUI[] upcomingCardsUI;
    public GameObject handPanel;
    public Animator playerAnimator;
    public Entity player;
    public float globalCooldown;

    public float xCooldown;
    public float yCooldown;
    public float aCooldown;
    public float bCooldown;

    public Sprite shuffleArt;


    [SerializeField]
    private AudioSource CardChange_SFX;
    [SerializeField]
	private Deck currentDeck;
    [SerializeField]
    private Mantras currentMantras;
    [SerializeField]
    private scr_SoulManager soulManager;

    private int decisionNumber;
    private ActionData projectedAttack;
    private bool projectingTiles = false;
    private bool readyToCastAction = true;
    private bool readyToCastAbility = true;

    private bool readyToCastXMantra = true;
    private bool readyToCastYMantra = true;
    private bool readyToCastAMantra = true;
    private bool readyToCastBMantra = true;

    private bool deckIsEnabled = true;

    void Awake()
    {
        currentDeck = GetComponent<Deck>();
        currentMantras = GetComponent<Mantras>();
    }

	void Start ()
    {
        AudioSource SFX_Source = GetComponent<AudioSource>();
        playerAnimator = ObjectReference.Instance.Player.GetComponentInChildren<Animator>();
        player = ObjectReference.Instance.PlayerEntity;
        CardChange_SFX = SFX_Source;
        SetMantraGraphics();

        xCooldown = currentMantras.activeMantras[0].cooldown;
        yCooldown = currentMantras.activeMantras[1].cooldown;
        aCooldown = currentMantras.activeMantras[2].cooldown;
        bCooldown = currentMantras.activeMantras[3].cooldown;
    }

    void Update()
    {
        if (deckIsEnabled == true)
        {
            if (player.isStunned == false)
            {
                ProjectAttack();
                CheckForMovement();
                GetUserInput();
            }
        }
    }

    private void CheckForMovement()
    {
        if (projectingTiles)
        {
            if(InputManager.MainHorizontal() != 0 || InputManager.MainVertical() != 0)
            {
                if(projectedAttack != null)
                {
                    projectedAttack.DeProject();
                    projectedAttack.Project();
                }
            }
        }
    }

    private void CleanUpProjections()
    {
        for(int i = 0; i < scr_Grid.GridController.columnSizeMax; i++)
        {
            for (int j = 0; j < scr_Grid.GridController.rowSizeMax; j++)
            {
                scr_Grid.GridController.grid[i, j].DeHighlight();
            }
        }
    }

    private void ProjectAttack()
    {
        //decisionNumber will be either: -1,0,1,2,3
        decisionNumber = InputManager.ActionNumberPrimer();

        if (decisionNumber != -1 && readyToCastAction)
        {
            projectingTiles = true;
            projectedAttack = currentMantras.activeMantras[decisionNumber];
            if (projectedAttack != null)
            {
                projectedAttack.Project();
            }
        }
    }

    private void GetUserInput()
    {
        //decisionNumber will be either: -1,0,1,2,3
        decisionNumber = InputManager.ActionNumber();

        if (decisionNumber != -1 && readyToCastAction)
        {
            projectingTiles = false;
            if (currentMantras.activeMantras[decisionNumber] != null)
            {
                currentMantras.activeMantras[decisionNumber].DeProject();
            }
            
            if(decisionNumber == 0 && readyToCastXMantra)
            {
                ActivateMantra(decisionNumber);
            }
            if (decisionNumber == 1 && readyToCastYMantra)
            {
                ActivateMantra(decisionNumber);
            }
            if (decisionNumber == 2 && readyToCastAMantra)
            {
                ActivateMantra(decisionNumber);
            }
            if (decisionNumber == 3 && readyToCastBMantra)
            {
                ActivateMantra(decisionNumber);
            }
        }
    }

    private void ActivateMantra(int decisionNumber)
    {
        currentMantras.Activate(decisionNumber);
        StartCoroutine(ActionCooldown());
        playerAnimator.SetBool("Cast", true);

        for (int i = 0; i < mantraUI.Length; i++)
        {
            mantraUI[i].StartCooldown(globalCooldown);
        }

        switch (decisionNumber)
        {
            case 0:
                StartCoroutine(XMantraCooldown());
                mantraUI[0].StartCooldown(currentMantras.activeMantras[0].cooldown);
                break;
            case 1:
                StartCoroutine(YMantraCooldown());
                mantraUI[1].StartCooldown(currentMantras.activeMantras[1].cooldown);
                break;
            case 2:
                StartCoroutine(AMantraCooldown());
                mantraUI[2].StartCooldown(currentMantras.activeMantras[2].cooldown);
                break;
            case 3:
                StartCoroutine(BMantraCooldown());
                mantraUI[3].StartCooldown(currentMantras.activeMantras[3].cooldown);
                break;
        }
    }

    private void SetMantraGraphics()
    {
        for (int i = 0; i < mantraUI.Length; i++)
        {
            if(currentMantras.activeMantras[i] != null)
            {
                mantraUI[i].SetArt(currentMantras.activeMantras[i].art);
            }
            else
            {
                Debug.Log("No Active Mantra UI set!");
            }
        }
    }

    private IEnumerator ActionCooldown()
    {
        if (globalCooldown != 0)
        {
            readyToCastAction = false;
            CleanUpProjections();
            yield return new WaitForSeconds(globalCooldown);
            readyToCastAction = true;
        }
    }

    private IEnumerator AbilityCooldown(float cooldown)
	{
        if(cooldown != 0)
        {
            readyToCastAbility = false;
            yield return new WaitForSeconds(cooldown);
            readyToCastAbility = true;
        }
	}

    private IEnumerator XMantraCooldown()
    {
        if (xCooldown != 0)
        {
            readyToCastXMantra = false;
            yield return new WaitForSeconds(xCooldown);
            readyToCastXMantra = true;
        }
    }

    private IEnumerator YMantraCooldown()
    {
        if (yCooldown != 0)
        {
            readyToCastYMantra = false;
            yield return new WaitForSeconds(yCooldown);
            readyToCastYMantra = true;
        }
    }

    private IEnumerator AMantraCooldown()
    {
        if (aCooldown != 0)
        {
            readyToCastAMantra = false;
            yield return new WaitForSeconds(aCooldown);
            readyToCastAMantra = true;
        }
    }

    private IEnumerator BMantraCooldown()
    {
        if (bCooldown != 0)
        {
            readyToCastBMantra = false;
            yield return new WaitForSeconds(bCooldown);
            readyToCastBMantra = true;
        }
    }

    public void DisableBasicActions(bool actionsAreDisabled)
    {
        if (actionsAreDisabled)
        {
            deckIsEnabled = false;
            handPanel.SetActive(false);
        }
        else
        {
            deckIsEnabled = true;
            handPanel.SetActive(true);
        }
    }
}
