using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Bosses/Raitori/StormStrikes")]
public class atk_StormStrikes : AttackData
{
    public Raitori_Stages stage_manager;

    public override Vector2Int BeginAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        return new Vector2Int(xPos, yPos);
    }

    public override Vector2Int ProgressAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        if (activeAtk.currentIncrement == maxIncrementRange)
        {
            switch (Raitori_Stages.Instance.currentPhase)
            {
                case Phase.Stage1:
                    if (xPos > 0)
                    {
                        scr_Grid.GridController.ActivateTile(xPos - 1, yPos);
                    }

                    scr_Grid.GridController.ActivateTile(xPos, yPos);
                    scr_Grid.GridController.ActivateTile(xPos + 1, yPos);
                    break;

                case Phase.Stage2:
                    if (xPos > 0)
                    {
                        scr_Grid.GridController.ActivateTile(xPos - 1, yPos);
                    }
                    if (yPos > 0)
                    {
                        scr_Grid.GridController.ActivateTile(xPos, yPos - 1);
                    }
                    if (yPos < scr_Grid.GridController.rowSizeMax)
                    {
                        scr_Grid.GridController.ActivateTile(xPos, yPos + 1);
                    }

                    scr_Grid.GridController.ActivateTile(xPos, yPos);
                    scr_Grid.GridController.ActivateTile(xPos + 1, yPos);
                    break;

                case Phase.Stage3:
                    if (xPos > 0)
                    {
                        scr_Grid.GridController.ActivateTile(xPos - 1, yPos);
                        if (yPos > 0)
                        {
                            scr_Grid.GridController.ActivateTile(xPos - 1, yPos - 1);
                            scr_Grid.GridController.ActivateTile(xPos + 1, yPos - 1);

                            if (yPos < scr_Grid.GridController.rowSizeMax)
                            {
                                scr_Grid.GridController.ActivateTile(xPos - 1, yPos + 1);
                                scr_Grid.GridController.ActivateTile(xPos + 1, yPos + 1);
                            }
                        }
                    }
                    if (yPos > 0)
                    {
                        scr_Grid.GridController.ActivateTile(xPos, yPos - 1);
                    }
                    if (yPos < scr_Grid.GridController.rowSizeMax)
                    {
                        scr_Grid.GridController.ActivateTile(xPos, yPos + 1);
                    }

                    scr_Grid.GridController.ActivateTile(xPos, yPos);
                    scr_Grid.GridController.ActivateTile(xPos + 1, yPos);
                    break;
                default:
                    break;
            }
        }

        return new Vector2Int(xPos, yPos);
    }

    public override bool CheckCondition(Entity _ent)
    {
        return true;
    }

    public override void LaunchEffects(ActiveAttack activeAttack)
    {
        activeAttack.particle = Instantiate(particles, scr_Grid.GridController.GetWorldLocation(activeAttack.position.x, activeAttack.position.y) + particlesOffset, Quaternion.identity);
        activeAttack.particle.sortingOrder = -activeAttack.position.y;
    }

    public override void ProgressEffects(ActiveAttack activeAttack)
    {
        activeAttack.particle.transform.position = Vector3.Lerp(activeAttack.particle.transform.position, scr_Grid.GridController.GetWorldLocation(activeAttack.lastPosition.x, activeAttack.lastPosition.y) + activeAttack.attack.particlesOffset, (4.5f) * Time.deltaTime);
    }

    public override void ImpactEffects(int xPos = -1, int yPos = -1)
    {
    }
    public override void EndEffects(ActiveAttack activeAttack)

    {
    }
}
