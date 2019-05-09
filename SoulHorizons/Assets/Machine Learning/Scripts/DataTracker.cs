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
    private int horizontalPosition;
    private int verticalPosition;
    private int horizontalSteps = 0;
    private int verticalSteps = 0;
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
            horizontalPosition = player._gridPos.x;
            verticalPosition = player._gridPos.y;
            ResetPlayerData();
            TrackSingleEnemy();
        }
    }

    private void Update()
    {
        if(player._gridPos.x != horizontalPosition)
        {
            horizontalPosition = player._gridPos.x;
            CalculateHorizontalDistance();
        }
        if(player._gridPos.y != verticalPosition)
        {
            verticalPosition = player._gridPos.y;
            CalculateVerticalDistance();
        }
    }

    private void TrackSingleEnemy()
    {
        enemyToTrack = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Entity>();
    }

    private void ResetPlayerData()
    {
        int i = 0;
        foreach (TrackableData phase in playerData.phaseData)
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
        horizontalSteps = 0;
        verticalSteps = 0;
        CalculateHorizontalDistance();
        CalculateVerticalDistance();
        totalSteps = 0;
        attackCounter = 0;
        startTime = Time.time;
    }

    //When enemy ends phase
    public void EndPhase()
    {
        endTime = Time.time;
        CalculateTotals();
        trackerIndex++;
    }

    //When player shoots
    public void AddAttackCounter()
    {
        attackCounter++;
    }

    //When player moves horizontally
    public void CalculateHorizontalDistance()
    {
        Debug.Log("Calculating horizontal distance");
        horizontalSteps++;
        playerData.phaseData[trackerIndex].horizontalDistance += horizontalPosition - enemyToTrack._gridPos.x;
    }

    //When enemy moves vertically
    public void CalculateVerticalDistance()
    {
        verticalSteps++;
        playerData.phaseData[trackerIndex].verticalDistance += verticalPosition - enemyToTrack._gridPos.y;
    }

    //When enemy ends phase
    private void CalculateTotals()
    {
        totalTime = endTime - startTime;
        totalSteps = verticalSteps + horizontalSteps;

        playerData.phaseData[trackerIndex].attackFrequency = attackCounter / totalTime;
        playerData.phaseData[trackerIndex].movementFrequency = totalSteps / totalTime;

        CalculateHorizontalDistance();
        playerData.phaseData[trackerIndex].horizontalDistance /= horizontalSteps;
        CalculateVerticalDistance();
        playerData.phaseData[trackerIndex].verticalDistance /= verticalSteps;
    }
}
