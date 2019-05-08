using System;
using UnityEngine;

public class DataTracker : MonoBehaviour
{
    public static DataTracker Instance;
    public PlayerData playerData;
    public Entity enemyToTrack;
    [Tooltip("Select this for new data to be tracked")]
    public bool trackData;

    private Entity player;
    private float startTime;
    private float endTime;
    private float totalTime;
    private static int trackerIndex;
    private int horizontalSteps;
    private int verticalSteps;
    private float totalSteps;
    private float attackCounter;

    private void Awake()
    {
        try
        {
            player = ObjectReference.Instance.PlayerEntity;
        }
        catch
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
        }
        Instance = this;
    }

    private void Start()
    {
        if (trackData == true)
        {
            trackerIndex = 0;
            ResetPlayerData();
            TrackSingleEnemy();
        }
    }

    private void TrackSingleEnemy()
    {
        enemyToTrack = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Entity>();
    }

    private void ResetPlayerData()
    {
        int i = 0;
        foreach (TrackableData phases in playerData.phaseData)
        {
            for (int j = 0; j < 4; j++)
            {
                playerData.phaseData[i].attackFrequency = 0;
                playerData.phaseData[i].movementFrequency = 0;
                playerData.phaseData[i].horizontalDistance = 0;
                playerData.phaseData[i].verticalDistance = 0;
            }
        }
    }

    //When enemy goes into new stage
    public void StartPhase()
    {
        trackerIndex++;
        horizontalSteps = 0;
        verticalSteps = 0;
        totalSteps = 0;
        attackCounter = 0;
        startTime = Time.time;
    }

    //When enemy ends phase
    public void EndPhase()
    {
        endTime = Time.time;
        CalculateTotals();
    }

    //When enemy moves horizontally
    public void CalculateHorizontalDistance()
    {
        horizontalSteps++;
        playerData.phaseData[trackerIndex].horizontalDistance += enemyToTrack._gridPos.x - player._gridPos.x;
        playerData.phaseData[trackerIndex].horizontalDistance /= horizontalSteps;
    }

    //When enemy moves vertically
    public void CalculateVerticalDistance()
    {
        verticalSteps++;
        playerData.phaseData[trackerIndex].verticalDistance += enemyToTrack._gridPos.y - player._gridPos.y;
        playerData.phaseData[trackerIndex].verticalDistance /= verticalSteps;
    }

    //When enemy ends phase
    private void CalculateTotals()
    {
        totalTime = endTime - startTime;
        totalSteps = verticalSteps + horizontalSteps;

        playerData.phaseData[trackerIndex].attackFrequency = attackCounter / totalTime;
        playerData.phaseData[trackerIndex].movementFrequency = totalSteps / totalTime;
    }
}
