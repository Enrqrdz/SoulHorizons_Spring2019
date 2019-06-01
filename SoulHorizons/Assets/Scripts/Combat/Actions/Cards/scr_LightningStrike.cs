using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Lightning Strike")]
[RequireComponent(typeof(AudioSource))]
public class scr_LightningStrike : ActionData
{
    public AttackData lightningStrikeAttack;
    public int range;
    private AudioSource PlayCardSFX;
    //private AttackData HydroBurst;

    private Entity player;
    private int playerX, playerY;
    private int impactX;

    public override void Project()
    {
        if (player == null)
        {
            player = ObjectReference.Instance.PlayerEntity;
        }

        playerX = player._gridPos.x;
        playerY = player._gridPos.y;

        impactX = Mathf.Min(playerX + range, scr_Grid.GridController.columnSizeMax);

        scr_Grid.GridController.grid[impactX, playerY].Highlight();
    }

    public override void DeProject()
    {
        scr_Grid.GridController.grid[impactX, playerY].DeHighlight();
    }

    public override void Activate()
    {
        if (player == null)
        {
            player = ObjectReference.Instance.PlayerEntity;
        }

        playerX = player._gridPos.x;
        playerY = player._gridPos.y;

        impactX = Mathf.Min(playerX + range, scr_Grid.GridController.columnSizeMax);

        AttackController.Instance.AddNewAttack(lightningStrikeAttack, impactX, playerY, player);

        for (int i = 0; i < scr_Grid.GridController.activeEntities.Length; i++)
        {
            Entity target = scr_Grid.GridController.activeEntities[i];
            if(target != null)
            {
                if (target.isDampened && target._gridPos != new Vector2Int(impactX, playerY))
                {
                    AttackController.Instance.AddNewAttack(lightningStrikeAttack, target._gridPos.x, target._gridPos.y, player);
                }
            }
        }

        
    }
}
