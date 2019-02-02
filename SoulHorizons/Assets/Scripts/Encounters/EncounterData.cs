using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "New Encounter", menuName = "Encounter")]
[System.Serializable]
public class EncounterData : ScriptableObject {

    public new string name;
    public string sceneName = SceneNames.ENCOUNTER;


    [Header("Grid Size")]
    public int columnNumber = 3;
    public int rowNumber = 3;
    [Header("Terrain")]
    public string defaultTerrain;
    public List<Terrain_Entry> tiles = new List<Terrain_Entry>();
    [Header("Assets")]
    public EntitySpawnLocation[] entities;
    public int mouseNum;
    public int mushNum;
    public int archerNum;
    [Header("Encounter Data")]
    public int tier;

    [Header("Territory")]
    public TerritoryRow[] territoryColumn;

    [System.Serializable]
    public class Asset_Entry
    {
        public GameObject asset;
        public int x;
        public int y;
    }

    [System.Serializable]
    public class Terrain_Entry
    {
        public string type;
        public int x;
        public int y;
    }

    [System.Serializable]
    public class TerritoryRow
    {
        public Territory[] territoryRow; 
    }

    [System.Serializable]
    public class EntitySpawnLocation
    {
        public scr_Entity _entity;
        public int x;
        public int y;

        public EntitySpawnLocation(scr_Entity ent, int a, int b)
        {
            _entity = ent;
            x = a;
            y = b;
        }
    }
}

