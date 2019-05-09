using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public enum AgentStates {MoveUp, MoveDown, Attacking, Idle}
public class MushineAgent : Agent
{
    public Entity Player;
    private MushineAI AI;
    private Entity MushineEntity;

    //movementCooldown
    //idleFrequency
    //primaryAttack.incrementTime
    //primaryAttack.damage


    private void Start()
    {
        MushineEntity = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Entity>();
        AI = MushineEntity.GetComponent<MushineAI>();
    }

    public override void CollectObservations()
    {
        AddVectorObs(AI.movementCooldown);
        AddVectorObs(AI.idleFrequency);
        AddVectorObs(AI.primaryAttack.incrementTime);
        AddVectorObs(AI.primaryAttack.damage);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {

    }

    public override void AgentReset()
    {

    }

    public override void AgentOnDone()
    {

    }
}