using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TerrName { Player, Enemy, Neutral, Blocked }

[System.Serializable]
public struct Territory
{
    public TerrName name;
    public Color TerrColor;
    public Sprite TerrSprite;

    public Territory(TerrName nam, Color col, Sprite spr)
    {
        name = nam;
        TerrColor = col;
        TerrSprite = spr;
    }

    public Territory(Territory territoryToCopy)
    {
        name = territoryToCopy.name;
        TerrColor = territoryToCopy.TerrColor;
        TerrSprite = territoryToCopy.TerrSprite;
    }
}
public class scr_Tile : MonoBehaviour{

    [Header("Combat Colors")]
    public Color primeColor;
    public Color activeColor;
    public Color playerActiveColor;
    public Color playerPrimeColor; 
    //public Color inactiveColor;

    
    public bool occupied;
    public bool harmful;
    public bool isPrimed;
    public bool isActive; 
    public Territory territory;
    GameObject gridController;
    scr_Grid grid;
    public int gridPositionX;
    public int gridPositionY;
    public int queuedAttacks = 0;
    public scr_Entity entityOnTile;
    

    Vector2 spriteSize = new Vector2 (1f,.85f);
    SpriteRenderer spriteRenderer;
     

    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        territory.TerrColor.a = 1f;
        spriteRenderer.color = territory.TerrColor;
        spriteRenderer.drawMode = SpriteDrawMode.Sliced;
        spriteRenderer.size = spriteSize;
        isPrimed = false;                                                       //Sets a tile to about to be hit (yellow)
        isActive = false;                                                       //Sets a tile to do hit          (red)
        harmful = false;                                                        //Sets tile to do persistent harm. May not be needed
        occupied = false;                                                       //Sets a tile to be occupied by an entity
        gridController = GameObject.FindGameObjectWithTag("GridController");    //Grid Controller
        grid = gridController.GetComponent<scr_Grid>();
        entityOnTile = null;
         
        
    }
    private void Update()
    {
        if (isActive && entityOnTile != null)
        {
            //Debug.Log("OW!!!!");
            //TEMPORARY HARDCODED VALUE, GET ATTACK ASSOCIATED WITH ACTIVATED TILE AND GET DAMAGE FROM THAT
            //entityOnTile._health.TakeDamage(1);
            isActive = false; //So it only hits once and not every frame, can change if it's multi hit, add that functionality later
        }

    }

    public void InitalizeTile()
    {


    }

    public void SetTerritory(TerrName newName, Color newColor)
    {
        territory.name = newName;
        territory.TerrColor = newColor;
        spriteRenderer.color = territory.TerrColor;
    }

    public void Prime()
    {
        isPrimed = true;
        if (!isActive)
        {   
            spriteRenderer.color = primeColor;
        }
        
    }
    public void DePrime()
    {
        isPrimed = false;
        if (!isPrimed)
            spriteRenderer.color = territory.TerrColor; 
    }
    public void Activate()
    {
        queuedAttacks++; 
        isPrimed = false; 
        isActive = true; 
        spriteRenderer.color = activeColor;
    }
    public void Activate(ActiveAttack activeAttack)
    {
        queuedAttacks++;
        isPrimed = false;
        isActive = true;
        if(activeAttack.entity.type == EntityType.Player)
        {
            spriteRenderer.color = playerActiveColor;
        }
        else if (activeAttack.entity.type == EntityType.Enemy)
        {
            spriteRenderer.color = activeColor;
        }
        
    }

    public void Deactivate()
    {
        queuedAttacks--; 
        if(queuedAttacks <= 0)
        {
            queuedAttacks = 0; 
            if (isPrimed)
            {
                Prime(); 
            }
            else
            {
                isActive = false;
                spriteRenderer.color = territory.TerrColor;
            }
            
        }
        
         
    }
   
}
