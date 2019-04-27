using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/SoulBlight")]
[RequireComponent(typeof(AudioSource))]

public class Scr_SoulBlight : ActionData 
{
    private AudioSource PlayCardSFX;
    public AudioClip BlightSFX;
    public AttackData blightAttack;
    private Entity player;
    private int playerX, playerY;
    private int impactX, impactY;
    private bool northTileExists, southTileExists, westTileExists, eastTileExists;

    public override void Activate()
    {

        PlayCardSFX = ObjectReference.Instance.ActionManager;
        PlayCardSFX.clip = BlightSFX;
        PlayCardSFX.Play();

        //add attack to attack controller script
        //does a check to see if the target col is off the map

        AttackController.Instance.AddNewAttack(blightAttack, playerX, playerY, player);

    }

    public override void Project()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
        }

        playerX = player._gridPos.x;
        playerY = player._gridPos.y;

        impactX = Mathf.Min((playerX + blightAttack.maxIncrementRange), (scr_Grid.GridController.columnSizeMax - 1));

        for (int i = playerX + 1; i < impactX; i++)
        {
            if(scr_Grid.GridController.CheckIfOccupied(i, playerY))
            {
                impactX = i;
                break;
            }
        }

        impactY = playerY;

        northTileExists = impactY != scr_Grid.GridController.rowSizeMax - 1;
        southTileExists = impactY != 0;
        westTileExists = impactX != DomainManager.Instance.columnToBeSeized;
        eastTileExists = impactX != scr_Grid.GridController.columnSizeMax - 1;
        
        scr_Grid.GridController.grid[impactX, impactY].Highlight();

        if (northTileExists)
        {
            scr_Grid.GridController.grid[impactX, impactY + 1].Highlight();
        }
        if (southTileExists)
        {
            scr_Grid.GridController.grid[impactX, impactY - 1].Highlight();
        }
        if (westTileExists)
        {
            scr_Grid.GridController.grid[impactX - 1, impactY].Highlight();
        }
        if (eastTileExists)
        {
            scr_Grid.GridController.grid[impactX + 1, impactY].Highlight();
        }
    }

    public override void DeProject()
    {
        scr_Grid.GridController.grid[impactX, impactY].DeHighlight();

        if (northTileExists)
        {
            scr_Grid.GridController.grid[impactX, impactY + 1].DeHighlight();
        }
        if (southTileExists)
        {
            scr_Grid.GridController.grid[impactX, impactY - 1].DeHighlight();
        }
        if (westTileExists)
        {
            scr_Grid.GridController.grid[impactX - 1, impactY].DeHighlight();
        }
        if (eastTileExists)
        {
            scr_Grid.GridController.grid[impactX + 1, impactY].DeHighlight();
        }
    }
}
