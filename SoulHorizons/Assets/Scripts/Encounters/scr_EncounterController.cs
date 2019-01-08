using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

//Cameron Made this 

public class scr_EncounterController : MonoBehaviour
{

    public static scr_EncounterController globalEncounterController;
    public scr_SceneManager sceneManager;

    public EncounterSave[] encounterArray = new EncounterSave[10];





    public int totalButtons;
    public Button[] buttons;

    public GameObject buttonPrefab;


    public Encounter[] tier1Encounters = new Encounter[1];
    public Encounter[] tier2Encounters = new Encounter[1];
    public Encounter[] tier3Encounters = new Encounter[1];
    public int currentEncounterIndex;

    void OnSceneChange(Scene _scene, LoadSceneMode _mode)
    {
        if (_scene.name == "sn_LocalMap")
        {
            if (Load())
            {
                GenerateButtons();
            }
            else
            {
                //encounterArray = new EncounterSave[totalButtons];
                BuildMap();
                GenerateButtons();
                Save();
            }
        }
    }

    public void OnNewGame()
    {
        BuildMap();
        GenerateButtons();
        Save();
    }

    public void SetEncounterComplete(int _index, bool _completionState)
    {
        encounterArray[_index].completed = _completionState;
        Save();
    }
    public void SetCurrentEncounterComplete()
    {
        encounterArray[currentEncounterIndex].completed = true;
        Save();
    }


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





        /*
        if(SaveLoad.currentGame.encounterSaves == null)  //might be null, check if length is 0?
        {
            BuildMap();
            GenerateButtons();
            

        }
        else
        {
            GetSaveData(SaveLoad.currentGame.encounterSaves);
        }

        */

    }


    void Update()
    {



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
        List<Encounter> selectedEncounters = new List<Encounter>();
        for (int i = 0; i < totalButtons; i++)
        {
            if (i < 1)
            {
                encounterArray[i].tier = 1;
            }
            else if (i >= 1 && i <= 7)
            {
                encounterArray[i].tier = 2;
            }
            else if (i > 7)
            {
                encounterArray[i].tier = 3;
            }

        }


        for (int i = 0; i < buttons.Length; i++)
        {
            encounterArray[i].completed = false; 
            bool _goodPick = false;
            int _tries = 0;
            int num = 0;
            //need to make sure we dont pick the same Encounter 2x. 
            if (encounterArray[i].tier == 1)
            {
                /*
                while (!_goodPick  && _tries < 10)
                {
                    num = UnityEngine.Random.Range(0, tier1Encounters.Length);
                    if (!selectedEncounters.Contains(tier1Encounters[num]))
                    {
                        _goodPick = true; 
                    }
                    _tries++; 
                }
                */
                num = UnityEngine.Random.Range(0, tier1Encounters.Length);
                encounterArray[i].encounterNumber = num;
                //selectedEncounters.Add(tier1Encounters[num]);
            }
            else if (encounterArray[i].tier == 2)
            {
                num = UnityEngine.Random.Range(0, tier2Encounters.Length);
                encounterArray[i].encounterNumber = num;
            }
            else if (encounterArray[i].tier == 3)
            {
                num = UnityEngine.Random.Range(0, tier3Encounters.Length);
                encounterArray[i].encounterNumber = num;
            }

        }




    }

    public void GetSaveData(EncounterSave[] _encounters)
    {

        encounterArray = new EncounterSave[_encounters.Length];


        for (int i = 0; i < _encounters.Length; i++)
        {
            encounterArray[i].encounterNumber = _encounters[i].encounterNumber;
            encounterArray[i].tier = _encounters[i].tier;
            encounterArray[i].completed = _encounters[i].completed;
        }
        GenerateButtons();

    }

    public void GenerateButtons()
    {
        buttons = new Button[totalButtons];
        if(scr_SceneManager.globalSceneManager.ReturnSceneName() == "sn_LocalMap")
        {
            GameObject encounterCanvas = GameObject.FindWithTag("EncounterCanvas");

            for (int i = 0; i < totalButtons; i++)
            {
                if (encounterCanvas.gameObject != null)
                {
                    GameObject newButton = Instantiate(buttonPrefab);
                    newButton.GetComponent<scr_EncounterButtons>().GatherInfo(encounterArray[i].encounterNumber, encounterArray[i].tier, encounterArray[i].completed);
                    
                    newButton.transform.SetParent(encounterCanvas.GetComponent<RectTransform>());
                    Encounter newEncounter = new Encounter();
                    if (encounterArray[i].tier == 1)
                    {
                        newEncounter = tier1Encounters[encounterArray[i].encounterNumber];
                    }
                    else if (encounterArray[i].tier == 2)
                    {
                        newEncounter = tier2Encounters[encounterArray[i].encounterNumber];
                    }
                    else if (encounterArray[i].tier == 3)
                    {
                        newEncounter = tier3Encounters[encounterArray[i].encounterNumber];
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



    public void Save()
    {
        EncounterData data = new EncounterData();
        data.encounters = new EncounterSave[totalButtons];
        for (int i = 0; i < totalButtons; i++)
        {
            data.encounters[i] = new EncounterSave();
            data.encounters[i].Clone(encounterArray[i]);
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/encounterShit.json"); //you can call it anything you want
        bf.Serialize(file, data);
        file.Close();
    }


    public bool Load()
    {
        if (File.Exists(Application.persistentDataPath + "/encounterShit.json"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/encounterShit.json", FileMode.Open);
            EncounterData data = new EncounterData();
            data = (EncounterData)bf.Deserialize(file);
            file.Close();
            totalButtons = data.encounters.Length;
            encounterArray = new EncounterSave[data.encounters.Length];
            for (int i = 0; i < totalButtons; i++)
            {
                encounterArray[i] = new EncounterSave();
                encounterArray[i].Clone(data.encounters[i]);
            }
            return true;
        }
        else
        {
            return false;
        }
    }










    /*
        void CycleEvents(){

	    //for Loop to deactivate all of the buttons - Here we are doing it to cycle the events.  We'll remove this loop later.
	    for (int i = 0; i < buttons.Length; i++) {
		    buttons [i].gameObject.SetActive(false); 
	    } 

	    //Loop to Randomize which buttons are on/off.  Also probably not exactly what we want, but will do for now. - Plus it dont even work right 
	    for (int i = 0; i < activeScenarios; i++) {							//Will run as many times as you want Active Scenarios
		    bool good = false; 														//temp bool for incoming while loop
		    int tries = 0; 															// This is not necessary, but a precaution.  If numberOfActiveScenariosn > the total number of buttons in the array, Unity dies.  This prevents it from running more than x times.  <see while loop> 
		    while (!good  && tries < 50) {
			    int num = Random.Range (0, buttons.Length); 						//generates a random number between 0 and (total number of buttons - 1) *since there is a button[0]
			    if (!buttons [num].gameObject.activeSelf) {							//if this random button is not active 
				    buttons [num].gameObject.SetActive (true); 						//turn on this random button
				    good = buttons [num].gameObject.activeSelf;						// good = the return value of whether or not the button is active

			    } 
			    else {																//Else
				    tries++; 														//try again
			    }

		    }
	    }
    }
     * 
     * */
}



[System.Serializable]
public class EncounterSave
{
    public int tier;
    public int encounterNumber;
    public bool completed;

    public EncounterSave()
    {
        completed = false;
        tier = 0;
        encounterNumber = 0;
    }
    public void Clone(EncounterSave _encounter)
    {
        completed = _encounter.completed;
        tier = _encounter.tier;
        encounterNumber = _encounter.encounterNumber;
    }


}

[System.Serializable]
public class EncounterData
{
    public EncounterSave[] encounters;



}


