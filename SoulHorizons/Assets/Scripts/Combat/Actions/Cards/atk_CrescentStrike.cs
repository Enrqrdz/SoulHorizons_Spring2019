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

    public override Vector2Int BeginAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        return new Vector2Int(xPos, yPos);
    }
    public override ActiveAttack BeginAttack(ActiveAttack activeAtk)
    {
        activeAtk.particle = Instantiate(particles, scr_Grid.GridController.GetWorldLocation(activeAtk.position), Quaternion.identity);

        playerX = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>()._gridPos.x;
        playerY = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>()._gridPos.y;
        
        if(playerY == 0)
        {
            maxIncrementRange = maxIncrementRange - 1;
        }

        if (PlayCardSFX == null)
        {
            PlayCardSFX = GameObject.Find("ActionManager").GetComponent<AudioSource>();
        }

        PlayCardSFX.clip = CrescentSFX;
        PlayCardSFX.Play();
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
        int tempX = xPos - playerX;
        int tempY = yPos - playerY;
        if (tempX <= 1 && yPos > playerY)
        {
            scr_Grid.GridController.PrimeNextTile(xPos + 1, yPos);
            scr_Grid.GridController.ActivateTile(xPos, yPos);
            return new Vector2Int(xPos + 1, yPos);
        }
        else if (tempX == 2 && yPos <= playerY)
        {
            scr_Grid.GridController.PrimeNextTile(xPos, yPos + 1);
            scr_Grid.GridController.ActivateTile(xPos, yPos);
            return new Vector2Int(xPos, yPos + 1);
        }
        else
        {
            scr_Grid.GridController.PrimeNextTile(xPos - 1, yPos);
            scr_Grid.GridController.ActivateTile(xPos, yPos);
            return new Vector2Int(xPos - 1, yPos);
        }
    }
    public override void ProgressEffects(ActiveAttack activeAttack)
    {
        activeAttack.particle.transform.position = Vector3.Lerp(activeAttack.particle.transform.position, scr_Grid.GridController.GetWorldLocation(activeAttack.lastPosition.x, activeAttack.lastPosition.y) + activeAttack.attack.particlesOffset, (particleSpeed) * Time.deltaTime);
        activeAttack.particle.transform.Rotate(0, 0, 30, Space.Self);
        activeAttack.particle.sortingOrder = -activeAttack.position.y;
    }
}
