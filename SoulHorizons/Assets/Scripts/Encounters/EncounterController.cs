using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class EncounterController : MonoBehaviour
{
    public static EncounterController globalEncounterController;

    [SerializeField]
    private int numberOfEncounters;

    RegionState currentRegion;

    [SerializeField]
    private GameObject buttonPrefab;
    public Button[] buttons;

    public Encounter[] tier1Encounters = new Encounter[1];
    public Encounter[] tier2Encounters = new Encounter[1];
    public Encounter[] tier3Encounters = new Encounter[1];
    public int currentEncounterIndex;

    void Start()
    {
        if (globalEncounterController != null && globalEncounterController != this)
        {
            Destroy(gameObject);
        }
        else
        {
            globalEncounterController = this;
            DontDestroyOnLoad(this.gameObject);
        }

        SceneManager.sceneLoaded += OnSceneChange;
    }

    void Update()
    {
    }

    void OnSceneChange(Scene scene, LoadSceneMode mode)
    {
        currentRegion = SaveManager.currentGame.GetRegion();

        if (scene.name == "LocalMap")
        {
            GenerateButtons();
        }
    }

    public void OnNewGame()
    {
        BuildMap();
        GenerateButtons();
        SaveManager.Save();
    }

    public void SetEncounterComplete(int _index, bool _completionState)
    {
        currentRegion.encounters[_index].completed = _completionState;
        SaveManager.Save();
    }
    public void SetCurrentEncounterComplete()
    {
        currentRegion.encounters[currentEncounterIndex].completed = true;
        SaveManager.Save();
    }

    public void GoToEncounter(Encounter encounterName, int index)
    {
        currentEncounterIndex = index;
        //Here is where we will put all of our info about the encounter
        //SceneManager.LoadScene or whatever (encounterName.Scene); 
        string nameOfEncounter = encounterName.name;
        scr_SceneManager.globalSceneManager.currentEncounter = encounterName;
        scr_SceneManager.globalSceneManager.currentEncounterNumber = index;
        scr_SceneManager.globalSceneManager.ChangeScene(encounterName.sceneName);
    }

    public void BuildMap()
    {
        currentRegion = SaveManager.currentGame.GetRegion();

        List<Encounter> selectedEncounters = new List<Encounter>();
        for (int i = 0; i < numberOfEncounters; i++)
        {
            currentRegion.encounters.Add(new EncounterState());

            if (i < 1)
            {
                currentRegion.encounters[i].SetTier(1);
            }
            else if (i >= 1 && i <= 7)
            {
                currentRegion.encounters[i].SetTier(2);
            }
            else if (i > 7)
            {
                currentRegion.encounters[i].SetTier(3);
            }
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            currentRegion.encounters[i].completed = false; 
            int num = 0;
            if (currentRegion.encounters[i].tier == 1)
            {
                num = UnityEngine.Random.Range(0, tier1Encounters.Length);
                currentRegion.encounters[i].encounterNumber = num;
            }
            else if (currentRegion.encounters[i].tier == 2)
            {
                num = UnityEngine.Random.Range(0, tier2Encounters.Length);
                currentRegion.encounters[i].encounterNumber = num;
            }
            else if (currentRegion.encounters[i].tier == 3)
            {
                num = UnityEngine.Random.Range(0, tier3Encounters.Length);
                currentRegion.encounters[i].encounterNumber = num;
            }
        }
    }

    public void GenerateButtons()
    {
        buttons = new Button[numberOfEncounters];
        if(scr_SceneManager.globalSceneManager.ReturnSceneName() == "LocalMap")
        {
            GameObject encounterCanvas = GameObject.FindWithTag("EncounterCanvas");

            for (int i = 0; i < numberOfEncounters; i++)
            {
                if (encounterCanvas.gameObject != null)
                {
                    GameObject newButton = Instantiate(buttonPrefab);
                    newButton.GetComponent<scr_EncounterButtons>().GatherInfo(currentRegion.encounters[i].encounterNumber, currentRegion.encounters[i].tier, currentRegion.encounters[i].completed);
                    
                    newButton.transform.SetParent(encounterCanvas.GetComponent<RectTransform>());
                    Encounter newEncounter = new Encounter();
                    if (currentRegion.encounters[i].tier == 1)
                    {
                        newEncounter = tier1Encounters[currentRegion.encounters[i].encounterNumber];
                    }
                    else if (currentRegion.encounters[i].tier == 2)
                    {
                        newEncounter = tier2Encounters[currentRegion.encounters[i].encounterNumber];
                    }
                    else if (currentRegion.encounters[i].tier == 3)
                    {
                        newEncounter = tier3Encounters[currentRegion.encounters[i].encounterNumber];
                    }
                    //DO IT HERE COLOR/COMPLETIONOVERLAY/ETC 
                    int temp = i;
                    newButton.GetComponent<scr_EncounterButtons>().GatherEnemyInfo(newEncounter.mouse,newEncounter.mush,newEncounter.archer);
                    newButton.GetComponent<Button>().onClick.AddListener(delegate { GoToEncounter(newEncounter, temp); });
                    buttons[i] = newButton.GetComponent<Button>();

                }
                else { return; }
            }
            GameObject _eventSystem = GameObject.Find("/EventSystem");
            _eventSystem.GetComponent<EventSystem>().firstSelectedGameObject = buttons[0].gameObject;
        }
    }
}