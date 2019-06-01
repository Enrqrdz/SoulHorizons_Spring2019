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
            Entity enemyHit = activeAttack.entityHit;
            int entityXPos = enemyHit._gridPos.x;
            int entityYPos = enemyHit._gridPos.y;

            entityXPos = DomainManager.Instance.columnToBeSeized;
            if (!scr_Grid.GridController.CheckIfOccupied(entityXPos, entityYPos))
            {
                enemyHit.SetTransform(entityXPos, entityYPos);
                enemyHit.GotStunned(stunTime);
            }
            else
            {
                enemyHit.GotStunned(stunTime);
                return;
            }

        }
    }

}
