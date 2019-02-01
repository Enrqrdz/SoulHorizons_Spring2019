using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class scr_EnemyAI_1 : scr_EntityAI
{

    public float basicAttackChance;
    public float chargeAttackChance;
    public float chargeTimeUpper;
    public float chargeTimeLower; 
    public float movementIntervalLower;
    public float movementIntervalUpper;
    bool waiting = false;
    public AttackData attack1;
    public AttackData chargedAttack;

    AudioSource Attack_SFX;
    AudioSource Footsteps_SFX;
    public AudioClip[] movements_SFX;
    private AudioClip movement_SFX;
    public AudioClip[] attacks_SFX;
    private AudioClip attack_SFX;
    private bool canMove = true;
    private bool charging = false;
    private int state = 2;
    private bool completedTask = true;
    private bool broken = false; //This is to check if something has caused their AI to stop working 

    public void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
    }

    public override void Move()
    {

    }
    public override void Attack()
    {

    }
    public override void UpdateAI()
    {
        scr_Grid.GridController.SetTileOccupied(true, entity._gridPos.x, entity._gridPos.y, this.entity);
        if (completedTask)
        {
            StartCoroutine(Brain()); 
        }
    }

    public override void Die()
    {
        entity.Death();
    }

    void Movement()
    {
        AudioSource[] SFX_Sources = GetComponents<AudioSource>();
        Footsteps_SFX = SFX_Sources[0];
        Attack_SFX = SFX_Sources[1];
        int index = Random.Range(0, movements_SFX.Length);
        movement_SFX = movements_SFX[index];
        Footsteps_SFX.clip = movement_SFX;
        Footsteps_SFX.Play();

        //Decide if we are moving horiz or vert.
        int _temp = Random.Range(0, 2);                                         //Pick a number between 0 and 1
        int _x = entity._gridPos.x;
        int _y = entity._gridPos.y;
        int _tries = 0; 
        
        

        while(_tries < 10)
        {
            _temp = Random.Range(0, 2);
            if (_temp == 0)                                                          //if that number == 0, then we're moving vertically 
            {
                _y = PickYCoord();

            }
            else if (_temp == 1)                                                     //if that number == 1, we're moving horizonally 
            {
                _x = PickXCoord();

            }

            if (!scr_Grid.GridController.CheckIfOccupied(_x, _y) && (scr_Grid.GridController.ReturnTerritory(_x, _y).name == entity.entityTerritory.name))
            {
                entity.SetTransform(_x, _y);                               //move to new position
                completedTask = true;
                return;
            }
            else
            {
                _tries++;
                if (_tries >= 10)
                {
                    broken = true;
                    Debug.Log("I think I am broken");
                }
            }
        }
        completedTask = true;
    }


    void Attack1()
    {
        //make sure we do a check condition for the attack : if(chargedAttack.CheckCondition(entity))
        scr_AttackController.attackController.AddNewAttack(attack1, entity._gridPos.x, entity._gridPos.y, entity);
        int index2 = Random.Range(0, attacks_SFX.Length);
        attack_SFX = attacks_SFX[index2];
        Attack_SFX.clip = attack_SFX;
        Attack_SFX.Play();
    }
    void Attack2()
    {
        scr_AttackController.attackController.AddNewAttack(chargedAttack, entity._gridPos.x, entity._gridPos.y, entity);
    }
    void StartAttack1()
    {
        anim.SetBool("Attack", true);
    }
    void StartAttack2()
    {
        anim.SetBool("Attack2", true);
    }





    int PickXCoord()
    {
        //must return int 
        int _range = scr_Grid.GridController.maxColumnSize;
        int _currPosX = entity._gridPos.x;

        if(_currPosX == _range - 1)
        {
            return (_currPosX - 1); 
        }
        else if(_currPosX == _range / 2)
        {
            return _currPosX + 1; 
        }
        else
        {
            int _temp = Random.Range(0, 2);
            if(_temp == 0)
            {
                return _currPosX + 1; 
            }
            else if(_temp == 1)
            {
                return _currPosX - 1; 
            }

            return 0;
        }

    }

    int PickYCoord()
    {
        if (entity._gridPos.y == 0)                             //AI is on y = 0 and can only move to 1 (down)                             
        {
            return 1;
        }
        else if (entity._gridPos.y == 1)                        //AI is on y = 1 and can move either up or down
        {
            int _temp = Random.Range(0, 2);             //make a random number 0 or 1
            if (_temp == 0)                              //if this number is 0, move to 0 (up)
            {
                return 0;
            }
            else                                        //if this number is 1, move to 1 (down) 
            {
                return 2;
            }
        }
        else                                            //otherwise, the AI is on 2 and can only move to 1 (up)
        {
            return 1;
        }
    }

    int AngerTest()                     //here is where we will decide whether or not the unit attacks, and if it does, what type of attack: returns a state # for the brain  
    {
        //Do I attack?
        int _testVal = Random.Range(0, 100);
        if (_testVal <= basicAttackChance)
        {
            if (_testVal <= chargeAttackChance)
            {
                completedTask = true; 
                return 4; //Begins Charging charged attack 
            }
            else
            {
                completedTask = true;
                return 3; //small wait then, casts basic attack 
            }
        }
        else
        {
            completedTask = true;
            return 2;   //small wait to move again 
        }
    }
    
    private IEnumerator Brain()
    {
        switch(state)
        {
            case 0:
                completedTask = false; 
                Movement();

                state = 1; 
                break;

            case 1:
                completedTask = false; 
                state = AngerTest(); 
                break;

            case 2:
                completedTask = false;
                float _movementInterval = Random.Range(movementIntervalLower, movementIntervalUpper);
                yield return new WaitForSecondsRealtime(_movementInterval);
                state = 0;
                completedTask = true; 
                break;

            case 3:
                completedTask = false;
                yield return new WaitForSecondsRealtime(.75f); 
                StartAttack1();
                yield return new WaitForSecondsRealtime(2);
                state = 0;
                completedTask = true;
                break;

            case 4:
                completedTask = false;
                float _thisTime = Random.Range(chargeTimeLower, chargeTimeUpper);
                yield return new WaitForSecondsRealtime(_thisTime); 
                StartAttack2();
                yield return new WaitForSecondsRealtime(2f);
                state = 0;
                completedTask = true;
                break;
        }
        yield return null; 
    }
    

}