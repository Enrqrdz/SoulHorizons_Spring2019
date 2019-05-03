using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Player", menuName = "PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [Tooltip("Size based off of number of enemy phases to track")]
    public List<TrackableData> phaseData = new List<TrackableData>();
}
