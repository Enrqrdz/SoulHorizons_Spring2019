using UnityEngine;

public class DataTracker : MonoBehaviour
{
    public static DataTracker Instance;
    public PlayerData playerData;
    public Entity enemyToTrack;
    public static int stageTracker;
    public bool trackData;
    private Entity player;


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
        if(Instance != null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (trackData == true)
        {
            stageTracker = 0;
            ResetPlayerData();
        }
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

    private void Update()
    {
        
    }
}
