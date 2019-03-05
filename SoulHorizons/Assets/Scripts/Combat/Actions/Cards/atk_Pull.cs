using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Pull")]
public class atk_Pull : AttackData
{
    public float stunTime = .5f;
    public override Vector2Int ProgressAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        return new Vector2Int(xPos + 1, yPos);
    }

    //--Effects Methods--
    public override void LaunchEffects(ActiveAttack activeAttack)
    {
        activeAttack.particle = Instantiate(particles, scr_Grid.GridController.GetWorldLocation(activeAttack.position.x, activeAttack.position.y) + particlesOffset, Quaternion.identity);
        activeAttack.particle.sortingOrder = -activeAttack.position.y;
    }

    public override void ProgressEffects(ActiveAttack activeAttack)
    {
        activeAttack.particle.transform.position = Vector3.Lerp(activeAttack.particle.transform.position, scr_Grid.GridController.GetWorldLocation(activeAttack.lastPosition.x, activeAttack.lastPosition.y) + activeAttack.attack.particlesOffset, (particleSpeed) * Time.deltaTime);
    }

    public override void ImpactEffects(int xPos = -1, int yPos = -1)
    {

    }

    public override void EndEffects(ActiveAttack activeAttack)
    {
        if (activeAttack.entityIsHit == true)
        {
            int entityXPos = activeAttack.entityHit._gridPos.x;
            int entityYPos = activeAttack.entityHit._gridPos.y;
            int xLimit = GetXLimit(entityXPos, activeAttack.entityHit);
            Debug.Log(entityXPos);
            if ((entityXPos - 1) != xLimit)
            {
                entityXPos = activeAttack.entityHit._gridPos.x--;
                if (!scr_Grid.GridController.CheckIfOccupied(entityXPos, entityYPos) && (scr_Grid.GridController.ReturnTerritory(entityXPos, entityYPos).name != TerrName.Player))
                {
                    //if the tile is not occupied
                    scr_Grid.GridController.SetTileOccupied(true, entityXPos, entityYPos, activeAttack.entityHit);          //set it to be occupied  
                    activeAttack.entityHit.SetTransform(entityXPos, entityYPos);
                    activeAttack.entityHit.gotStunned(stunTime);
                    return;
                }
            }
            else
            {
                activeAttack.entityHit.gotStunned(stunTime);
                return;
            }
        }
    }

    int GetXLimit(int xPos, Entity entity)
    {
        int xRange = scr_Grid.GridController.columnSizeMax-1;
        int xLimit = xPos;
        int tempX = xPos;
        for (int i = 0; i < xRange; i++)
        {
            tempX--;
            if (scr_Grid.GridController.grid[xLimit, entity._gridPos.y].territory.name != TerrName.Player)
            {
                xLimit = tempX;
            }
            else
            {
                return xLimit;
            }
        }
        return xLimit;
    }
}
