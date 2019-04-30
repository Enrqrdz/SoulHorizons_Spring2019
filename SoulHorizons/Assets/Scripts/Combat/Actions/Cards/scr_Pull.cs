using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Cards/Pull")]
[RequireComponent(typeof(AudioSource))]

public class scr_Pull : ActionData
{
    private AudioSource PlayCardSFX;
    public AudioClip PullSFX;
    public AttackData pull;
    private Entity player;
    private Entity enemy;
    private int playerX, playerY;
    private Vector2Int pullPosition;

    public override void Activate()
    {
        PlayCardSFX = ObjectReference.Instance.ActionManager;
        PlayCardSFX.clip =PullSFX;
        PlayCardSFX.Play();
        player = ObjectReference.Instance.PlayerEntity;
        playerX = player._gridPos.x;
        playerY = player._gridPos.y;

        //add attack to attack controller script
        AttackController.Instance.AddNewAttack(pull, playerX, playerY, player);
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
                pullPosition = new Vector2Int(i, playerY);
                scr_Grid.GridController.grid[pullPosition.x, pullPosition.y].Highlight();
                scr_Grid.GridController.grid[DomainManager.Instance.columnToBeSeized, pullPosition.y].Highlight();
                break;
            }
        }
    }

    public override void DeProject()
    {
        for (int i = 0; i < scr_Grid.GridController.columnSizeMax; i++)
        {
            enemy = scr_Grid.GridController.GetEntityAtPosition(i, playerY);

            if (enemy != null && enemy.type == EntityType.Enemy)
            {
                pullPosition = new Vector2Int(i, playerY);
                scr_Grid.GridController.grid[pullPosition.x, pullPosition.y].DeHighlight();
                scr_Grid.GridController.grid[DomainManager.Instance.columnToBeSeized, pullPosition.y].DeHighlight();
                break;
            }
        }
    }
}
