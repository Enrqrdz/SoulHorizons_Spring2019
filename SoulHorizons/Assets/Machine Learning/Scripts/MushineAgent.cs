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
        try
        {
            Player = ObjectReference.Instance.PlayerEntity;
        }
        catch
        {
            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
        }
        MushineEntity = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Entity>();
        AI = MushineEntity.GetComponent<MushineAI>();
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
            var actionMoveCooldown = Mathf.Clamp(vectorAction[0], AI.minimumMovementCooldown, AI.startingMovementCooldown);
            var actionIdleFrequency = Mathf.Clamp(vectorAction[1], AI.minimumIdleFrequency, AI.minimumIdleFrequency);
            var actionAttackSpeed = Mathf.Clamp(vectorAction[2], AI.startingSpeed, AI.maxSpeed);
            var actionDamage = Mathf.Clamp(vectorAction[3], AI.startingDamage, AI.maxDamage);

            AI.movementCooldown = actionMoveCooldown;
            AI.idleFrequency = (int)actionIdleFrequency;
            AI.primaryAttack.incrementTime = actionAttackSpeed;
            AI.primaryAttack.damage = (int)actionDamage;
        }
        if (Player.isBeingEvasive || MushineEntity.isBeingEvasive == false)
        {
            SetReward(-0.1f);
        }
        else if(Player.isBeingEvasive == false || MushineEntity.isBeingEvasive)
        {
            SetReward(0.1f);
        }
        if(Player._health.hp <= 0)
        {
            Done();
        }
        if (MushineEntity._health.hp <= 0)
        {
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