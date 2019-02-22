using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static Grid Instance { get; private set; }
    [SerializeField]
    private GridGenerator GridGenerator;

    public int columnSizeMax;
    public int rowSizeMax;
    public Tile[,] grid;
    public Entity[] activeEntities;

    private void Awake()
    {
        InitializeSingleton();
        //IntializeGridGenerator();
        GridGenerator.GenerateEncounter();
        SetGrid();
        GridGenerator.SetStartingEntities();
        SetEntities();
        InitializePlayer();
    }

    private void IntializeGridGenerator()
    {
        GridGenerator = GetComponent<GridGenerator>();
    }

    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }

    private static void InitializePlayer()
    {
        scr_InputManager.cannotInput = false;
        scr_InputManager.cannotMove = false;
    }

    private void SetGrid()
    {
        grid = new Tile[GridGenerator.columnSizeMax, GridGenerator.rowSizeMax];
        //This is assuming grid will be generated based off of the column and row size maxes
        for (int x = 0; x < GridGenerator.columnSizeMax; x++)
        {
            for(int y = 0; y < GridGenerator.rowSizeMax; y++)
            {
                grid[x, y] = GridGenerator.GetGrid()[x, y];
            }
        }
    }

    private void SetEntities()
    {
        activeEntities = new Entity[GridGenerator.GetStartingEntities().Length];
        for (int i = 0; i < GridGenerator.GetStartingEntities().Length; i++)
        {
            activeEntities[i] = GridGenerator.GetStartingEntities()[i];
        }
    }

    public bool CheckIfOccupied(int x, int y)
    {
        return grid[x, y].occupied;
    }
    public bool CheckIfActive(int x, int y, ActiveAttack _activeAttack)
    {

        return grid[x, y].isActive;
    }

    public void PrimeNextTile(int x , int y)
    {
        if(LocationOnGrid(x , y ))
            grid[x, y].Prime(); 
    }

    /// <summary>
    /// Indicates that the tile is currently being attacked/affected
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void ActivateTile(int x, int y)
    {
        if (LocationOnGrid(x, y))
        {
            grid[x, y].Activate();
            
        }
    }
    public void ActivateTile(int x, int y, ActiveAttack activeAttack)
    {
        if (LocationOnGrid(x, y))
        {
            grid[x, y].Activate(activeAttack);

        }
    }

    /// <summary>
    /// Use this if you want to activate a tile for a set period of time, then have it deactivate.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="time"></param>
    public void BriefActivateTile(int x, int y, float time)
    {
        StartCoroutine(BriefActivateTile_Coroutine(x, y, time));
    }

    private IEnumerator BriefActivateTile_Coroutine(int x, int y, float time)
    {
        if (LocationOnGrid(x, y))
        {
            grid[x, y].Activate();
            yield return new WaitForSeconds(time);
            grid[x, y].Deactivate();
        }
    }

    public bool LocationOnGrid(int x, int y)
    {
        if(x >= 0 && grid.GetLength(0) > x && grid.GetLength(1) > y && y >=0)
        {
            return true;
        }
        return false;
    }
    
    public void DeactivateTile(int x, int y)
    {
        if (LocationOnGrid(x, y))
            grid[x, y].Deactivate();

    }
    public void DePrimeTile(int x, int y)
    {
        if (LocationOnGrid(x, y))
            grid[x, y].DePrime();
    }

    public void SetTileOccupied(bool isOccupied, int x, int y, Entity ent)
    {
        if (LocationOnGrid(x, y))
        {
            grid[x, y].occupied = isOccupied;
            if (isOccupied) grid[x, y].entityOnTile = ent;
            else grid[x, y].entityOnTile = null;
        }
    }

    public void SetTileTerritory(int x, int y, TerrName newName, Color newColor)
    {
        grid[x, y].SetTerritory(newName, newColor);
   
    }

    public Territory ReturnTerritory(int x, int y)
    {
        return grid[x, y].territory;
    }
    
    /// <summary>
    /// Check if an active entity is at the attack's position to be hit by the attack. Change the attack based on the results and return it.
    /// </summary>
    /// <param name="attack"></param>
    /// <returns></returns>
    public ActiveAttack AttackPosition(ActiveAttack attack)
    {
        for(int i=0; i < activeEntities.Length; i++)
        {
            if (activeEntities[i].gameObject.activeSelf)
            {
                if (activeEntities[i]._gridPos == attack.position) 
                {
                    if (activeEntities[i].type != attack.entity.type)
                    {
                        Debug.Log("ACTIVE ENTITY HIT!");
                        //Check if entity is invincible and assigns iframes accordingly
                        if (!activeEntities[i].isInvincible())
                        {
                            activeEntities[i].HitByAttack(attack.attack);
                            if (activeEntities[i].has_iframes)
                            {
                                //Activate invincibility frames
                                activeEntities[i].setInvincible(true, activeEntities[i].invulnTime);
                            }
                        }
                        attack.entityIsHit = true;
                        attack.entityHit = activeEntities[i];
                        attack.attack.ImpactEffects();
                    }
                }
            }
        }
        return attack; 
    }

    /// <summary>
    /// Returns the entity at this postion on the grid or null if there is none. Also returns null if the coordinates given are not on the grid.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Entity GetEntityAtPosition(int x, int y)
    {
        if (LocationOnGrid(x, y))
        {
            return grid[x,y].entityOnTile;
        }
        return null;
    }

    /// <summary>
    /// Returns true if the coordinates are on the grid and unoccupied, false otherwise.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool IsTileUnoccupied(int x, int y)
    {
        return LocationOnGrid(x, y) && !grid[x,y].occupied;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Vector3 GetWorldLocation(int x, int y)
    {
        if (LocationOnGrid(x, y))
            return new Vector3(grid[x, y].transform.position.x, grid[x, y].transform.position.y, 0);

        else
            return new Vector3(-100,-100,-100); // will def be off the grid 

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Vector3 GetWorldLocation(Vector2Int pos)
    {
        if (LocationOnGrid(pos.x,pos.y))
            return new Vector3(grid[pos.x, pos.y].transform.position.x, grid[pos.x, pos.y].transform.position.y, 0);

        else
            return new Vector3(-100, -100, -100); // will def be off the grid 

    }

    public void RemoveEntity(Entity entity)
    {
        float tempID = entity.gameObject.GetInstanceID();
        for (int i = 0; i < activeEntities.Length; i++){
            if (activeEntities[i].gameObject.GetInstanceID() == tempID)
            {
                Debug.Log("help me");
                Entity[] temporaryEntities = new Entity[activeEntities.Length - 1];
                for(int j = 0; j < activeEntities.Length; j++)
                {
                    if (j >= i)
                    {
                        temporaryEntities[j] = activeEntities[j + 1];
                        
                    }
                    else if(j < i)
                    {
                        temporaryEntities[j] = activeEntities[j];
                    }
                }
                Debug.Log(temporaryEntities);
                activeEntities = temporaryEntities;
                Destroy(entity.gameObject); 

            }
            else
            {
                return;
            }
        }
        
    }

    //Seize Domain Card
    public void seizeDomain(float duration)
    {
        bool colFound = false;
        //Debug.Log("SEIZE!");
        for (int i = 0; i < columnSizeMax; i++)
        {

            for (int j = 0; j < rowSizeMax; j++)
            {
                //Debug.Log(scr_Grid.GridController.grid[i, j].territory.name);
                if (grid[i, j].territory.name != TerrName.Player)
                {
                    //Debug.Log("Column: " + i);
                    colFound = true;
                 
                    if (!grid[i, j].occupied)
                    {
                        //Debug.Log("SEIZING!");
                        SetTileTerritory(i, j, TerrName.Player, TileDictionary.colorDict[TerrName.Player]);
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
        for (int i = columnSizeMax - 1; i >= 0; i--)
        {

            for (int j = 0; j < rowSizeMax; j++)
            {
                //Debug.Log(scr_Grid.GridController.grid[i, j].territory.name);
                if (grid[i, j].territory.name != TerrName.Enemy)
                {
                    //Debug.Log("Column: " + i);
                    colFound = true;

                    if (!grid[i, j].occupied)
                    {
                        //Debug.Log("SEIZING!");
                        SetTileTerritory(i, j, TerrName.Enemy, TileDictionary.colorDict[TerrName.Enemy]);
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

}
