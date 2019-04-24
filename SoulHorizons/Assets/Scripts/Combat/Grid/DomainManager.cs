using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DomainManager : MonoBehaviour
{
    public static DomainManager Instance;

    private int numberOfColumns;
    private int numberOfRows;
    public List<int> playerColumns = new List<int>();
    private int adjacentColumn;
    private bool largeEntity = false;
    private TerrName territorySeized;


    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        numberOfColumns = scr_Grid.GridController.columnSizeMax;
        numberOfRows = scr_Grid.GridController.rowSizeMax;

        for (int i = 0; i < numberOfColumns; i++)
        {
            for (int j = 0; j < numberOfRows; j++)
            {
                if (scr_Grid.GridController.grid[i, j].territory.name != TerrName.Player)
                {
                    territorySeized = scr_Grid.GridController.grid[i, j].territory.name;
                    return;
                }
                if (j == numberOfRows - 1)
                {
                    playerColumns.Add(i);
                }
            }
        }

        adjacentColumn = playerColumns.Count;
    }

    //This begins a recursive chain to attempt the following movements:
    // 1) x+1, y
    // 2) x+1, y+1
    // 3) x+1, y-1
    // 4) x+2, y...
    // 5) Repeat
    // n) Base Case any open position, if not, kill the entity.

    public void Activate(float duration)
    {
        adjacentColumn = playerColumns.Count;
        playerColumns.Add(adjacentColumn);

        for (int j = 0; j < numberOfRows; j++)
        {
            Entity entityOnTile = scr_Grid.GridController.grid[adjacentColumn, j].entityOnTile;
            if (entityOnTile != null)
            {
                if(entityOnTile.gridPositions != null)
                {
                    largeEntity = true;
                }
                PushRight(adjacentColumn, j);
            }
            scr_Grid.GridController.SetTileTerritory(adjacentColumn, j, TerrName.Player, scr_TileDict.colorDict[TerrName.Player]);
        }
        StartCoroutine(SeizeDomain(duration));
    }

    public void Deactivate()
    {
        for (int j = 0; j < numberOfRows; j++) 
        {
            if (scr_Grid.GridController.grid[playerColumns[playerColumns.Count - 1], j].entityOnTile != null &&
                scr_Grid.GridController.grid[playerColumns[playerColumns.Count - 1], j].entityOnTile.type == EntityType.Player)
            {
                PushLeft(playerColumns[playerColumns.Count - 1], j);
            }
            scr_Grid.GridController.SetTileTerritory(playerColumns[playerColumns.Count - 1], j, territorySeized, scr_TileDict.colorDict[territorySeized]);
        }

        playerColumns.RemoveAt(playerColumns.Count - 1);
        adjacentColumn = playerColumns.Count;
    }

    private IEnumerator SeizeDomain(float duration)
    {
        Debug.Log("Seize Domain has started!");
        yield return new WaitForSeconds(duration);
        Deactivate();
    }

    private void PushRight(int x, int y)
    {
        int newX = x + 1;

        if (newX < scr_Grid.GridController.columnSizeMax)
        {
            if (scr_Grid.GridController.CheckIfOccupied(newX, y) == false)
            {
                scr_Grid.GridController.grid[x, y].entityOnTile.SetTransform(newX, y);
            }
            else
            {
                PushUp(newX, y);
            }
        }
        else
        {
            MoveSomewhereAvailable(x, y);
        }
    }

    private void PushUp(int newX, int y)
    {
        int newY = y;

        if (newY < scr_Grid.GridController.rowSizeMax - 1)
        {
            newY = y + 1;
        }

        if (scr_Grid.GridController.CheckIfOccupied(newX, newY) == false)
        {
            scr_Grid.GridController.grid[newX - 1, y].entityOnTile.SetTransform(newX, newY);
        }
        else
        {
            PushDown(newX, y);
        }
    }

    private void PushDown(int newX, int y)
    {
        int newY = y;

        if (newY > 0)
        {
            newY = y - 1;
        }

        if (scr_Grid.GridController.CheckIfOccupied(newX, newY) == false)
        {
            scr_Grid.GridController.grid[newX - 1, y].entityOnTile.SetTransform(newX, newY);
        }
        else if (scr_Grid.GridController.grid[newX, y].entityOnTile.type != EntityType.Player)
        {
            PushRight(newX, y);
        }
        else if (scr_Grid.GridController.grid[newX, y].entityOnTile.type == EntityType.Player)
        {
            PushLeft(newX, y);
        }
    }
    private void PushLeft(int x, int y)
    {
        int newX = x;

        if (newX > 0)
        {
            newX = x - 1;

            if (scr_Grid.GridController.CheckIfOccupied(newX, y) == false)
            {
                scr_Grid.GridController.grid[x, y].entityOnTile.SetTransform(newX, y);
            }
            else
            {
                PushUp(newX, y);
            }
        }
        else
        {
            MoveSomewhereAvailable(x, y);
        }
    }


    private void MoveSomewhereAvailable(int x, int y)
    {
        bool foundSomewhere = false;

        for (int i = adjacentColumn; i < numberOfColumns; i++)
        {
            for (int j = 0; j < numberOfRows; j++)
            {
                if (scr_Grid.GridController.grid[i, j].occupied == false)
                {
                    foundSomewhere = true;
                    scr_Grid.GridController.grid[adjacentColumn, y].entityOnTile.SetTransform(i, j);
                }
            }
        }

        //Only called if the entity is not able to move anywhere on the grid
        if (foundSomewhere == false && scr_Grid.GridController.grid[x, y].entityOnTile != null)
        {
            scr_Grid.GridController.grid[x, y].entityOnTile.Death();
        }
    }
}
