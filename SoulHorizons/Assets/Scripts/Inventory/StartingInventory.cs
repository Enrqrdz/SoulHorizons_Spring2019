using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Starting Inventory", menuName = "StartingInventory")]
[System.Serializable]
public class StartingInventory : ScriptableObject 
{
    public List<ActionData> startingInventoryCards;
    public List<int> startingInventoryCardNumbers;
}
