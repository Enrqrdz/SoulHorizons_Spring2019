using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectReference : MonoBehaviour
{
    public static ObjectReference Instance;
    //[HideInInspector]
    public AudioSource ActionManager;
    //[HideInInspector]
    public RegionManager RegionManager;
    //[HideInInspector]
    public GameObject EventSystem;
    //[HideInInspector]
    public GameObject Player;
    //[HideInInspector]
    public Entity PlayerEntity;


    private void Awake()
    {
        InitializeSingleton();
        ActionManager = GameObject.Find("ActionManager").GetComponent<AudioSource>();
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerEntity = Player.GetComponent<Entity>();
    }

    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
