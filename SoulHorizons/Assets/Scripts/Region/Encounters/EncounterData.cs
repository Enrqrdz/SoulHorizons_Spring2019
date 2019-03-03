using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Encounter", menuName = "Encounter")]
[System.Serializable]
public class EncounterData : ScriptableObject
{
    public new string name;
    public string sceneName = SceneNames.ENCOUNTER;
    public int tier;

    [Header("Entities")]
    public EntitySpawnLocation[] entities;

    [SerializeField]
    private EncounterMapData mapData;

    public int GetNumberOfMouses()
    {
        return CountEnemy<scr_Critter>();
    }

    public int GetNumberOfMush()
    {
        return CountEnemy<scr_FoulTrifling>();
    }

    public int GetNumberOfArchers()
    {
        return CountEnemy<scr_ExiledArcher>();
    }

    private int CountEnemy<T>()
    {
        int count = 0;

        foreach(EntitySpawnLocation entitySpawn in entities)
        {
            if(entitySpawn.entity.GetComponent<T>() != null)
                count++;
        }

        return count;
    }

    public Territory GetTerrorityAtXAndY(int x, int y)
    {
        return mapData.GetTerrorityAtXAndY(x, y);
    }

    public int GetNumberOfColumns()
    {
        return mapData.numberOfColumns;
    }

    public int GetNumberOfRows()
    {
        return mapData.numberOfRows;
    }

    [System.Serializable]
    public class EntitySpawnLocation
    {
        public Entity entity;
        public int x;
        public int y;

        public EntitySpawnLocation(Entity ent, int a, int b)
        {
            entity = ent;
            x = a;
            y = b;
        }
    }
}

