using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/StaticShield")]
[RequireComponent(typeof(AudioSource))]
public class atk_StaticShield : AttackData
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
        GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>().hasShield = false;
    }

    public override void ImpactEffects(int xPos = -1, int yPos = -1)
    {
    }

    public override void LaunchEffects(ActiveAttack activeAttack)
    {

    }

    public override Vector2Int ProgressAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {

        //scr_Grid.GridController.PrimeNextTile(xPos + 1, yPos);
        scr_Grid.GridController.ActivateTile(playerX - 1, playerY);
        scr_Grid.GridController.ActivateTile(playerX, playerY - 1);
        scr_Grid.GridController.ActivateTile(playerX + 1, playerY);
        scr_Grid.GridController.ActivateTile(playerX, playerY + 1);

        return new Vector2Int(xPos + 1, yPos);
 

    }
    public override void ProgressEffects(ActiveAttack activeAttack)
    {
        activeAttack.particle.transform.position = Vector3.Lerp(activeAttack.particle.transform.position, scr_Grid.GridController.GetWorldLocation(activeAttack.lastPosition.x, activeAttack.lastPosition.y) + activeAttack.attack.particlesOffset, (particleSpeed) * Time.deltaTime);
        activeAttack.particle.transform.Rotate(0, 0, 30, Space.Self);
        activeAttack.particle.sortingOrder = -activeAttack.position.y;
    }
}
