using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_RockBehavior : scr_EntityAI
{
    public int collisionDamage = 10;
    private void Start()
    {
        scr_Grid.GridController.SetTileOccupied(true, entity._gridPos.x, entity._gridPos.y, this.entity);
    }

    public override void Move()
    {
        
    }

    public override void UpdateAI()
    {
        
    }

    

    public override void Die()
    {
        entity.Death();
    }

}
