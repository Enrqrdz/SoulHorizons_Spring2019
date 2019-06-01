using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Biting Wind")]
public class atk_BitingWind : AttackData
{
    public float stunDuration;
    public float vulnerabilityDuration;
    public float vulnerabilityMultiplier = 2f;
    private Vector2Int gridPosition;
    private Entity entityHit;

    public override void LaunchEffects(ActiveAttack activeAttack)
    {
        activeAttack.particle = Instantiate(particles, scr_Grid.GridController.GetWorldLocation(activeAttack.position.x, activeAttack.position.y) + particlesOffset, Quaternion.identity);
        activeAttack.particle.sortingOrder = -activeAttack.position.y;
    }

    public override Vector2Int ProgressAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        return new Vector2Int(xPos + 1, yPos);
    }

    public override void ProgressEffects(ActiveAttack activeAttack)
    {
        gridPosition = activeAttack.position;
        activeAttack.particle.transform.position = Vector3.Lerp(activeAttack.particle.transform.position, scr_Grid.GridController.GetWorldLocation(activeAttack.lastPosition.x, activeAttack.lastPosition.y) + activeAttack.attack.particlesOffset, (particleSpeed) * Time.deltaTime);
    }

    public override void ImpactEffects(int xPos = -1, int yPos = -1)
    {
        entityHit = (scr_Grid.GridController.GetEntityAtPosition(gridPosition.x, gridPosition.y));

        if(entityHit != null && entityHit._health.hp > 0)
        {
            entityHit.Weaken(vulnerabilityMultiplier, vulnerabilityDuration);
            Debug.Log("Target is vulnerable!");
            if (entityHit.isDampened)
            {
                entityHit.GotStunned(stunDuration);
                Debug.Log("Target is stunned!");
            }
            
        }

    }

    public override void EndEffects(ActiveAttack activeAttack)
    {
       
    }
}
