﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Water Prison")]
public class Atk_WaterPrison : AttackData
{
    public float floodedDuration = 4f;
    public float damageRate = 2;
    public int damageEachTick = 3;
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
        scr_Grid.GridController.grid[activeAttack.position.x - 1, activeAttack.position.y].DeBuffTile(floodedDuration, 0, 0, 2);
        if (activeAttack.entityIsHit == true)
        {
            activeAttack.entityHit.TakeDamageOverTime(floodedDuration, damageRate, damageEachTick);
            return;
        }

    }
}
