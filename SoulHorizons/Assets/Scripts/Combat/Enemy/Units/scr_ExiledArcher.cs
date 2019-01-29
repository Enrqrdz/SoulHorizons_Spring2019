using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class scr_ExiledArcher : scr_EntityAI {


    public AttackData hunterShot;
    public float hSChargeTime;
    private bool hSOnCD = false;   //On Cooldown 
    private float hSCooldownTime = 1.5f; 



    public AttackData arrowRain;
    public float aRInterval;
    public float movementIntervalLower;
    public float movementIntervalUpper;
    public float dodgeChance; 
    private bool canArrowRain = true;
    private bool broken = false;
    private bool canMove = true; 

    AudioSource Attack_SFX;
    public AudioClip[] attacks_SFX;
    private AudioClip attack_SFX;

    private void Start()
    {
        AudioSource[] SFX_Sources = GetComponents<AudioSource>();
        Attack_SFX = SFX_Sources[1];
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
        if(!hSOnCD  && HunterShotCheck())
        {
            StartCoroutine(HunterShot());
        }
        if (canMove)
        {
            StartCoroutine(MovementClock());
        }


        /*
        if (canArrowRain)
        {
            StartCoroutine(ArrowRain(aRInterval)); 
        }
        */

    }

    public override void Die()
    {
        entity.Death();
    }

    bool HunterShotCheck()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        int playerY = player.GetComponent<scr_Entity>()._gridPos.y;
        if(entity._gridPos.y == playerY)
        {
            return true;
        }
        return false; 
    }

    void StartHunterShot()
    {
        scr_AttackController.attackController.AddNewAttack(hunterShot, entity._gridPos.x, entity._gridPos.y, entity);
    }

    private IEnumerator HunterShot()
    {
        hSOnCD = true;
        yield return new WaitForSecondsRealtime(hSChargeTime);
        int index = Random.Range(0, attacks_SFX.Length);
        attack_SFX = attacks_SFX[index];
        Attack_SFX.clip = attack_SFX;
        Attack_SFX.Play();
        anim.SetBool("Attack", true);
        //scr_AttackController.attackController.AddNewAttack(hunterShot, entity._gridPos.x, entity._gridPos.y, entity);
        yield return new WaitForSecondsRealtime(hSCooldownTime);
        hSOnCD = false; 
    }

    private IEnumerator ArrowRain(float _aRInterval)
    {
        //TELEGRAPH 
        canArrowRain = false; 
        yield return new WaitForSecondsRealtime(1f);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        int playerXPos = player.GetComponent<scr_Entity>()._gridPos.x;
        scr_AttackController.attackController.AddNewAttack(arrowRain, playerXPos, scr_Grid.GridController.rowSizeMax - 1, entity);
        yield return new WaitForSecondsRealtime(_aRInterval);
        canArrowRain = true; 
    }


    int PickXCoord()
    {
        //must return int 
        int _range = scr_Grid.GridController.columnSizeMax;
        int _currPosX = entity._gridPos.x;

        if (_currPosX == _range - 1)
        {
            return (_currPosX - 1);
        }
        else if (_currPosX == _range / 2)
        {
            return _currPosX + 1;
        }
        else
        {
            int _temp = Random.Range(0, 2);
            if (_temp == 0)
            {
                return _currPosX + 1;
            }
            else if (_temp == 1)
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

    void Movement()
    {


        //Decide if we are moving horiz or vert.
        int _temp = Random.Range(0, 2);                                         //Pick a number between 0 and 1
        int _x = entity._gridPos.x;
        int _y = entity._gridPos.y;
        int _tries = 0;



        while (_tries < 10)
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
    }

    IEnumerator MovementClock()
    {
        if (canMove)
        {
            Movement();
            float _movementInterval = Random.Range(movementIntervalLower, movementIntervalUpper);
            canMove = false; 
            yield return new WaitForSecondsRealtime(_movementInterval);
            canMove = true; 
        }
    }

    void DodgeAttackVertically(int x, int y)
    {
        int _y = y + 1;
        
        if(_y > scr_Grid.GridController.rowSizeMax - 1)
        {
            _y = y - 2; 
        }

        entity.SetTransform(x, _y);
    }
}
