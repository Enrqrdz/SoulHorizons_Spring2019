using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    public PlayerData dataToEmulate;
    public ActionData abilityToActivate;
    private Entity player;
    private Entity enemy;
    private MushineAI enemyAI;
    private float movementFrequency;
    private float attackFrequency;
    private bool isMoving = false;
    private bool isAttacking = false;
    private bool isAbleToShoot;
    private int movementIndex;
    private int attackIndex;
    private int horizontalIndex;
    private int verticalIndex;


    private void Start()
    {
        player = gameObject.GetComponent<Entity>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Entity>();
        enemyAI = enemy.GetComponent<MushineAI>();
    }

    void Update()
    {
        FindMovementAndAttackIndex();
        FindDistanceIndex();

        movementFrequency = dataToEmulate.phaseData[movementIndex].movementFrequency;
        attackFrequency = dataToEmulate.phaseData[attackIndex].attackFrequency;

        if(isMoving == false)
        {
            StartCoroutine(Moving());
        }
        if(isAttacking == false)
        {
            StartCoroutine(Attacking());
        }
    }

    private IEnumerator Moving()
    {
        isMoving = true;
        player._gridPos.x = enemy._gridPos.x + dataToEmulate.phaseData[horizontalIndex].horizontalDistance;
        player._gridPos.y = enemy._gridPos.y + dataToEmulate.phaseData[verticalIndex].verticalDistance;
        yield return new WaitForSeconds(1/movementFrequency);
        isMoving = false;
    }

    private IEnumerator Attacking()
    {
        isAttacking = true;
        abilityToActivate.Activate();
        yield return new WaitForSeconds(0.7f);
        isAttacking = false;
    }

    private void AbleToShoot()
    {
        throw new NotImplementedException();
    }

    private void FindDistanceIndex()
    {
        for (int i = 0; i < dataToEmulate.phaseData.Count; i++)
        {
            if (enemyAI.speedIncrement <= enemyAI.startingSpeed - enemyAI.speedIncrement * i
                && enemyAI.speedIncrement > enemyAI.startingSpeed - enemyAI.speedIncrement * (i + 1))
            {
                horizontalIndex = i;
                verticalIndex = i;
                break;
            }
        }
    }

    private void FindMovementAndAttackIndex()
    {
        for (int i = 0; i < dataToEmulate.phaseData.Count; i++)
        {
            if (enemyAI.movementCooldown <= enemyAI.startingMovementCooldown - enemyAI.movementIncrement * i
                && enemyAI.movementCooldown > enemyAI.startingMovementCooldown - enemyAI.movementIncrement * (i + 1))
            {
                movementIndex = i;
                attackIndex = i;
                break;
            }
        }
    }
}
