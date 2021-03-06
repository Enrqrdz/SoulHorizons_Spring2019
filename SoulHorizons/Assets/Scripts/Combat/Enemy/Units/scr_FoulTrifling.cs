﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_FoulTrifling : scr_EntityAI
{
    public float movementInterval;

    public AttackData attack1;
    public AttackData chargedAttack;
    private int attackCounter = 0; // keeps track of how many tiles the entity has moved before it can attack , if it reaches 3 then it attempts to do a charge attack

    AudioSource[] SFX_Sources;
    AudioSource Attack_SFX;
    AudioSource Footsteps_SFX;
    public AudioClip[] movements_SFX;
    private AudioClip movement_SFX;
    public AudioClip[] attacks_SFX;
    private AudioClip attack_SFX;

    private int state = 0;
    private bool completedTask = true;
    private bool isStuck = false;
    private bool moveRight = false; //false means left, true means right
    private bool moveUp = false; //false means down, true means up

    public void Start()
    {
        AudioSource[] SFX_Sources = GetComponents<AudioSource>();
        Footsteps_SFX = SFX_Sources[0];
        Attack_SFX = SFX_Sources[1];
        anim = gameObject.GetComponentInChildren<Animator>();
        scr_Grid.GridController.SetTileOccupied(true, entity._gridPos.x, entity._gridPos.y, this.entity);
    }

    public override void UpdateAI()
    {
        if (completedTask)
        {
            StartCoroutine(Brain());
        }
    }

    public override void Move()
    {

        int xPos = entity._gridPos.x;
        int yPos = entity._gridPos.y;


        int index = Random.Range(0, movements_SFX.Length);
        movement_SFX = movements_SFX[index];
        Footsteps_SFX.clip = movement_SFX;
        Footsteps_SFX.Play();


        if (!moveRight)
        {
            xPos--;
            if (!scr_Grid.GridController.CheckIfOccupied(xPos, yPos) && (scr_Grid.GridController.ReturnTerritory(xPos, yPos).name == entity.entityTerritory.name))
            {
                entity.SetTransform(xPos, yPos);
                return;
            }
            else if (scr_Grid.GridController.ReturnTerritory(xPos, yPos).name == TerrName.Player)
            {
                moveRight = !moveRight;
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
                moveRight = !moveRight;
                Move();
            }
        }
    }

    void MoveAlongColumn(int xPos, int yPos, bool direction)
    {
        int index = Random.Range(0, movements_SFX.Length);
        movement_SFX = movements_SFX[index];
        Footsteps_SFX.clip = movement_SFX;
        Footsteps_SFX.Play();
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
        else
        {
            int random = Random.Range(1, 10);
            if (random < 7)
            {
                StartAttack1();
            }
        }
       
    }

    void GetYDirection(int yPos)
    {
        int yRange = scr_Grid.GridController.rowSizeMax;
        if (yPos == 0)
        {
            moveUp = true;
        }
        else if (yPos == yRange - 1)
        {
            moveUp = false;
        }
        else
        {
            int random = Random.Range(0, 2);
            if (random == 1)
            {
                moveUp = true;
            }
            else
            {
                moveUp = false;
            }
        }
    }

    void Attack1()
    {
        AudioSource[] SFX_Sources = GetComponents<AudioSource>();
        Attack_SFX = SFX_Sources[0];
        int index = Random.Range(0, attacks_SFX.Length);
        attack_SFX = attacks_SFX[index];
        Attack_SFX.clip = attack_SFX;
        Attack_SFX.Play();
        AttackController.Instance.AddNewAttack(attack1, entity._gridPos.x, entity._gridPos.y, entity);
    }
    void Attack2()
    {

        AudioSource[] SFX_Sources = GetComponents<AudioSource>();
        Attack_SFX = SFX_Sources[0];
        int index = Random.Range(0, attacks_SFX.Length);
        attack_SFX = attacks_SFX[index];
        Attack_SFX.clip = attack_SFX;
        Attack_SFX.Play();
        AttackController.Instance.AddNewAttack(chargedAttack, entity._gridPos.x, entity._gridPos.y, entity);
    }
    void StartAttack1()
    {
        anim.SetBool("Attack", true);
        PrimeAttackTiles(attack1, entity._gridPos.x, entity._gridPos.y);
    }
    void StartAttack2()
    {
        anim.SetBool("Attack2", true);
        PrimeAttackTiles(chargedAttack, entity._gridPos.x, entity._gridPos.y);
    }

    private IEnumerator Brain()
    {
        switch (state)
        {
            case 0:
                completedTask = false;
                Move();
                yield return new WaitForSeconds(movementInterval);
                state = 1;
                completedTask = true;
                break;

            case 1:
                completedTask = false;
                int yRange = scr_Grid.GridController.rowSizeMax;
                GetYDirection(entity._gridPos.y);
                for (int i = 0; i < yRange; i++) //along the column
                {
                    MoveAlongColumn(entity._gridPos.x, entity._gridPos.y, moveUp);
                    attackCounter++;
                    AttackManager();
                    yield return new WaitForSeconds(movementInterval);
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
                yield return new WaitForSeconds(movementInterval);
                break;
            case 2:
                break;
        }
        yield return null;
    }
}
