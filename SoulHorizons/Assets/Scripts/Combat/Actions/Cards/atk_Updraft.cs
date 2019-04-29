using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Updraft")]
[RequireComponent(typeof(AudioSource))]

public class atk_Updraft : AttackData
{
    Entity player;
    public override Vector2Int BeginAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        player = ObjectReference.Instance.PlayerEntity;
        activeAtk.particle = Instantiate(particles, scr_Grid.GridController.GetWorldLocation(activeAtk.entity._gridPos.x, activeAtk.entity._gridPos.y) + new Vector3(0, 2.5f, 0), Quaternion.Euler(new Vector3(0, 0, 320)));   
        return new Vector2Int(xPos, yPos);
    }
    public override ActiveAttack BeginAttack(ActiveAttack activeAtk)
    {
        return activeAtk;
    }

    public override Vector2Int ProgressAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        return LinearForward_ProgressAttack(xPos, yPos, activeAtk);
    }

    Vector2Int LinearForward_ProgressAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        scr_Grid.GridController.ActivateTile(xPos, yPos);
        return new Vector2Int(xPos, yPos);
    }
    public override bool CheckCondition(Entity _ent)
    {
        return true;
    }

    public override void LaunchEffects(ActiveAttack activeAttack)
    {

    }

    public override void ProgressEffects(ActiveAttack activeAttack)
    {
        activeAttack.particle.transform.position = Vector3.MoveTowards(activeAttack.particle.transform.position, scr_Grid.GridController.GetWorldLocation(activeAttack.position) + activeAttack.attack.particlesOffset, (18f) * Time.deltaTime);
    }

    public override void ImpactEffects(int xPos = -1, int yPos = -1)
    {
    }

    public override void EndEffects(ActiveAttack activeAttack)
    {
        player.isStunned = false;
        player.spr.color = Color.white;
        player.invincible = false;
    }
}
