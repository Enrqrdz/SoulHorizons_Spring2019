using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/ChargeBeam")]
[RequireComponent(typeof(AudioSource))]

public class atk_ChargeBeam : AttackData
{
    Entity player;
    public float stunTime = 4f;
    public int BeamDamage = 10;
    public float BeamDamageDuration = 2f;
    public int BeamDamageRate = 1;
    int size = 0;

    public override Vector2Int ProgressAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        player.isStunned = true;
        try
        {
            scr_Grid.GridController.ActivateTile(xPos, yPos);
            if (scr_Grid.GridController.ReturnTerritory(xPos, yPos).name == TerrName.Enemy)
            {
                scr_Grid.GridController.grid[xPos, yPos].DeBuffTile(BeamDamage, BeamDamage, BeamDamageRate, 0);
            }
            return new Vector2Int(xPos + 1, yPos);
        }
        catch
        {
            scr_Grid.GridController.grid[xPos, yPos].DeBuffTile(BeamDamage, BeamDamage, BeamDamageRate, 0);
            return new Vector2Int(xPos + 1, yPos);
        }
    }

    //--Effects Methods--
    public override void LaunchEffects(ActiveAttack activeAttack)
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
        player.StartCoroutine(player.gotStunned(stunTime / 2));
        activeAttack.particle = Instantiate(particles, scr_Grid.GridController.GetWorldLocation(activeAttack.position.x, activeAttack.position.y) + particlesOffset, Quaternion.identity);

    }

    public override void ProgressEffects(ActiveAttack activeAttack)
    {
        
    }

    public override void ImpactEffects(int xPos = -1, int yPos = -1)
    {

    }

    public override void EndEffects(ActiveAttack activeAttack)
    {
        player.isStunned = false;
    }
}
