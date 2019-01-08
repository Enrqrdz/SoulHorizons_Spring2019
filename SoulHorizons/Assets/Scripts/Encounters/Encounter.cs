using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "New Encounter", menuName = "Encounter")]
public class Encounter : ScriptableObject {

    public new string name;
    public string sceneName = "sn_GridTest";                        //Cameron made this variable.  It is used by the scene manager to go to a specific string.  For now all of our combat happens in GridTest, but that might change?


    [Header("Grid Size")]
    public int xWidth = 3;
    public int yHeight = 3;
    [Header("Terrain")]
    public string defaultTerrain;
    public List<Terrain_Entry> tiles = new List<Terrain_Entry>();
    [Header("Assets")]
    public EntitySpawnLocation[] entities;
    public int mouse;
    public int mush;
    public int archer;

    [Header("Territory")]
    public TerritoryRow[] territoryColumn;




    /*
    void OnValidate()
    {
        territoryColumn = new TerritoryRow[length];
        StartCoroutine(WaitALittle());
        
        

    }

    IEnumerator WaitALittle()
    {
        yield return new WaitForEndOfFrame();

        for (int x = 0; x < territoryColumn.Length; x++)
        {
            territoryColumn[x].territoryRow = new scr_Tile.Territory[width];
        }
    }
    */



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
        //TODO: how does the terrain system work on the grid? This will decide what needs to go in this class.
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

