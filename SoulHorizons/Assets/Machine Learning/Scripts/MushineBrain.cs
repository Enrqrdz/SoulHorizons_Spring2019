using System.Collections.Generic;
using UnityEngine;
using MLAgents;


public class MushineBrain : Decision
{
    [Header("Mushine Brain")]
    private Entity Player;
    private Entity MushineEntity;
    private MushineAI AI;

    private void Awake()
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

    public override float[] Decide(
        List<float> vectorObs,
        List<Texture2D> visualObs,
        float reward,
        bool done,
        List<float> memory)
    {

        Player = ObjectReference.Instance.PlayerEntity;

        if (brainParameters.vectorActionSpaceType == SpaceType.continuous)
        {
            List<float> actions = new List<float>();

            if(AI.entity.isHit)
            {
                Debug.Log("Increasing Movement");
                actions.Add(vectorObs[0] += AI.movementIncrement);
                actions.Add(vectorObs[1] += AI.idleIncrement);
            }
            else if(MushineEntity.isBeingEvasive)
            {
                Debug.Log("Decreasing Movement");
                actions.Add(vectorObs[0] -= AI.movementIncrement);
                actions.Add(vectorObs[1] -= AI.idleIncrement);
            }
            if (Player.isBeingEvasive)
            {
                Debug.Log("Increasing attack speed");
                actions.Add(vectorObs[2] += AI.speedIncrement);
            }
            if (Player.isBeingEvasive == false && AI.playerIsStayingOnRow)
            {
                Debug.Log("Increasing attack damage");
                actions.Add(vectorObs[3] += AI.damageIncrement);
                AI.playerIsStayingOnRow = false;
            }

        }

        return new float[1] { 1f };
    }

    public override List<float> MakeMemory(List<float> vectorObs, List<Texture2D> visualObs, float reward, bool done, List<float> memory)
    {
        return new List<float>();
    }
}
