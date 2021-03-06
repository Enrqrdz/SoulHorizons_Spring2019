﻿using UnityEngine;

[CreateAssetMenu(menuName = "Cards/GuardBreak")]
[RequireComponent(typeof(AudioSource))]

public class scr_GuardBreak : ActionData
{
    private Entity player;
    private Entity enemy;
    private AudioSource PlayCardSFX;
    public AudioClip BreakSFX;
    public AttackData BreakAttack;
    public int GuardbreakDamage = 5;
    public float teleportTime = 0.5f;
    int playerX;
    int playerY;
    Vector2Int teleportPosition;

    public override void Activate()
    {

        PlayCardSFX = ObjectReference.Instance.ActionManager;
        PlayCardSFX.clip = BreakSFX;
        PlayCardSFX.Play();

        player = ObjectReference.Instance.PlayerEntity;

        playerX = player._gridPos.x;
        playerY = player._gridPos.y;

        for (int i = 0; i < scr_Grid.GridController.columnSizeMax; i++)
        {
            enemy = scr_Grid.GridController.GetEntityAtPosition(i, playerY);

            if (enemy != null && enemy.type == EntityType.Enemy)
            {
                player.SetTransform(enemy._gridPos.x - 1, enemy._gridPos.y);
                player.StartCoroutine(player.Teleport(teleportTime, GuardbreakDamage, playerX, playerY, enemy));
                break;
            }
        }
        //AttackController.Instance.AddNewAttack(BreakAttack, player._gridPos.x + 1, player._gridPos.y, player);
        //scr_Grid.GridController.activeEntities[i].type == EntityType.Enemy
    }

    public override void Project()
    {
        player = ObjectReference.Instance.PlayerEntity;

        playerX = player._gridPos.x;
        playerY = player._gridPos.y;

        for (int i = 0; i < scr_Grid.GridController.columnSizeMax; i++)
        {
            enemy = scr_Grid.GridController.GetEntityAtPosition(i, playerY);

            if (enemy != null && enemy.type == EntityType.Enemy)
            {
                teleportPosition = new Vector2Int(i - 1, playerY);
                scr_Grid.GridController.grid[teleportPosition.x, teleportPosition.y].Highlight();
                break;
            }
        }
    }

    public override void DeProject()
    {
        if(teleportPosition != null)
        {
            scr_Grid.GridController.grid[teleportPosition.x, teleportPosition.y].DeHighlight();
        }
    }
}
