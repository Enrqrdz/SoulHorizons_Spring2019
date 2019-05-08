using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrackableData
{
    [Tooltip("How often the player moves")]
    public float movementFrequency;
    [Tooltip("How often the player attacks")]
    public float attackFrequency;
    [Tooltip("How close the player is to the enemy, Horizontally")]
    public int horizontalDistance;
    [Tooltip("How close the player is to the enemy, Vertically")]
    public int verticalDistance;
}
