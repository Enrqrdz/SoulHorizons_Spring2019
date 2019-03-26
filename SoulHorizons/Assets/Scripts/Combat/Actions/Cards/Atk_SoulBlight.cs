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
        Vector2 affectedTileCenter = new Vector2(activeAttack.position.x - 1, activeAttack.position.y);
        Vector2 affectedTileNorth = new Vector2(affectedTileCenter.x, affectedTileCenter.y + 1);
        Vector2 affectedTileSouth = new Vector2(affectedTileCenter.x, affectedTileCenter.y - 1);
        Vector2 affectedTileEast = new Vector2(affectedTileCenter.x + 1, affectedTileCenter.y);
        Vector2 affectedTileWest = new Vector2(affectedTileCenter.x - 1, affectedTileCenter.y);

        Debug.Log(affectedTileCenter.x + ", " + affectedTileCenter.y);

        scr_Grid.GridController.grid[(int)affectedTileCenter.x, (int)affectedTileCenter.y].DeBuffTile(duration, blightMainDamage, damageRate);

        try
        {
            if (scr_Grid.GridController.ReturnTerritory((int)affectedTileNorth.x, (int)affectedTileNorth.y).name == TerrName.Enemy)
            {
                Debug.Log("North");
                scr_Grid.GridController.grid[(int)affectedTileNorth.x, (int)affectedTileNorth.y].DeBuffTile(duration, blightSideDamage, damageRate);
            }
        }
        catch
        { }
        try
        {
            if (scr_Grid.GridController.ReturnTerritory((int)affectedTileSouth.x, (int)affectedTileSouth.y).name == TerrName.Enemy)
            {
                Debug.Log("South");
                scr_Grid.GridController.grid[(int)affectedTileSouth.x, (int)affectedTileSouth.y].DeBuffTile(duration, blightSideDamage, damageRate);
            }
        }
        catch
        { }
        try
        {
            Debug.Log("East");
            scr_Grid.GridController.grid[(int)affectedTileEast.x, (int)affectedTileEast.y].DeBuffTile(duration, blightSideDamage, damageRate);
        }
        catch
        { }
        try
        {            
            if (scr_Grid.GridController.ReturnTerritory((int)affectedTileWest.x, (int)affectedTileWest.y).name == TerrName.Enemy)
            {
                Debug.Log("West");
                scr_Grid.GridController.grid[(int)affectedTileWest.x, (int)affectedTileWest.y].DeBuffTile(duration, blightSideDamage, damageRate);
            }
        }
        catch
        {}     
    }


}
