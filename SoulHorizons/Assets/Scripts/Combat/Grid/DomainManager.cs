using System.Collections;
using UnityEngine;

public class DomainManager : MonoBehaviour
{
    public static DomainManager Instance;

    public int numberOfColumns;
    public int numberOfRows;
    public int playerColumns;
    public int columnToBeSeized;
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
        playerColumns = (numberOfColumns / 2) - 1;
        columnToBeSeized = playerColumns + 1;
    }

    //This begins a recursive chain to attempt the following movements:
    // 1) x+1, y
    // 2) (x+1) + 1, y
    // 3) Repeat until the end of the grid
    // n) Base Case any open position, if not, kill the entity.

    public void Activate(float duration)
    {
        playerColumns++;

        territorySeized = scr_Grid.GridController.grid[columnToBeSeized, 0].territory.name;

        for (int j = 0; j < numberOfRows; j++)
        {
            Entity entityOnTile = scr_Grid.GridController.grid[columnToBeSeized, j].entityOnTile;
            if (entityOnTile != null)
            {
                if(entityOnTile.gridPositions != null)
                {
                    largeEntity = true;
                }
                PushRight(columnToBeSeized, j);
            }
            scr_Grid.GridController.SetTileOccupied(false, columnToBeSeized, j, null);
            scr_Grid.GridController.SetTileTerritory(columnToBeSeized, j, TerrName.Player, scr_TileDict.colorDict[TerrName.Player]);
        }

        columnToBeSeized = playerColumns + 1;

        StartCoroutine(SeizeDomain(duration));
    }

    public void Deactivate()
    {
        playerColumns--;
        columnToBeSeized = playerColumns + 1;

        for (int j = 0; j < numberOfRows; j++) 
        {
            Entity entityOnTile = scr_Grid.GridController.grid[columnToBeSeized, j].entityOnTile;
            if (entityOnTile != null && entityOnTile.type == EntityType.Player)
            {
                PushLeft(columnToBeSeized, j);
            }
            scr_Grid.GridController.SetTileOccupied(false, columnToBeSeized, j, null);
            scr_Grid.GridController.SetTileTerritory(columnToBeSeized, j, territorySeized, scr_TileDict.colorDict[territorySeized]);
        }
    }

    private IEnumerator SeizeDomain(float duration)
    {
        yield return new WaitForSeconds(duration);
        Deactivate();
    }

    private void PushRight(int x, int y)
    {
        int xToMoveTo = x + 1;
        
        if (xToMoveTo < numberOfColumns)
        {
            if (scr_Grid.GridController.CheckIfOccupied(xToMoveTo, y) == false)
            {
                scr_Grid.GridController.grid[x, y].entityOnTile.SetTransform(xToMoveTo, y);
            }
            else
            {
                PushRight(xToMoveTo, y);
            }
        }
        else
        {
            MoveSomewhereAvailable(columnToBeSeized, y, false);
        }
    }

    private void PushLeft(int x, int y)
    {
        int xToMoveTo = x - 1;

        if (xToMoveTo >= 0)
        {
            if (scr_Grid.GridController.CheckIfOccupied(xToMoveTo, y) == false)
            {
                scr_Grid.GridController.grid[x, y].entityOnTile.SetTransform(xToMoveTo, y);
            }
            else
            {
                PushLeft(xToMoveTo, y);
            }
        }
        else
        {
            MoveSomewhereAvailable(columnToBeSeized, y, true);
        }
    }


    private void MoveSomewhereAvailable(int x, int y, bool isPlayer)
    {
        bool foundSomewhere = false;

        if (isPlayer)
        {
            for (int i = x - 1; i >= 0; i--)
            {
                for (int j = 0; j < numberOfRows; j++)
                {
                    if (scr_Grid.GridController.grid[i, j].occupied == false)
                    {
                        foundSomewhere = true;
                        scr_Grid.GridController.grid[x, y].entityOnTile.SetTransform(i, j);
                    }
                }
            }
        }

        else
        {
            for (int i = x + 1; i < numberOfColumns; i++)
            {
                for (int j = 0; j < numberOfRows; j++)
                {
                    if (scr_Grid.GridController.grid[i, j].occupied == false)
                    {
                        foundSomewhere = true;
                        scr_Grid.GridController.grid[x, y].entityOnTile.SetTransform(i, j);
                    }
                }
            }
        }


        //Only called if the entity is not able to move anywhere on the grid
        if (foundSomewhere == false)
        {
            scr_Grid.GridController.grid[x, y].entityOnTile.Death();
        }
    }
}
