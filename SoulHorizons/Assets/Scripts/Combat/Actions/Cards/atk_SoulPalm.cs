using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/SoulPalm")]
[RequireComponent(typeof(AudioSource))]

public class atk_SoulPalm : AttackData
{
    private AudioSource PlayCardSFX;
    public AudioClip PalmSFX;

    Entity player;
    int playerX;
    int playerY;
    public int currentIncrement;
    public int tempDamage;
    public float verticalOffset = 1.5f;
    public float horizontalOffset = 1.5f;
    static int counter = 0;
    
    public override Vector2Int BeginAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        currentIncrement = 0;
        damage = 0;
        return new Vector2Int(xPos, yPos);
    }

    public override ActiveAttack BeginAttack(ActiveAttack activeAtk)
    {
        counter++;
        currentIncrement = 0;
        damage = 0;

        if (counter == 1)
        {
            activeAtk.particle = Instantiate(particles, scr_Grid.GridController.GetWorldLocation(playerX + 1, playerY) + new Vector3(horizontalOffset, verticalOffset, 0), Quaternion.identity);
        }

        if(player == null)
        {
            player = ObjectReference.Instance.PlayerEntity;
        }
        playerX = player._gridPos.x;
        playerY = player._gridPos.y;

        if (PlayCardSFX == null)
        {
            PlayCardSFX = ObjectReference.Instance.ActionManager;
        }
        PlayCardSFX.clip = PalmSFX;
        PlayCardSFX.Play();

        return activeAtk;
    }

    public override bool CheckCondition(Entity entity)
    {
        return false;
    }

    public override void EndEffects(ActiveAttack activeAttack)
    {
        counter = 0;
        currentIncrement = 0;
    }

    public override void ImpactEffects(int xPos = -1, int yPos = -1)
    {
    }

    public override void LaunchEffects(ActiveAttack activeAttack)
    {

    }

    public override Vector2Int ProgressAttack(int xPos, int yPos, ActiveAttack activeAtk)
    {
        currentIncrement++;
        currentIncrement %= (6 * (maxIncrementRange + 1)) + 1;

        if (currentIncrement > 6 * maxIncrementRange - 1)
        {
            damage = tempDamage;
        }
        else
        {
            damage = 0;
        }
        return new Vector2Int(xPos, yPos);
    }

    public override void ProgressEffects(ActiveAttack activeAttack)
    {
        if (activeAttack.particle != null)
        {
            activeAttack.particle.transform.position =
            Vector3.Lerp(activeAttack.particle.transform.position,
            scr_Grid.GridController.GetWorldLocation(activeAttack.lastPosition.x, activeAttack.lastPosition.y) + activeAttack.attack.particlesOffset,
            (particleSpeed) * Time.deltaTime);
        }
    }
}
