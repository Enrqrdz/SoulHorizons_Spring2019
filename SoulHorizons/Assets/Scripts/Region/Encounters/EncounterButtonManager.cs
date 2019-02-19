using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class EncounterButtonManager : MonoBehaviour
{
    EncounterState encounterState;
    EncounterData encounter;

    public GameObject infoPanel;

    public TextMeshPro mouseText;
    public TextMeshPro mushText;
    public TextMeshPro archerText; 

    private GameObject eventSystem; 

    public float deltaT = .05f;
    private float t;

    void Start()
    {
        infoPanel.SetActive(false);
        eventSystem = GameObject.Find("/EventSystem");
        mouseText.text = "x " + encounter.GetNumberOfMouses();
        mushText.text = "x " + encounter.GetNumberOfMush();
        archerText.text = "x " + encounter.GetNumberOfArchers();

        if (encounterState.isCompleted)
        {
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red; 
        }
        else
        {
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    void Update()
    {        
        if(eventSystem.GetComponent<EventSystem>().currentSelectedGameObject == this.gameObject)
        {
            infoPanel.SetActive(true);
            Vector3 nodePosition = gameObject.transform.GetChild(0).transform.position;
            Vector3 newPosition = new Vector3(nodePosition.x, nodePosition.y, Camera.main.transform.position.z);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newPosition, t);
            t += deltaT;
        }
        else
        {
            infoPanel.SetActive(false);
            t = 0;
        }
    }

    public void SetEncounterStateAndData(EncounterState newState, EncounterData newEncounter)
    {
        encounterState = newState;
        encounter = newEncounter;
    }
}
