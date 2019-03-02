using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Attacks/CrescentStrike")]
[RequireComponent(typeof(AudioSource))]

public class atk_CrescentStrike : AttackData
{
    private AudioSource PlayCardSFX;
    public AudioClip CrescentSFX;
    int playerX;
    int playerY;

    private void OnEnable()
    {
         playerX = GameObject.FindGameObjectWithTag("Player").GetComponent<scr_Entity>()._gridPos.x;
         playerY = GameObject.FindGameObjectWithTag("Player").GetComponent<scr_Entity>()._gridPos.y;
    }
    public override Vector2Int BeginAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        return new Vector2Int(xPos, yPos);
    }
    public override ActiveAttack BeginAttack(ActiveAttack activeAtk)
    {
        activeAtk.particle = Instantiate(particles, scr_Grid.GridController.GetWorldLocation(activeAtk.position), Quaternion.identity);
        PlayCardSFX = GameObject.Find("DeckManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = CrescentSFX;
        PlayCardSFX.Play();
        activeAtk.attack.maxIncrementRange = ((scr_Grid.GridController.columnSizeMax * 2) + scr_Grid.GridController.rowSizeMax - 2);
        return activeAtk;
    }

    public override bool CheckCondition(scr_Entity _ent)
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
        int tempVarX = xPos - playerX;
        int tempVarY = yPos - playerY;
        if (tempVarX < 2)
        {
            return new Vector2Int(xPos++, yPos);
        }
        else if (tempVarX == 2 && yPos >= playerY)
        {
            return new Vector2Int(xPos, yPos--);
        }
        else
        {
            return new Vector2Int(xPos, yPos--);
        }
    }

    public override void ProgressEffects(ActiveAttack activeAttack)
    {
        activeAttack.particle.transform.position = Vector3.Lerp(activeAttack.particle.transform.position, scr_Grid.GridController.GetWorldLocation(activeAttack.lastPosition.x, activeAttack.lastPosition.y) + activeAttack.attack.particlesOffset, (particleSpeed) * Time.deltaTime);
        activeAttack.particle.transform.Rotate(0, 0, 30, Space.Self);
        activeAttack.particle.sortingOrder = -activeAttack.position.y;
    }
}

