using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_FoulTrifling : scr_EntityAI
{
    public float movementIntervalLower;
    public float movementIntervalUpper;

    public AttackData attack1;
    public AttackData chargedAttack;
    private int attackCounter = 0; // keeps track of how many tiles the entity has moved before it can attack , if it reaches 3 then it attempts to do a charge attack

    AudioSource Attack_SFX;
    AudioSource Footsteps_SFX;
    public AudioClip[] movements_SFX;
    private AudioClip movement_SFX;
    public AudioClip[] attacks_SFX;
    private AudioClip attack_SFX;

    private int state = 0;

    private bool completedTask = true;
    private bool isStuck = false;
    private bool xDirection = false; //false means left, true means right
    private bool yDirection = false; //false means down, true means up

    public void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
    }

    public override void UpdateAI()
    {
        scr_Grid.GridController.SetTileOccupied(true, entity._gridPos.x, entity._gridPos.y, this.entity);
        if (completedTask)
        {
            StartCoroutine(Brain());
        }
    }

    public override void Move()
    {

        int xPos = entity._gridPos.x;
        int yPos = entity._gridPos.y;


        if (!xDirection)
        {
            xPos--;
            if (!scr_Grid.GridController.CheckIfOccupied(xPos, yPos) && (scr_Grid.GridController.ReturnTerritory(xPos, yPos).name == entity.entityTerritory.name))
            {
                entity.SetTransform(xPos, yPos);
                return;
            }
            else if (scr_Grid.GridController.ReturnTerritory(xPos, yPos).name == TerrName.Player)
            {
                xDirection = !xDirection;
                Move();
            }
        }
        else
        {
            try
            {
                xPos++;
                if (!scr_Grid.GridController.CheckIfOccupied(xPos, yPos) && (scr_Grid.GridController.ReturnTerritory(xPos, yPos).name == entity.entityTerritory.name))
                {
                    entity.SetTransform(xPos, yPos);
                    return;
                }
            }
            catch
            {
                xDirection = !xDirection;
                Move();
            }
        }
    }

    void MoveAlongColumn(int xPos, int yPos, bool direction)
    {

        if (direction)
        {
            yPos = yPos + 1;
        }
        else
        {
            yPos = yPos - 1;
        }

        try
        {
            if (!scr_Grid.GridController.CheckIfOccupied(xPos, yPos) && (scr_Grid.GridController.ReturnTerritory(xPos, yPos).name == entity.entityTerritory.name))
            {
                //if the tile is not occupied
                scr_Grid.GridController.SetTileOccupied(true, xPos, yPos, entity);          //set it to be occupied  
                entity.SetTransform(xPos, yPos);
                return;
            }
        }
        catch
        {
            isStuck = true;
            return;
        }
    }

    public override void Die()
    {
        entity.Death();
    }

    void AttackManager()
    {
        int random = Random.Range(1, 10);
        if (random < 7)
        {
            StartAttack1();
        }

        if (attackCounter >= 3)
        {
            int rand = Random.Range(0, 2);
            {
                if (rand == 1)
                {
                    StartAttack2();

                }
                attackCounter = 0;
            }
        }
    }

    void GetYDirection(int yPos)
    {
        int yRange = scr_Grid.GridController.rowSizeMax;
        if (yPos == 0)
        {
            yDirection = true;
        }
        else if (yPos == yRange - 1)
        {
            yDirection = false;
        }
        else
        {
            int random = Random.Range(0, 2);
            if (random == 1)
            {
                yDirection = true;
            }
            else
            {
                yDirection = false;
            }
        }
    }

    void Attack1()
    {
        AttackController.Instance.AddNewAttack(attack1, entity._gridPos.x, entity._gridPos.y, entity);
        int index2 = Random.Range(0, attacks_SFX.Length);
        attack_SFX = attacks_SFX[0];
        Attack_SFX.clip = attack_SFX;
        Attack_SFX.Play();
    }
    void Attack2()
    {
        AttackController.Instance.AddNewAttack(chargedAttack, entity._gridPos.x, entity._gridPos.y, entity);
        int index2 = Random.Range(0, attacks_SFX.Length);
        attack_SFX = attacks_SFX[0];
        Attack_SFX.clip = attack_SFX;
        Attack_SFX.Play();
    }
    void StartAttack1()
    {
        anim.SetBool("Attack", true);
    }
    void StartAttack2()
    {
        anim.SetBool("Attack2", true);
    }


    private IEnumerator Brain()
    {
        switch (state)
        {
            case 0:
                completedTask = false;
                Move();
                float moveInterval = Random.Range(movementIntervalLower, movementIntervalUpper);
                yield return new WaitForSecondsRealtime(moveInterval);
                attackCounter++;
                AttackManager();
                state = 1;
                completedTask = true;
                break;

            case 1:
                completedTask = false;
                int yRange = scr_Grid.GridController.rowSizeMax;
                GetYDirection(entity._gridPos.y);
                for (int i = 0; i < yRange; i++) //along the column
                {
                    attackCounter++;
                    AttackManager();

                    MoveAlongColumn(entity._gridPos.x, entity._gridPos.y, yDirection);
                    float moveInterval2 = Random.Range(movementIntervalLower, movementIntervalUpper);
                    yield return new WaitForSecondsRealtime(moveInterval2);
                    if (entity._gridPos.y == 0)
                    {
                        break;
                    }
                    if (isStuck)
                    {
                        isStuck = false;
                        break;
                    }

                }
                completedTask = true;
                state = 0;
                float moveInterval3 = Random.Range(movementIntervalLower, movementIntervalUpper);
                yield return new WaitForSecondsRealtime(moveInterval3);
                break;
        }
        yield return null;
    }
}
