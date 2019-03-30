﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Grid : MonoBehaviour{
    
    public int columnSizeMax;
    public int rowSizeMax;
    [Tooltip("Use to move the center of the grid along the x axis")]
    public float columnOffset = 0;
    [Tooltip("Use to move the center of the grid along the y axis")]
    public float rowOffset = 0; 
    public Vector2 tileSpacing;
    public scr_Tile[,] grid;
    public scr_Tile tile;
    private SpriteRenderer spriteR;
    public Sprite tile_sprites;
    private int spriteTracker = 0;
    public Entity[] activeEntities;
    public Transform camera; 

    public static scr_Grid GridController;

    public EncounterData encounter;

    private void Awake()
    {
        GridController = this;     
    }


    private void Start()
    {
        encounter = SaveManager.currentGame.GetCurrentEncounterData();

        InitEncounter(); 
    }

    //Build Grid Tiles
    private void BuildGrid()
    {

        //tile_sprites = Resources.LoadAll<Sprite>("tiles_spritesheet");
        grid = new scr_Tile[columnSizeMax, rowSizeMax];
        Vector2 gridCenter = new Vector2((tileSpacing.x * (columnSizeMax-1) / 2), (tileSpacing.y * rowSizeMax / 2));
        camera.transform.position = new Vector3(gridCenter.x,gridCenter.y,camera.transform.position.z); 
        for (int j = 0; j < rowSizeMax; j++)
        {
            for (int i = 0; i < columnSizeMax; i++)
            {
                scr_Tile tileToAdd = null; 

                tileToAdd = (scr_Tile)Instantiate(tile, new Vector3((i * tileSpacing.x) + columnOffset, (j * tileSpacing.y) + rowOffset, 0), Quaternion.identity);

                tileToAdd.territory = encounter.GetTerrorityAtXAndY(i, j);
                tileToAdd.gridPositionX = i;
                tileToAdd.gridPositionY = j;

                spriteR = tileToAdd.GetComponent<SpriteRenderer>();
                spriteR.sprite = tile_sprites;
                
                if (tile_sprites == null) Debug.Log("MISSING SPRITE");

                grid[i, j] = tileToAdd;

                spriteTracker++;
            }
        }
        spriteTracker = 0;

    }

    public bool CheckIfOccupied(int x, int y)
    {
        return grid[x, y].occupied;
    }
    public bool CheckIfActive(int x, int y, ActiveAttack _activeAttack)
    {

        return grid[x, y].isActive; 
    }

    public bool CheckIfHarmful(int x, int y)
    {
        return grid[x, y].harmful;
    }

    public bool CheckIfHelpful(int x, int y)
    {
        try
        {
            return grid[x, y].helpful;
        }
        catch
        {
            return false;
        }
    }

    public float GetTileBuff (int x, int y)
    {
        return grid[x, y].GetTileBuff(); 
    }

    public float GetTileDamage(int x, int y)
    {
        return grid[x, y].GetTileDamage();
    }

    public void SetNewGrid(int new_xSizeMax, int new_ySizeMax)
    {
        columnSizeMax = new_xSizeMax;
        rowSizeMax = new_ySizeMax;
        BuildGrid(); 

    }

    //BUG - AT START TILES DON'T COUNT AS OCCUPIED, AFTER INIT SET TILES TO OCCUPIED FOR INITIALIZED ENTITIES
    public void InitEncounter()
    {
        //Set movement to true
        InputManager.cannotInputAnything = false;
        columnSizeMax = encounter.GetNumberOfColumns();
        rowSizeMax = encounter.GetNumberOfRows();
        //calling in awake as a debug, should be called in Encounter
        SetNewGrid(columnSizeMax, rowSizeMax);
        activeEntities = new Entity[encounter.entities.Length]; 
        for(int x = 0; x < activeEntities.Length; x++)
        {
            Entity _entity = new Entity();
            _entity = (Entity)Instantiate(encounter.entities[x].entity, Vector3.zero, Quaternion.identity);
            _entity.InitPosition(encounter.entities[x].x, encounter.entities[x].y);
            activeEntities[x] = _entity;
        }
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log("CENTER: " + grid[0, 0].transform.position.x);
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

}
