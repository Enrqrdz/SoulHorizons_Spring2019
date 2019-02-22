using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public EncounterData Encounter;
    public Tile tileForGrid;
    public Sprite tileSprite;
    public string parentObjectName = "Grid";
    public Tile[,] grid;
    public Entity[] startingEntities;
    public static Vector2 gridCenter;
    public int columnSizeMax;
    public int rowSizeMax;

    [SerializeField] private Vector2 spaceBetweenTiles = new Vector2(0.6f, 0.5f); //Place holders. Change to what fits best

    public void GenerateEncounter()
    {
        Encounter = SaveManager.currentGame.GetCurrentEncounter();
        columnSizeMax = Encounter.GetNumberOfColumns();
        rowSizeMax = Encounter.GetNumberOfRows();
        grid = new Tile[columnSizeMax, rowSizeMax];
        gridCenter.x = (spaceBetweenTiles.x * (columnSizeMax - 1) / 2);
        gridCenter.y = (spaceBetweenTiles.y * rowSizeMax / 2);
        BuildGrid();
    }

    private void BuildGrid()
    {
        for (int j = 0; j < rowSizeMax; j++)
        {
            for (int i = 0; i < columnSizeMax; i++)
            {
                Vector2 positionToAdd = new Vector2((i * spaceBetweenTiles.x), (j * spaceBetweenTiles.y) - 1);
                Tile tileToAdd = Instantiate(tileForGrid, positionToAdd, Quaternion.identity);
                tileToAdd.transform.parent = GameObject.Find(parentObjectName).transform;
                tileToAdd.name = (rowSizeMax - j) + "_" + (i+1);
                tileToAdd.territory = Encounter.GetTerrorityAtXAndY(i, j);
                tileToAdd.GetComponent<SpriteRenderer>().sprite = tileSprite;
                grid[i, j] = tileToAdd;
            }
        }
        Debug.Log("Done Building Grid");
    }

    public void SetStartingEntities()
    {
        Debug.Log("Getting Entities");
        startingEntities = new Entity[Encounter.entities.Length];
        for (int i = 0; i < startingEntities.Length; i++)
        {
            Entity temporaryEntity = new Entity();
            temporaryEntity = Instantiate(Encounter.entities[i].entity, Vector3.zero, Quaternion.identity);
            temporaryEntity.InitPosition(Encounter.entities[i].x, Encounter.entities[i].y);
            startingEntities[i] = temporaryEntity;
            Debug.Log("Step " + i + " done");
        }
        Debug.Log("Done");
    }

    public Tile[,] GetGrid()
    {
        return grid;
    }

    public Entity[] GetStartingEntities()
    {
        return startingEntities;
    }
    
}
