using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EvolutionManager : MonoBehaviour
{
    public float timeIntervalLength;
    public TextMeshProUGUI stageDisplay;
    private int numberOfStages;
    private int currentStage = 1;
    private float timeIntervalStart;
    private float totalTime;
    private MushineAI evolveableEntityAI;

    private void Start()
    {
        timeIntervalStart = timeIntervalLength;
        numberOfStages = DataTracker.Instance.playerData.phaseData.Count;
        evolveableEntityAI = GameObject.FindGameObjectWithTag("Enemy").GetComponent<MushineAI>();
        totalTime = timeIntervalLength * numberOfStages;
        stageDisplay.SetText("Stage: " + currentStage);
        DataTracker.Instance.StartPhase();
    }

    private void Update()
    {        

        if(totalTime > 0)
        {
            timeIntervalLength -= Time.deltaTime;
            totalTime -= Time.deltaTime;

            if (timeIntervalLength <= 0)
            {
                DataTracker.Instance.EndPhase();
                currentStage++;
                stageDisplay.SetText("Stage: " + currentStage);
                timeIntervalLength = timeIntervalStart;
                DataTracker.Instance.StartPhase();
                evolveableEntityAI.NextPhase();
            }
        }
        else
        {
            Debug.Log("Simulation Over");
            InputManager.cannotMove = true;
            InputManager.cannotInputAnything = true;
            InputManager.canInputMantras = false;
            DataTracker.Instance.EndPhase();
        }
    }
}
