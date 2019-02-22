using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_RockBehavior : EntityAI
{



    private void Start()
    {
        
    }

    public override void Move()
    {

    }

    public override void UpdateAI()
    {
        Grid.Instance.SetTileOccupied(true, entity._gridPos.x, entity._gridPos.y, this.entity);
    }

    public override void Die()
    {
        entity.Death();
    }

}
