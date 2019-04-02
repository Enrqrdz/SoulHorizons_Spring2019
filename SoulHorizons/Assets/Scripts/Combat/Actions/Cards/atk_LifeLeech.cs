using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/LifeLeech")]
[RequireComponent(typeof(AudioSource))]

public class atk_LifeLeech : AttackData
{
    Entity player;
    public float stunTime = 3f;
    public int leechDamage = 4;
    public int leechHeal = 4;
    public float leechRate = 1f;

    public override Vector2Int ProgressAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        scr_Grid.GridController.ActivateTile(xPos, yPos);
        return new Vector2Int(xPos + 1, yPos);
    }

    //--Effects Methods--
    public override void LaunchEffects(ActiveAttack activeAttack)
    {
        activeAttack.particle = Instantiate(particles, scr_Grid.GridController.GetWorldLocation(activeAttack.position.x, activeAttack.position.y) + particlesOffset, Quaternion.identity);
        activeAttack.particle.sortingOrder = -activeAttack.position.y;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
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
        if(activeAttack.entityIsHit == true)
        {
            activeAttack.entityHit.isStunned = true;
            player.isStunned = true;
            activeAttack.entityHit.TakeDamageOverTime(stunTime, leechRate, leechDamage);
            player.HealOverTime(stunTime, leechRate, leechHeal);
            activeAttack.entityHit.isStunned = false;
            player.isStunned = false;
        }
    }
}
