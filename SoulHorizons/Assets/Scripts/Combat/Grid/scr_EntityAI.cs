using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class scr_EntityAI : MonoBehaviour {

    public Entity entity;
    public Animator anim;

    public abstract void Move();

    public abstract void UpdateAI();

    public abstract void Die();

    public void PrimeAttackTiles (AttackData attack, int xPos, int yPos)
    {
        for (int i = 0; i < attack.maxIncrementRange; i++)
        {
            if (scr_Grid.GridController.GetEntityAtPosition(xPos - 1 - i, yPos) == null || (scr_Grid.GridController.GetEntityAtPosition(xPos - 1 - i, yPos).type == EntityType.Player))
                scr_Grid.GridController.PrimeNextTile(xPos - 1 - i, yPos);
            else
            {
                break;
            }
        }
    }
}
