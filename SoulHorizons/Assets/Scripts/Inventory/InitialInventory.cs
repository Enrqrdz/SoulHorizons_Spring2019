using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Encounter", menuName = "test")]
public class InitialInventory : ScriptableObject 
{
    private static List<CardData> StartingInventory;
}
