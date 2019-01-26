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

    RegionState currentRegion;

    [SerializeField]
    private GameObject buttonPrefab;
    private Button[] buttons;

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

    void OnSceneChange(Scene scene, LoadSceneMode mode)
    {
        currentRegion = SaveManager.currentGame.GetRegion();

        if (scene.name == "LocalMap")
        {
            GenerateButtons();
        }
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

    public void GenerateButtons()
    {
        buttons = new Button[currentRegion.encounters.Count];
        if(scr_SceneManager.globalSceneManager.ReturnSceneName() == "LocalMap")
        {
            GameObject encounterCanvas = GameObject.FindWithTag("EncounterCanvas");

            for (int i = 0; i < currentRegion.encounters.Count; i++)
            {
                if (encounterCanvas.gameObject != null)
                {
                    GameObject newButton = Instantiate(buttonPrefab);
                    newButton.GetComponent<scr_EncounterButtons>().GatherInfo(currentRegion.encounters[i].encounterIndex, currentRegion.encounters[i].tier, currentRegion.encounters[i].completed);
                    
                    newButton.transform.SetParent(encounterCanvas.GetComponent<RectTransform>());

                    EncounterState currentEncounterState = currentRegion.encounters[i];
                    Encounter newEncounter = new Encounter();

                    Debug.Log(currentEncounterState.tier);
                    newEncounter = EncounterPool.GetEncounterByTierAndIndex(currentEncounterState.tier, currentEncounterState.encounterIndex);
 
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