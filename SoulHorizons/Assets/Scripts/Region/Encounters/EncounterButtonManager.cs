using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EncounterButtonManager : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    EncounterState encounterState;

    public GameObject sprite;
    public GameObject infoPanel;
    public GameObject fogMask;

    public TextMeshPro mouseText;
    public TextMeshPro mushText;
    public TextMeshPro archerText; 

    private GameObject eventSystem; 

    [Header("Encounter Type Sprites")]
    public Sprite bossEncounter;
    public Sprite combatEncounter;
    public Sprite eventEncounter;
    public Sprite outpostEncounter;
    public Sprite restEncounter;
    public Sprite treasureEncounter;

    void Start()
    {
        eventSystem = GameObject.Find("/EventSystem");

        UpdateChildren();
    }

    public void OnSelect(BaseEventData eventData)
    {
        infoPanel.SetActive(true);

        Vector3 nodePosition = gameObject.transform.GetChild(0).transform.position;
        Vector3 newPosition = new Vector3(nodePosition.x, nodePosition.y, Camera.main.transform.position.z);
        Camera.main.GetComponent<CameraController>().SetDestination(newPosition);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        infoPanel.SetActive(false);
    }

    public void SetEncounterState(EncounterState newState)
    {
        encounterState = newState;
        UpdateChildren();
    }

    private void UpdateChildren()
    {
        infoPanel.SetActive(false);
        
        Color spriteColor;
        bool interactable;
        float fogRadius;

        if (encounterState.isCompleted)
        {
            spriteColor = Color.red; 
            interactable = true;
            fogRadius = encounterState.GetEncounterData().clearRadiusWhileCompleted;
        }
        else if (encounterState.isAccessible)
        {
            spriteColor = Color.white;
            interactable = true;
            fogRadius = encounterState.GetEncounterData().clearRadiusWhileDiscovered;
        }
        else
        {
            spriteColor = Color.gray; 
            interactable = false;
            fogRadius = encounterState.GetEncounterData().clearRadiusWhileUndiscovered;
        }

        sprite.GetComponent<SpriteRenderer>().color = spriteColor; 
        gameObject.GetComponent<Button>().interactable = interactable;
        fogMask.transform.localScale = new Vector3(fogRadius, fogRadius, 0);

        mouseText.text = "x " + encounterState.GetEncounterData().GetNumberOfMouses();
        mushText.text = "x " + encounterState.GetEncounterData().GetNumberOfMush();
        archerText.text = "x " + encounterState.GetEncounterData().GetNumberOfArchers();

        SetSprite();
    }

    private void SetSprite()
    {
        Sprite newNodeSprite = sprite.GetComponent<SpriteRenderer>().sprite;

        switch(encounterState.GetEncounterData().type)
        {
            case EncounterType.Boss:
                newNodeSprite = bossEncounter;
                break;
                
            case EncounterType.Combat:
                newNodeSprite = combatEncounter;
                break;

            case EncounterType.Event:
                newNodeSprite = eventEncounter;
                break;

            case EncounterType.Outpost:
                newNodeSprite = outpostEncounter;
                break;

            case EncounterType.Rest:
                newNodeSprite = restEncounter;
                break;

            case EncounterType.Treasure:
                newNodeSprite = treasureEncounter;
                break;

            
        }

        sprite.GetComponent<SpriteRenderer>().sprite = newNodeSprite;
    }
}
