using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/SoulBlight")]
public class Atk_SoulBlight : AttackData
{
    public float damageRate = 1f;
    public float duration = 6f;
    public int blightMainDamage = 6;
    public int blightSideDamage = 4;
    int counter = 0;

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
        Debug.Log(activeAttack.position.x + " " + activeAttack.position.y);
        scr_Grid.GridController.grid[activeAttack.position.x, activeAttack.position.y].DeBuffTile(duration, blightMainDamage, damageRate);
        try
        {
            Debug.Log("1");
            scr_Grid.GridController.grid[activeAttack.position.x + 1, activeAttack.position.y].DeBuffTile(duration, blightSideDamage, damageRate);
        }
        catch
        { }
        try
        {
            
            if (scr_Grid.GridController.ReturnTerritory(activeAttack.position.x - 1, activeAttack.position.y).name == TerrName.Enemy)
            {
                Debug.Log("2");
                scr_Grid.GridController.grid[activeAttack.position.x - 1, activeAttack.position.y].DeBuffTile(duration, blightSideDamage, damageRate);
            }
        }
        catch
        {
            //not on the grid
        }
        try
        {
            
            if (scr_Grid.GridController.ReturnTerritory(activeAttack.position.x, activeAttack.position.y - 1).name == TerrName.Enemy)
            {
                Debug.Log("3");
                scr_Grid.GridController.grid[activeAttack.position.x, activeAttack.position.y - 1].DeBuffTile(duration, blightSideDamage, damageRate);
            }
        }
        catch
        { }
        try
        {
            
            if (scr_Grid.GridController.ReturnTerritory(activeAttack.position.x, activeAttack.position.y + 1).name == TerrName.Enemy)
            {
                Debug.Log("4");
                scr_Grid.GridController.grid[activeAttack.position.x , activeAttack.position.y + 1].DeBuffTile(duration, blightSideDamage, damageRate);
            }
        }
        catch
        { }
        
    }


}
