using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Cards/Cleave")]
[RequireComponent(typeof(AudioSource))]

public class scr_Cleave : ActionData
{
	public AttackData attack;
    private AudioSource PlayCardSFX;
    public AudioClip CleaveSFX;
    private Entity player;
    private int playerX, playerY;

    public override void Project()
    {
        if(player == null)
        {
            player = ObjectReference.Instance.PlayerEntity;
        }
        playerX = player._gridPos.x;
        playerY = player._gridPos.y;

        if (playerY == 0)
        {
            for (int i = 0; i <= attack.maxIncrementRange; i++)
            {
                attack.maxIncrementRange = 1;
                scr_Grid.GridController.grid[playerX + 1, playerY + i].Highlight();
            }

        }
        else if (playerY == scr_Grid.GridController.rowSizeMax - 1)
        {
            for (int i = 0; i <= attack.maxIncrementRange; i++)
            {
                attack.maxIncrementRange = 1;
                scr_Grid.GridController.grid[playerX + 1, playerY - 1 + i].Highlight();
            }
        }
        else
        {
            for (int i = 0; i <= attack.maxIncrementRange; i++)
            {
                attack.maxIncrementRange = 2;
                scr_Grid.GridController.grid[playerX + 1, playerY - 1 + i].Highlight();
            }
        }
    }

    public override void DeProject()
    {
        if (playerY == 0)
        {
            for (int i = 0; i <= attack.maxIncrementRange; i++)
            {
                scr_Grid.GridController.grid[playerX + 1, playerY + i].DeHighlight();
            }

        }
        else if (playerY == scr_Grid.GridController.rowSizeMax - 1)
        {
            for (int i = 0; i <= attack.maxIncrementRange; i++)
            {
                scr_Grid.GridController.grid[playerX + 1, playerY - 1 + i].DeHighlight();
            }
        }
        else
        {
            for (int i = 0; i <= attack.maxIncrementRange; i++)
            {
                scr_Grid.GridController.grid[playerX + 1, playerY - 1 + i].DeHighlight();
            }
        }
    }

    public override void Activate()
    {
        PlayCardSFX = ObjectReference.Instance.ActionManager;
        PlayCardSFX.clip = CleaveSFX;
        PlayCardSFX.Play();
        if (player == null)
        {
            player = ObjectReference.Instance.PlayerEntity;
        }
        playerX = player._gridPos.x;
        playerY = player._gridPos.y;


        if (playerY == 0)
		{
            AttackController.Instance.AddNewAttack(attack, playerX + 1, playerY, player);
		}
        else
        {
            AttackController.Instance.AddNewAttack(attack, playerX + 1, playerY - 1, player);
        }
    }
}
