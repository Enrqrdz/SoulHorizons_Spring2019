using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Bosses/Raitori/GustGale")]
public class atk_GustGale : AttackData
{
    public override Vector2Int BeginAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        return new Vector2Int(xPos, yPos);
    }

    public override Vector2Int ProgressAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        return LinearForward_ProgressAttack(xPos, yPos, activeAtk);
    }

    Vector2Int LinearForward_ProgressAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        scr_Grid.GridController.ActivateTile(xPos, yPos);

        if(yPos - 1 > 0)
        {
            return new Vector2Int(xPos, yPos - 1);
        }

        return new Vector2Int(xPos, yPos);
    }
    public override bool CheckCondition(Entity _ent)
    {
        return true;
    }

    //--Effects Methods--
    public override void LaunchEffects(ActiveAttack activeAttack)
    {
        activeAttack.particle = Instantiate(particles, scr_Grid.GridController.GetWorldLocation(activeAttack.position.x, activeAttack.position.y) + particlesOffset, Quaternion.identity);
        activeAttack.particle.sortingOrder = -activeAttack.position.y;
    }

    public override void ProgressEffects(ActiveAttack activeAttack)
    {
        activeAttack.particle.transform.position = Vector3.Lerp(activeAttack.particle.transform.position, scr_Grid.GridController.GetWorldLocation(activeAttack.lastPosition.x, activeAttack.lastPosition.y) + activeAttack.attack.particlesOffset, (4.5f) * Time.deltaTime);
        activeAttack.particle.transform.Rotate(Vector3.up * 60f);
    }

    public override void ImpactEffects(int xPos = -1, int yPos = -1)
    {
        for (int i = 0; i < scr_Grid.GridController.activeEntities.Length; i++)
        {
            if (scr_Grid.GridController.activeEntities[i].type == EntityType.Player)
            {
                scr_Grid.GridController.activeEntities[i].isStunned = true;
                scr_Grid.GridController.activeEntities[i].spr.color = Color.gray;
                break;
            }
        }
    }

    public override void EndEffects(ActiveAttack activeAttack)
    {

    }
}
