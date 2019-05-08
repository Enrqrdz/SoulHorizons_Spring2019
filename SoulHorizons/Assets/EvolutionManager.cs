using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EvolutionManager : MonoBehaviour
{
    public float timeIntervalLength;
    public TextMeshProUGUI stageDisplay;
    private int numberOfStages;
    private int currentStage;
    private float timeIntervalStart;
    private float totalTime;
    private MushineAI evolveableEntityAI;

    private void Start()
    {
        timeIntervalStart = timeIntervalLength;
        Debug.Log("Number of stages" + numberOfStages);
        numberOfStages = DataTracker.Instance.playerData.phaseData.Count;
        currentStage = numberOfStages;
        Debug.Log("Number of stages" + numberOfStages);
        evolveableEntityAI = GameObject.FindGameObjectWithTag("Enemy").GetComponent<MushineAI>();
        totalTime = timeIntervalLength * numberOfStages;
        stageDisplay.SetText("Stage:" + currentStage);
    }

    private void Update()
    {        

        if(totalTime > 0)
        {
            timeIntervalLength -= Time.deltaTime;
            totalTime -= Time.deltaTime;

            if (timeIntervalLength <= 0)
            {
                currentStage--;
                stageDisplay.SetText("Stage: " + currentStage);
                timeIntervalLength = timeIntervalStart;
                evolveableEntityAI.NextPhase();
            }
        }
        else
        {
            Debug.Log("Simulation Over");
            InputManager.cannotMove = true;
        }
    }
}
