using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Rock Fall")]
public class atk_RockFall : AttackData
{
    public GameObject rock;

    int startX;
    int startY;
    public override Vector2Int ProgressAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        scr_Grid.GridController.ActivateTile(xPos, yPos);
        return new Vector2Int(xPos, yPos);
    }

    public override void LaunchEffects(ActiveAttack activeAttack)
    {
        activeAttack.particle = Instantiate(particles, scr_Grid.GridController.GetWorldLocation(activeAttack.position.x, activeAttack.position.y) + new Vector3(0, 1.5f, 0), Quaternion.identity);
        activeAttack.particle.sortingOrder = -activeAttack.position.y;
        startX = activeAttack.position.x;
        startY = activeAttack.position.y;

    }

    public override void ProgressEffects(ActiveAttack activeAttack)
    {
        activeAttack.particle.transform.position = Vector3.MoveTowards(activeAttack.particle.transform.position, scr_Grid.GridController.GetWorldLocation(activeAttack.position) + activeAttack.attack.particlesOffset, (particleSpeed) * Time.deltaTime);
    }

    public override void ImpactEffects(int xPos = -1, int yPos = -1)
    {

    }

    public override void EndEffects(ActiveAttack activeAttack)
    {
        Debug.Log(activeAttack.position.x + ", " + activeAttack.position.y);
        if (activeAttack.entityIsHit == false)
        {
            Instantiate(rock, scr_Grid.GridController.GetWorldLocation(activeAttack.position.x, activeAttack.position.y), Quaternion.identity);
            scr_Grid.GridController.SetTileOccupied(false, startX, startY, rock.GetComponent<Entity>());
            rock.GetComponent<Entity>().InitPosition(startX, startY);
            rock.GetComponent<Entity>().SetTransform(startX, startY);

            Debug.Log(rock.GetComponent<Entity>()._gridPos.x + ", " + rock.GetComponent<Entity>()._gridPos.y);
        }
    }
}
