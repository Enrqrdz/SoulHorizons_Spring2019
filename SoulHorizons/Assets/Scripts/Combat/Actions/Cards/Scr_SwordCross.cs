using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[CreateAssetMenu(menuName = "Cards/SwordCross")]

public class Scr_SwordCross : ActionData
{
    private AudioSource PlayCardSFX;
    public AudioClip SwordSFX;
    public AttackData swordAttack;
    private Entity player;
    private int playerX, playerY;
    private int impactX;

    public override void Project()
    {
        player = ObjectReference.Instance.PlayerEntity;
        playerX = player._gridPos.x;
        playerY = player._gridPos.y;

        impactX = Mathf.Min(playerX + 2, scr_Grid.GridController.columnSizeMax - 1);

        scr_Grid.GridController.grid[impactX, playerY].Highlight();

        if (playerY == 0)
        {
            scr_Grid.GridController.grid[impactX + 1, playerY + 1].Highlight();
            scr_Grid.GridController.grid[impactX - 1, playerY + 1].Highlight();
        }
        else if (playerY == scr_Grid.GridController.rowSizeMax - 1)
        {
            scr_Grid.GridController.grid[impactX + 1, playerY - 1].Highlight();
            scr_Grid.GridController.grid[impactX - 1, playerY - 1].Highlight();
        }
        else
        {
            scr_Grid.GridController.grid[impactX + 1, playerY + 1].Highlight();
            scr_Grid.GridController.grid[impactX - 1, playerY + 1].Highlight();
            scr_Grid.GridController.grid[impactX + 1, playerY - 1].Highlight();
            scr_Grid.GridController.grid[impactX - 1, playerY - 1].Highlight();
        }
    }

    public override void DeProject()
    {
        scr_Grid.GridController.grid[impactX, playerY].DeHighlight();

        if (playerY == 0)
        {
            scr_Grid.GridController.grid[impactX + 1, playerY + 1].DeHighlight();
            scr_Grid.GridController.grid[impactX - 1, playerY + 1].DeHighlight();
        }
        else if (playerY == scr_Grid.GridController.rowSizeMax - 1)
        {
            scr_Grid.GridController.grid[impactX + 1, playerY - 1].DeHighlight();
            scr_Grid.GridController.grid[impactX - 1, playerY - 1].DeHighlight();
        }
        else
        {
            scr_Grid.GridController.grid[impactX + 1, playerY + 1].DeHighlight();
            scr_Grid.GridController.grid[impactX - 1, playerY + 1].DeHighlight();
            scr_Grid.GridController.grid[impactX + 1, playerY - 1].DeHighlight();
            scr_Grid.GridController.grid[impactX - 1, playerY - 1].DeHighlight();
        }
    }

    public override void Activate()
    {
        PlayCardSFX = ObjectReference.Instance.ActionManager;
        PlayCardSFX.clip = SwordSFX;
        PlayCardSFX.Play();

        //add attack to attack controller script
        //does a check to see if the target col is off the map
        if (playerY == 0)
        {
            AttackController.Instance.AddNewAttack(swordAttack, impactX + 1, playerY + 1, player);
            AttackController.Instance.AddNewAttack(swordAttack, impactX - 1, playerY + 1, player);
        }
        else if (playerY == scr_Grid.GridController.rowSizeMax - 1)
        {
            AttackController.Instance.AddNewAttack(swordAttack, impactX + 1, playerY - 1, player);
            AttackController.Instance.AddNewAttack(swordAttack, impactX - 1, playerY - 1, player);
        }
        else
        {
            AttackController.Instance.AddNewAttack(swordAttack, impactX + 1, playerY + 1, player);
            AttackController.Instance.AddNewAttack(swordAttack, impactX + 1, playerY - 1, player);
            AttackController.Instance.AddNewAttack(swordAttack, impactX - 1, playerY + 1, player);
            AttackController.Instance.AddNewAttack(swordAttack, impactX - 1, playerY - 1, player);
        }
    }
}
