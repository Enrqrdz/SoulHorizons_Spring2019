using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/SoulBlight")]
public class Atk_SoulBlight : AttackData
{
    public float damageRate = 1f;
    public float blightDuration = 6f;
    public int blightMainDamage = 6;
    public int blightSideDamage = 4;

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
        int impactX = activeAttack.position.x - 1;
        int impactY = activeAttack.position.y;

        bool northTileExsists = impactY != scr_Grid.GridController.rowSizeMax - 1;
        bool southTileExsists = impactY != 0;
        bool westTileExists = impactX != DomainManager.Instance.columnToBeSeized;
        bool eastTileExists = impactX != scr_Grid.GridController.columnSizeMax - 1;




        scr_Grid.GridController.grid[impactX, impactY].DeBuffTile(blightDuration, blightMainDamage, damageRate, 0);

        if (northTileExsists)
        {
            scr_Grid.GridController.grid[impactX, impactY + 1].DeBuffTile(blightDuration, blightMainDamage, damageRate, 0);
        }
        if (southTileExsists)
        {
            scr_Grid.GridController.grid[impactX, impactY - 1].DeBuffTile(blightDuration, blightMainDamage, damageRate, 0);
        }
        if (westTileExists)
        {
            scr_Grid.GridController.grid[impactX - 1, impactY].DeBuffTile(blightDuration, blightMainDamage, damageRate, 0);
        }
        if (eastTileExists)
        {
            scr_Grid.GridController.grid[impactX + 1, impactY].DeBuffTile(blightDuration, blightMainDamage, damageRate, 0);
        }
    }


}
