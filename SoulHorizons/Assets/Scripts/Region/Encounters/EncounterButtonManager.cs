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

    void Start()
    {
        eventSystem = GameObject.Find("/EventSystem");
        mouseText.text = "x " + encounterState.GetEncounterData().GetNumberOfMouses();
        mushText.text = "x " + encounterState.GetEncounterData().GetNumberOfMush();
        archerText.text = "x " + encounterState.GetEncounterData().GetNumberOfArchers();

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
    }

    private void UpdateChildren()
    {
        infoPanel.SetActive(false);

        if (encounterState.isCompleted)
        {
            sprite.GetComponent<SpriteRenderer>().color = Color.red; 
            fogMask.SetActive(true);
            gameObject.GetComponent<Button>().interactable = true;
        }
        else if (encounterState.isAccessible)
        {
            sprite.GetComponent<SpriteRenderer>().color = Color.white;
            fogMask.SetActive(true);
            gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            sprite.GetComponent<SpriteRenderer>().color = Color.gray; 
            fogMask.SetActive(false);
            gameObject.GetComponent<Button>().interactable = false;
        }
    }
}
