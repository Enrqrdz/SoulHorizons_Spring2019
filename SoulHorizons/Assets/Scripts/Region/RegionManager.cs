using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;

public class RegionManager : MonoBehaviour
{
    [Header("Must Be Set")]
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject nodeConnectionPrefab;
    [SerializeField] private GameObject inventoryButton;
    [SerializeField] private RegionGenerator regionGenerator;

    private RegionState currentRegion;

    private List<Button> buttons;
    private GameObject encounterMap;

    private Vector3 bossLocation;

    void Start()
    {        
        if(SaveManager.IsSaveLoaded())
        {
            currentRegion = SaveManager.currentGame.GetRegion();
            if(currentRegion == null)
            {
                currentRegion = regionGenerator.GenerateRegion();
                SaveManager.currentGame.SetRegion(currentRegion);
            }
        }
        else
        {
            currentRegion = regionGenerator.GenerateRegion();
            SaveManager.currentGame.SetRegion(currentRegion);
        }

        encounterMap = new GameObject("Map");

        GenerateButtons();
        CreateNodeConnections();

        EventSystem eventSystem = EventSystem.current;
    }

    void Update()
    {
        CheckForInventoryInput();
    }

    public void GoToEncounter(EncounterState encounter)
    {
        if(encounter.isAccessible)
        {
            SaveManager.currentGame.SetCurrentEncounterState(encounter);
            scr_SceneManager.globalSceneManager.ChangeScene(encounter.GetEncounterData().sceneName);
        }
    }

    public void GenerateButtons()
    {
        buttons = new List<Button>();
        int selectedEncounterIndex = 0;

        for(int i = 0; i < currentRegion.map.rings.Count; i++)
        {
            foreach(Node node in currentRegion.map.rings[i])
            {
                GameObject newButton = Instantiate(
                    buttonPrefab, 
                    node.position,
                    Quaternion.identity,
                    encounterMap.transform);

                EncounterButtonManager encounterButtonManager = newButton.GetComponent<EncounterButtonManager>();
                encounterButtonManager.SetEncounterState(node.GetEncounterState());

                Button button = newButton.GetComponent<Button>();
                button.onClick.AddListener(delegate {GoToEncounter(node.encounter);});

                buttons.Add(newButton.GetComponent<Button>());

                if(node.GetEncounterState() == SaveManager.currentGame.GetCurrentEncounterState())
                {
                    selectedEncounterIndex = buttons.Count - 1;
                }

                if(node.GetEncounterState().GetEncounterData().type == EncounterType.Boss)
                {
                    bossLocation = node.position;
                }
            }
        }

        GameObject eventSystem = GameObject.Find("/EventSystem");
        eventSystem.GetComponent<EventSystem>().firstSelectedGameObject = buttons[selectedEncounterIndex].gameObject;      
    }

    private void CreateNodeConnections()
    {
        for(int i = 0; i < currentRegion.map.rings.Count; i++)
        {
            foreach(Node node in currentRegion.map.rings[i])
            {
                foreach(Node connectedNode in node.nextNodes)
                {
                    GameObject newConnection = Instantiate(
                        nodeConnectionPrefab, 
                        node.position,
                        Quaternion.identity,
                        encounterMap.transform);

                    Vector3[] points = new Vector3[2];
                    points[0] = node.position;
                    points[1] = connectedNode.position;

                    LineRenderer lr = newConnection.GetComponent<LineRenderer>();
                    lr.positionCount = points.Length;
                    lr.SetPositions(points);
                }
            }
        }
    }

    public void CheckForInventoryInput()
    {
        if(Input.GetButtonDown("PlayCard2_Button") || Input.GetKeyDown("i"))
        {
            inventoryButton.GetComponent<Button>().onClick.Invoke();
        }
    }

    public void BossCameraSequence()
    {
        CameraController camController = Camera.main.GetComponent<CameraController>();

        camController.AddDestination(bossLocation);   
        camController.SetWaitTime(1f);
    }
}