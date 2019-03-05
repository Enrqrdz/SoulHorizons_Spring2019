using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Encounter Map", menuName = "EncounterMap")]
[System.Serializable]
public class EncounterMapData : ScriptableObject
{
    public int numberOfColumns = 6, numberOfRows = 3;

    public Territory playerTile;
    public Territory enemyTile;

    public Territory GetTerrorityAtXAndY(int x, int y)
    {
        Territory newTerritory;

        if(x < numberOfColumns/2)
            newTerritory = playerTile;
        else
            newTerritory = enemyTile;

        return new Territory(newTerritory);
    }

    [System.Serializable]
    public class TerritoryRow
    {
        public List<Territory> column;
    }
}
