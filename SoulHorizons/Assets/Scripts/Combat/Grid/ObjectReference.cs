using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectReference : MonoBehaviour
{
    public static ObjectReference Instance;
    [HideInInspector]
    public AudioSource ActionManager;
    [HideInInspector]
    public RegionManager RegionManager;
    [HideInInspector]
    public GameObject EventSystem;


    private void Awake()
    {
        InitializeSingleton();
        ActionManager = GameObject.Find("ActionManager").GetComponent<AudioSource>();
        RegionManager = GameObject.Find("RegionManager").GetComponent<RegionManager>();
        EventSystem = GameObject.Find("EventSystem");
    }

    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DontDestroyOnLoad(Instance);
        }
    }
}
