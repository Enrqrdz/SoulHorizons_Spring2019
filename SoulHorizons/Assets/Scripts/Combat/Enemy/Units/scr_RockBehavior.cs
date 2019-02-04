using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_RockBehavior : scr_EntityAI
{



    private void Start()
    {
        
    }

    public override void Move()
    {

    }
 
    public override void UpdateAI()
    {
        scr_Grid.GridController.SetTileOccupied(true, entity._gridPos.x, entity._gridPos.y, this.entity);


    }

    public override void Die()
    {
        entity.Death();
    }

}
