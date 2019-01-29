using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EncounterButtonManager : MonoBehaviour
{
    EncounterState encounterState;
    EncounterData encounter;

    public Image infoPanel;

    public Text mouseText;
    public Text mushText;
    public Text archerText; 

    private GameObject eventSystem; 

    void Start()
    {
        infoPanel.enabled = false;
        SetIsActive(false); 
        eventSystem = GameObject.Find("/EventSystem");
        Debug.Log(encounter.mouseNum);
        mouseText.text = "x " + encounter.mouseNum;
        mushText.text = "x " + encounter.mushNum;
        archerText.text = "x " + encounter.archerNum;

        if (encounterState.isCompleted)
        {
            GetComponent<Image>().color = Color.red; 
        }
        else
        {
            GetComponent<Image>().color = Color.white;
        }
    }

    void Update()
    {        
        if(eventSystem.GetComponent<EventSystem>().currentSelectedGameObject == this.gameObject)
        {
            infoPanel.enabled = true;
            SetIsActive(true); 
        }
        else
        {
            infoPanel.enabled = false;
            SetIsActive(false); 
        }
    }

    public void SetStateAndEncounter(EncounterState newState, EncounterData newEncounter)
    {
        encounterState = newState;
        encounter = newEncounter;
    }

    public void SetIsActive(bool isActive)
    {
        infoPanel.transform.GetChild(0).gameObject.SetActive(isActive);
        infoPanel.transform.GetChild(1).gameObject.SetActive(isActive);
        infoPanel.transform.GetChild(2).gameObject.SetActive(isActive);
    }
}
