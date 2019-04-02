using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raitori_Stages : MonoBehaviour
{
    [HideInInspector]
    public static Raitori_Stages Instance;
    [SerializeField]
    private Entity Raitori;
    public Phase currentPhase;
    public float introTime = 2f;
    public float transitionTime;

    //Stage1
    [Tooltip("Damage required before first phase transition")]
    public int transition1Requirement  = 100;
    [HideInInspector]
    public int transition1Value;

    //Stage2
    [Tooltip("Damage required before second phase transition")]
    public int transition2Requirement = 100;
    [HideInInspector]
    public int transition2Value;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        transition1Value = Raitori._health.max_hp - transition1Requirement;
        transition2Value = transition1Value - transition2Requirement;

        currentPhase = Phase.Idle;
    }

    private void Start()
    {
        currentPhase = Phase.Stage1;
    }

    private void Update()
    {
        if(Raitori._health.hp <= transition1Value && Raitori._health.hp > transition2Value)
        {
            //RunTransition();
            currentPhase = Phase.Stage2;
        }
        if (Raitori._health.hp <= transition2Value)
        {
            //RunTransition();
            currentPhase = Phase.Stage3;
        }
    }

    private IEnumerator RunTransition()
    {
        currentPhase = Phase.Transition;
        yield return new WaitForSecondsRealtime(0f);
    }


    public Phase GetCurrentPhase()
    {
        return currentPhase;
    }
}
