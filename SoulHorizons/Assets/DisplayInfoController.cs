using UnityEngine;

public class DisplayInfoController : MonoBehaviour
{
    public Transform move, idle, speed, damage;
    private MushineAI mushineAI;
    private float originalMoveScale, originalIdleScale, originalSpeedScale, originalDamageScale;
    private float agentMoveMin, agentIdleMin, attackSpeedMin, attackDamageMin;
    private float agentMoveMax, agentIdleMax, attackSpeedMax, attackDamageMax;
    private float moveRange, idleRange, speedRange, damageRange;

    private void Awake()
    {
        mushineAI = GameObject.FindGameObjectWithTag("Enemy").GetComponent<MushineAI>();
    }

    private void Start()
    {
        agentMoveMin = mushineAI.minimumMovementCooldown;
        agentIdleMin = mushineAI.minimumIdleFrequency;
        attackSpeedMin = mushineAI.minSpeed;
        attackDamageMin = mushineAI.startingDamage;

        agentMoveMax = mushineAI.startingMovementCooldown;
        agentIdleMax = mushineAI.startingIdleFrequency;
        attackSpeedMax = mushineAI.startingSpeed;
        attackDamageMax = mushineAI.maxDamage;

        moveRange = agentMoveMax - agentMoveMin;
        idleRange = agentIdleMax - agentIdleMin;
        speedRange = attackSpeedMax - attackSpeedMin;
        damageRange = attackDamageMax - attackDamageMin;

        originalMoveScale = move.localScale.x;
        originalIdleScale = idle.localScale.x;
        originalSpeedScale = speed.localScale.x;
        originalDamageScale = damage.localScale.x;
    }

    //Inverse for Movement - Lower is better
    //Inverse for Speed - Lower is better
    //Inverse for Idle - Lower is better
    //Damage is normal - Higher is better

    private void Update()
    {
        float movePercentage = 1 - ((mushineAI.movementCooldown - agentMoveMin)/moveRange);
        float idlePercentage = 1 - ((mushineAI.idleFrequency - agentIdleMin)/idleRange);
        float speedPercentage = 1 - ((mushineAI.primaryAttack.incrementTime - mushineAI.minSpeed)/speedRange);
        float damagePercentage = ((float)mushineAI.primaryAttack.damage / (float)mushineAI.maxDamage);

        move.localScale = new Vector3(originalMoveScale * movePercentage, move.localScale.y, move.localScale.z);
        idle.localScale = new Vector3(originalIdleScale * idlePercentage, idle.localScale.y, idle.localScale.z);
        speed.localScale = new Vector3(originalSpeedScale * speedPercentage, speed.localScale.y, speed.localScale.z);
        damage.localScale = new Vector3(originalDamageScale * damagePercentage, damage.localScale.y, damage.localScale.z);
    }
}
