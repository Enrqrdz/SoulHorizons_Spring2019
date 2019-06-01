using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Tectonic Smash")]

public class atk_TectonicSmash : AttackData
{
    public float stunTime = .5f;
    public int rockCollisionDamage = 10;
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

            if (scr_Grid.GridController.LocationOnGrid(entityXPos + 1, entityYPos) && (scr_Grid.GridController.ReturnTerritory(entityXPos + 1, entityYPos).name != TerrName.Player))
            {
              
                if (!scr_Grid.GridController.CheckIfOccupied(entityXPos + 1, entityYPos) && activeAttack.entityHit.type == EntityType.Enemy)
                {
                    scr_Grid.GridController.SetTileOccupied(false, entityXPos, entityYPos, activeAttack.entityHit);
                    scr_Grid.GridController.SetTileOccupied(true, entityXPos++, entityYPos, activeAttack.entityHit);
                    activeAttack.entityHit.SetTransform(entityXPos++, entityYPos);
                    activeAttack.entityHit.GotStunned(stunTime);
                    Debug.Log(activeAttack.entityHit._gridPos.x + ", " + activeAttack.entityHit._gridPos.y);
                    return;
                }
                else if (scr_Grid.GridController.CheckIfOccupied(entityXPos + 1, entityYPos) && activeAttack.entityHit.type == EntityType.Obstacle)
                {
                    Debug.Log("Commin straight from the underground");
                    scr_Grid.GridController.SetTileOccupied(false, entityXPos, entityYPos, activeAttack.entityHit);
                    Entity hitByRock = scr_Grid.GridController.GetEntityAtPosition(entityXPos+1, entityYPos);
                    hitByRock._health.TakeDamage(rockCollisionDamage, hitByRock);
                    activeAttack.entityHit.Death();
                }
                else //if (!scr_Grid.GridController.CheckIfOccupied(entityXPos + 1, entityYPos) && activeAttack.entityHit.type == EntityType.Obstacle)
                {
                    activeAttack.entityHit.GotStunned(stunTime);
                    scr_Grid.GridController.SetTileOccupied(false, entityXPos, entityYPos, activeAttack.entityHit);
                    scr_Grid.GridController.SetTileOccupied(true, entityXPos++, entityYPos, activeAttack.entityHit);
                    activeAttack.entityHit.SetTransform(entityXPos++, entityYPos);
                }
            }
            else
            {
                activeAttack.entityHit.GotStunned(stunTime);
                scr_Grid.GridController.SetTileOccupied(false, entityXPos, entityYPos, activeAttack.entityHit);
                scr_Grid.GridController.SetTileOccupied(true, entityXPos++, entityYPos, activeAttack.entityHit);
                activeAttack.entityHit.SetTransform(entityXPos++, entityYPos);
            }
        }
    }
}
