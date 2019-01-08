using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class scr_EncounterButtons : MonoBehaviour
{


    public int encounterNumber;
    public int tier;
    public bool complete = false;
    public bool locked;
    public GameObject nextEncounter;
    public Image infoPanel;


    private int mouseNum;
    private int mushNum;
    private int archerNum; 

    public Text mouseText;
    public Text mushText;
    public Text archerText; 

    private GameObject eventSystem; 


    void Start()
    {
        infoPanel.enabled = false;
        Active(false); 
        eventSystem = GameObject.Find("/EventSystem");
        mouseText.text = "x " + mouseNum;
        mushText.text = "x " + mushNum;
        archerText.text = "x " + archerNum;
    }


    void Update()
    {

        if (complete)
        {
            GetComponent<Image>().color = Color.red;
            //GetComponent<Button>().interactable = false; 
        }
        else
        {
            GetComponent<Image>().color = Color.white;
        }


        
        if(eventSystem.GetComponent<EventSystem>().currentSelectedGameObject == this.gameObject)
        {
            infoPanel.enabled = true;
            Active(true); 

        }
        else
        {
            infoPanel.enabled = false;
            Active(false); 
        }
        


    }

    public void SetComplete(bool completed)
    {

    }

    public void GatherInfo(int _encounterNumber, int _tier, bool _complete)
    {
        encounterNumber = _encounterNumber;
        tier = _tier;
        complete = _complete;
    }

    public void GatherEnemyInfo(int mouse, int mush, int archer)
    {
        mouseNum = mouse;
        mushNum = mush;
        archerNum = archer; 
    }

    public void Active(bool _active)
    {
        infoPanel.transform.GetChild(0).gameObject.SetActive(_active);
        infoPanel.transform.GetChild(1).gameObject.SetActive(_active);
        infoPanel.transform.GetChild(2).gameObject.SetActive(_active);
    }
}
