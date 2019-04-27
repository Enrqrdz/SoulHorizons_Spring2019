using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

[CreateAssetMenu(menuName = "Cards/CrescentStrike")]
public class scr_CrescentStrike : ActionData
{
    private AudioSource PlayCardSFX;
    public AudioClip CrescentSFX;
    public AttackData crescentAttack;
    private Entity player;
    private int playerX, playerY;
    private int verticalRange = 3, horizontalRange = 2;

    public override void Activate()
    {
        
        PlayCardSFX = ObjectReference.Instance.ActionManager;
        PlayCardSFX.clip = CrescentSFX;
        PlayCardSFX.Play();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
        playerX = player._gridPos.x;
        playerY = player._gridPos.y;

        //add attack to attack controller script
        //does a check to see if the target col is off the map
        if (player.GetComponent<Entity>()._gridPos.y == 0)
        {
            AttackController.Instance.AddNewAttack(crescentAttack, playerX + 1, playerY, player);
        }
        else
        {
            AttackController.Instance.AddNewAttack(crescentAttack, playerX + 1, playerY - 1, player);
        }

    }

    public override void Project()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
        playerX = player._gridPos.x;
        playerY = player._gridPos.y;

        if (playerY == 0)
        {
            scr_Grid.GridController.grid[playerX + 1, playerY + 0].Highlight();
            scr_Grid.GridController.grid[playerX + 2, playerY + 0].Highlight();
            scr_Grid.GridController.grid[playerX + 2, playerY + 1].Highlight();
            scr_Grid.GridController.grid[playerX + 1, playerY + 1].Highlight();
        }
        else if(playerY == scr_Grid.GridController.rowSizeMax - 1)
        {
            scr_Grid.GridController.grid[playerX + 1, playerY - 1].Highlight();
            scr_Grid.GridController.grid[playerX + 2, playerY - 1].Highlight();
            scr_Grid.GridController.grid[playerX + 2, playerY + 0].Highlight();
            scr_Grid.GridController.grid[playerX + 1, playerY + 0].Highlight();
        }
        else
        {
            scr_Grid.GridController.grid[playerX + 1, playerY - 1].Highlight();
            scr_Grid.GridController.grid[playerX + 2, playerY - 1].Highlight();
            scr_Grid.GridController.grid[playerX + 2, playerY + 0].Highlight();
            scr_Grid.GridController.grid[playerX + 2, playerY + 1].Highlight();
            scr_Grid.GridController.grid[playerX + 1, playerY + 1].Highlight();
        }
        
    }

    public override void DeProject()
    {
        if (playerY == 0)
        {
            scr_Grid.GridController.grid[playerX + 1, playerY + 0].DeHighlight();
            scr_Grid.GridController.grid[playerX + 2, playerY + 0].DeHighlight();
            scr_Grid.GridController.grid[playerX + 2, playerY + 1].DeHighlight();
            scr_Grid.GridController.grid[playerX + 1, playerY + 1].DeHighlight();
        }
        else if (playerY == scr_Grid.GridController.rowSizeMax - 1)
        {
            scr_Grid.GridController.grid[playerX + 1, playerY - 1].DeHighlight();
            scr_Grid.GridController.grid[playerX + 2, playerY - 1].DeHighlight();
            scr_Grid.GridController.grid[playerX + 2, playerY + 0].DeHighlight();
            scr_Grid.GridController.grid[playerX + 1, playerY + 0].DeHighlight();
        }
        else
        {
            scr_Grid.GridController.grid[playerX + 1, playerY - 1].DeHighlight();
            scr_Grid.GridController.grid[playerX + 2, playerY - 1].DeHighlight();
            scr_Grid.GridController.grid[playerX + 2, playerY + 0].DeHighlight();
            scr_Grid.GridController.grid[playerX + 2, playerY + 1].DeHighlight();
            scr_Grid.GridController.grid[playerX + 1, playerY + 1].DeHighlight();
        }
    }
}
