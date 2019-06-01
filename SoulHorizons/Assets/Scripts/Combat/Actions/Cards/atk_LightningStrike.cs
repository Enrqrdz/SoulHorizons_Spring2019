using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Lightning Strike")]
public class atk_LightningStrike : AttackData
{
    private AudioSource PlayCardSFX;
    public AudioClip LightningSFX;

    Entity player;
    int playerX;
    int playerY;
    public float horizontalOffset = 2f;
    public float verticalOffset = 1.5f;

    public override Vector2Int BeginAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        return new Vector2Int(xPos, yPos);
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
        activeAttack.particle = Instantiate(particles, scr_Grid.GridController.GetWorldLocation(activeAttack.position.x, activeAttack.position.y) + particlesOffset, Quaternion.identity);
        activeAttack.particle.sortingOrder = -activeAttack.position.y;

        if (PlayCardSFX == null)
        {
            PlayCardSFX = ObjectReference.Instance.ActionManager;
        }
        PlayCardSFX.clip = LightningSFX;
        PlayCardSFX.Play();
    }

    public override Vector2Int ProgressAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        return new Vector2Int(xPos, yPos);
    }
    public override void ProgressEffects(ActiveAttack activeAttack)
    {

    }
}
