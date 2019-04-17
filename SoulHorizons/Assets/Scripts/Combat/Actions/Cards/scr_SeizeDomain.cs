using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Cards/SeizeDomain")]
[RequireComponent(typeof(AudioSource))]

public class scr_SeizeDomain : ActionData
{
    public float duration;
    //public Color newColor;
    private AudioSource PlayCardSFX;
    public AudioClip SeizeDomainSFX;
    public override void Activate()
    {    
        PlayCardSFX = GameObject.Find("ActionManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = SeizeDomainSFX;
        PlayCardSFX.Play();
        DomainManager.Instance.Activate(duration);
    }
}

public class DomainManager : MonoBehaviour
{
    public static DomainManager Instance;

    private int numberOfColumns;
    private int numberOfRows;
    public List<int> playerColumns = new List<int>();
    private int adjacentColumn;


    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        numberOfColumns = scr_Grid.GridController.columnSizeMax;
        numberOfRows = scr_Grid.GridController.columnSizeMax;

        for (int i = 0; i < numberOfColumns; i++)
        {
            for (int j = 0; j < numberOfRows; j++)
            {
                if (scr_Grid.GridController.grid[i, j].territory.name != TerrName.Player)
                {
                    return;
                }
                if (j == numberOfRows - 1)
                {
                    playerColumns.Add(i);
                    adjacentColumn = playerColumns.Count;
                }
            }
        }
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
        playerColumns.Add(adjacentColumn);

        for(int j = 0; j < numberOfRows; j++)
        {
            if(scr_Grid.GridController.grid[adjacentColumn, j].entityOnTile.type != EntityType.Player)
            {
                PushBack(adjacentColumn, j);
            }
            scr_Grid.GridController.SetTileTerritory(adjacentColumn, j, TerrName.Player, scr_TileDict.colorDict[TerrName.Player]);
        }
    }

    private void PushBack(int x, int y)
    {
        
        int newX = x + 1;

        if(newX < scr_Grid.GridController.columnSizeMax)
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

    private void PushUp(int x, int y)
    {
        int newY = y;

        if (newY < scr_Grid.GridController.rowSizeMax - 1)
        {
            newY = y + 1;
        }

        if (scr_Grid.GridController.CheckIfOccupied(x, newY) == false)
        {
            scr_Grid.GridController.grid[x, y].entityOnTile.SetTransform(x, newY);
        }
        else
        {
            PushDown(x, y);
        }
    }

    private void PushDown(int x, int y)
    {
        int newY = y;

        if (newY > 0)
        {
            newY = y - 1;
        }

        if (scr_Grid.GridController.CheckIfOccupied(x, newY) == false)
        {
            scr_Grid.GridController.grid[x, y].entityOnTile.SetTransform(x, newY);
        }
        else
        {
            PushBack(x,y);
        }
    }

    private void MoveSomewhereAvailable(int x, int y)
    {
        bool foundSomewhere = false;

        for(int i = playerColumns[adjacentColumn]; i < numberOfColumns; i++)
        {
            for(int j = 0; j < numberOfRows; j++)
            {
                if(scr_Grid.GridController.CheckIfOccupied(i,j) == false)
                {
                    foundSomewhere = true;
                    scr_Grid.GridController.grid[x, y].entityOnTile.SetTransform(i, j);
                }
            }
        }

        //Only called if the entity is not able to move anywhere on the grid
        if(foundSomewhere == false)
        {
            scr_Grid.GridController.grid[x, y].entityOnTile.Death();
        }
    }

    public void Deactivate()
    {
        playerColumns.Remove(adjacentColumn);
    }

    /*
    //Seize Domain Card
    public void seizeDomain(float duration)
    {
        bool colFound = false;

        for (int i = 0; i < numberOfColumns; i++)
        {

            for (int j = 0; j < numberOfRows; j++)
            {
                //Debug.Log(scr_Grid.GridController.grid[i, j].territory.name);
                if (grid[i, j].territory.name != TerrName.Player)
                {
                    //Debug.Log("Column: " + i);
                    colFound = true;

                    if (!grid[i, j].occupied)
                    {
                        //Debug.Log("SEIZING!");
                        SetTileTerritory(i, j, TerrName.Player, scr_TileDict.colorDict[TerrName.Player]);
                    }
                }
            }
            //Debug.Log(colFound);
            if (colFound)
            {
                StartCoroutine(reSeize(duration));
                break;
            }


        }
    }

    private IEnumerator reSeize(float waitTime)
    {
        Debug.Log("RESEIZE");
        yield return new WaitForSeconds(waitTime);
        bool colFound = false;
        for (int i = numberOfColumns - 1; i >= 0; i--)
        {

            for (int j = 0; j < numberOfRows; j++)
            {
                //Debug.Log(scr_Grid.GridController.grid[i, j].territory.name);
                if (grid[i, j].territory.name != TerrName.Enemy)
                {
                    //Debug.Log("Column: " + i);
                    colFound = true;

                    if (!grid[i, j].occupied)
                    {
                        //Debug.Log("SEIZING!");
                        SetTileTerritory(i, j, TerrName.Enemy, scr_TileDict.colorDict[TerrName.Enemy]);
                    }
                }
            }
            //Debug.Log(colFound);
            if (colFound)
            {
                break;
            }
        }
    }
}*/
}

