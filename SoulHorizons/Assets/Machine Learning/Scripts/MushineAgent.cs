using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public enum AgentStates { MoveUp, MoveDown, Attacking, Idle }
public class MushineAgent : Agent
{
    [Header("Mushine Agent")]
    public Entity Player;
    private Entity MushineEntity;
    private MushineAI AI;

    private int playerHitCounter;
    private int totalAgentAttacks;

    private int agentHitCounter;
    private int playerTotalAttacks;

    //movementCooldown
    //idleFrequency
    //primaryAttack.incrementTime
    //primaryAttack.damage

    public override void InitializeAgent()
    {
        Debug.Log("Initializing Agent");
        try
        {
            Player = ObjectReference.Instance.PlayerEntity;
        }
        catch
        {
            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
        }
        MushineEntity = gameObject.GetComponent<Entity>();
        AI = gameObject.GetComponent<MushineAI>();
    }

    public override void CollectObservations()
    {
        AddVectorObs(AI.movementCooldown);              //Movement Frequency
        AddVectorObs(AI.idleFrequency);                 //Idle Frequency (Harder to predict)
        AddVectorObs(AI.primaryAttack.incrementTime);   //Attack Speed
        AddVectorObs(AI.primaryAttack.damage);          //Attack Damage
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if(brain.brainParameters.vectorActionSpaceType == SpaceType.continuous)
        {
            var actionMoveCooldown = Mathf.Clamp(vectorAction[0], -AI.movementIncrement, AI.movementIncrement);
            var actionIdleFrequency = Mathf.Clamp(vectorAction[1], -AI.idleIncrement, AI.idleIncrement);
            var actionAttackSpeed = Mathf.Clamp(vectorAction[2], -AI.speedIncrement, AI.speedIncrement);
            var actionDamage = Mathf.Clamp(vectorAction[3], -AI.damageIncrement, AI.damageIncrement);

            AI.movementCooldown += actionMoveCooldown;
            AI.idleFrequency += (int)actionIdleFrequency;
            AI.primaryAttack.incrementTime += actionAttackSpeed;
            AI.primaryAttack.damage += (int)actionDamage;

            AI.movementCooldown = Mathf.Clamp(AI.movementCooldown, AI.minimumMovementCooldown, AI.startingMovementCooldown);
            AI.idleFrequency = Mathf.Clamp(AI.idleFrequency, AI.minimumIdleFrequency, AI.startingIdleFrequency);
            AI.primaryAttack.incrementTime = Mathf.Clamp(AI.primaryAttack.incrementTime, AI.startingSpeed, AI.minSpeed);
            AI.primaryAttack.damage = Mathf.Clamp(AI.primaryAttack.damage, AI.startingDamage, AI.maxDamage);


        }

        else if(Player.isBeingEvasive == false || MushineEntity.isBeingEvasive)
        {
            SetReward(0.1f + (AI.primaryAttack.damage / 10f));
        }

        if (Player.isBeingEvasive || MushineEntity.isHit)
        {
            SetReward(-0.1f);
        }

        if(Player._health.hp <= 0)
        {
            SetReward((MushineEntity._health.hp/ MushineEntity._health.max_hp) * 100);
            Done();
        }
        if (MushineEntity._health.hp <= 0)
        {
            SetReward(-(Player._health.hp / Player._health.max_hp) * 100);
            Done();
        }
    }

    public override void AgentReset()
    {
        if(Player == null)
        {
            Player = ObjectReference.Instance.PlayerEntity;
        }
        Player._health.hp = Player._health.max_hp;

        if(MushineEntity == null)
        {
            MushineEntity = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Entity>();
        }
        MushineEntity._health.hp = MushineEntity._health.max_hp;
    }

    public override void AgentOnDone()
    {
        Player._health.hp = Player._health.max_hp;
        MushineEntity._health.hp = MushineEntity._health.max_hp;
    }
}