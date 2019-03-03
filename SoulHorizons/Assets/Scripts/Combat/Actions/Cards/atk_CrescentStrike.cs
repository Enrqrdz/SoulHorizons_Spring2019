using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/CrescentStrike")]
[RequireComponent(typeof(AudioSource))]

public class atk_CrescentStrike : AttackData
{
    private AudioSource PlayCardSFX;
    public AudioClip CrescentSFX;


    public override Vector2Int BeginAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        return new Vector2Int(xPos, yPos);
    }
    public override ActiveAttack BeginAttack(ActiveAttack activeAtk)
    {
        activeAtk.particle = Instantiate(particles, scr_Grid.GridController.GetWorldLocation(activeAtk.position), Quaternion.identity);

        if (PlayCardSFX == null)
        {
            PlayCardSFX = GameObject.Find("ActionManager").GetComponent<AudioSource>();
        }

        PlayCardSFX.clip = CrescentSFX;
        PlayCardSFX.Play();

        activeAtk.attack.maxIncrementRange = ((scr_Grid.GridController.columnSizeMax * 2) + scr_Grid.GridController.rowSizeMax - 2);
        return activeAtk;
    }

    public override bool CheckCondition(Entity entity)
    {
        return false;
    }

    public override void EndEffects(ActiveAttack activeAttack)
    {

    }

    public override void ImpactEffects(int xPos = -1, int yPos = -1)
    {
    }

    public override void LaunchEffects(ActiveAttack activeAttack)
    {

    }

    public override Vector2Int ProgressAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {

        if (activeAtk.currentIncrement < 1)
        {
            return new Vector2Int(xPos++, yPos);
        }
        else if (activeAtk.currentIncrement >= 1 && activeAtk.currentIncrement < 3)
        {
            return new Vector2Int(xPos, yPos--);
        }
        else
        {
            return new Vector2Int(xPos--, yPos);
        }
    }

    public override void ProgressEffects(ActiveAttack activeAttack)
    {
        activeAttack.particle.transform.position = Vector3.Lerp(activeAttack.particle.transform.position, scr_Grid.GridController.GetWorldLocation(activeAttack.lastPosition.x, activeAttack.lastPosition.y) + activeAttack.attack.particlesOffset, (particleSpeed) * Time.deltaTime);
        activeAttack.particle.transform.Rotate(0, 0, 30, Space.Self);
        activeAttack.particle.sortingOrder = -activeAttack.position.y;
    }
}
