using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/GuardBreak")]
public class atk_GuardBreak : AttackData 
{
    Entity player;
    public float stunTime = 1.5f;
    public float timeTeleported = .25f;
    int playerX;
    int playerY;
    public override Vector2Int ProgressAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        scr_Grid.GridController.ActivateTile(xPos, yPos);
        player.isStunned = true;
        player.SetTransform(xPos - 1, yPos);
        return new Vector2Int(xPos + 1, yPos);
    }

    public override void LaunchEffects(ActiveAttack activeAttack)
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
        playerX = player._gridPos.x;
        playerY = player._gridPos.y;
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
        try
        {
            activeAttack.entityHit.gotStunned(stunTime);
            player.SetTransform(activeAttack.position.x - 1, activeAttack.position.y);
            player.SetTransform(playerX, playerY);
            player.isStunned = false;
        }
        catch
        { }

    }
}
