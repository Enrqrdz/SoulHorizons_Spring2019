using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class scr_ExiledArcher : scr_EntityAI {

    //AKA Bird Bow Boi
    //TODO: Antonio: Make it so the archer moves in a clockwise motion diagonally. 
    // Make it so its arrows can come from either straght in front of him, or one tile below or above

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
    private bool canMove = true;
    private bool goBackwards = false;
    private int movePosition = 0;

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
        int xPos = entity._gridPos.x;
        int yPos = entity._gridPos.y;
        int xRange = scr_Grid.GridController.columnSizeMax;
        int yRange = scr_Grid.GridController.rowSizeMax;
        int tries = 0;

        while (tries < 10)
        {

            yPos = PickYCoord(yPos, movePosition, goBackwards);
            xPos = PickXCoord(xPos, movePosition, goBackwards);

            if (!scr_Grid.GridController.CheckIfOccupied(xPos, yPos) && (scr_Grid.GridController.ReturnTerritory(xPos, yPos).name == entity.entityTerritory.name))
            {
                Debug.Log("LETS GET MOVING");
                entity.SetTransform(xPos, yPos);   //move to new position
                if (movePosition < 3)
                {
                    movePosition++;
                    Debug.Log("Position: " + movePosition);
                }
                else
                {
                    movePosition = 0;
                }

                return;
            }
            else
            {
                tries++;
                goBackwards = true;
                Debug.Log("goin backwards");
                if (tries >= 10)
                {
                    entity.SetTransform(scr_Grid.GridController.columnSizeMax / 2, scr_Grid.GridController.rowSizeMax / 2);
                    tries = 0;
                }
            }
        }
            /*//Decide if we are moving horiz or vert.
            int randomDirection = Random.Range(0, 2);                                         //Pick a number between 0 and 1
            int xPos = entity._gridPos.x;
            int yPos = entity._gridPos.y;
            int tries = 0;

            while (tries < 10)
            {
                randomDirection = Random.Range(0, 2);
                if (randomDirection == 0)                                                          //if that number == 0, then we're moving vertically 
                {
                    yPos = PickYCoord();

                }
                else if (randomDirection == 1)                                                     //if that number == 1, we're moving horizonally 
                {
                    xPos = PickXCoord();

                }

                if (!scr_Grid.GridController.CheckIfOccupied(xPos, yPos) && (scr_Grid.GridController.ReturnTerritory(xPos, yPos).name == entity.entityTerritory.name))
                {
                    entity.SetTransform(xPos, yPos);   //move to new position
                    return;
                }
                else
                {
                    tries++;
                    if (tries >= 10)
                    {
                        broken = true;
                        Debug.Log("I think I am broken");
                    }
                }
            } */
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
        int randomVal;
        randomVal = Random.Range(0, 6); //The arrow has a 3/5 chance to come out straight, and a 1/5 chance to come out either one tile below or above the archer
        if (randomVal == 0)
        {
            scr_AttackController.attackController.AddNewAttack(hunterShot, entity._gridPos.x, entity._gridPos.y + 1, entity);
        }
        else if (randomVal == 5)
        {
            scr_AttackController.attackController.AddNewAttack(hunterShot, entity._gridPos.x, entity._gridPos.y - 1, entity);
        }
        else
        {
            scr_AttackController.attackController.AddNewAttack(hunterShot, entity._gridPos.x, entity._gridPos.y, entity);
        }
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


    int PickXCoord(int xPos, int movePosition, bool backwards)
    {
        if (movePosition == 0)
        {
            if (!backwards)
            {
                return xPos - 1;
            }
            else
            {
                return xPos + 1;
            }
        }
        else if (movePosition == 1)
        {
            if (!backwards)
            {
                return xPos - 1;
            }
            else
            {
                return xPos + 1;
            }
        }
        else if (movePosition == 2)
        {
            if (!backwards)
            {
                return xPos + 1;
            }
            else
            {
                return xPos - 1;
            }
        }
        else
        {
            if (!backwards)
            {
                return xPos + 1;
            }
            else
            {
                return xPos - 1;
            }
        }
        /*//must return int 
        int _range = scr_Grid.GridController.columnSizeMax;
        int _currPosX = entity._gridPos.x;
        int range = scr_Grid.GridController.rowSizeMax;
        int curPosX = entity._gridPos.x;

        if (curPosX == range - 1)
        {
            return (curPosX - 1);
        }
        else if (curPosX == range / 2)
        {
            return curPosX + 1;
        }
        else
        {
            int temp = Random.Range(0, 2);
            if (temp == 0)
            {
                return curPosX + 1;
            }
            else if (temp == 1)
            {
                return curPosX - 1;
            }

            return 0;
        } */

    }

    int PickYCoord(int yPos, int movePosition, bool backwards)
    {

        if (movePosition == 0)
        {
            if (!backwards)
            {
                return yPos + 1;
            }
            else
            {
                return yPos + 1;
            }
        }
        else if (movePosition == 1)
        {
            if (!backwards)
            {
                return yPos - 1;
            }
            else
            {
                return yPos - 1;
            }
        }
        else if (movePosition == 2)
        {
            if (!backwards)
            {
                return yPos - 1;
            }
            else
            {
                return yPos - 1;
            }
        }
        else
        {
            if (!backwards)
            {
                return yPos - 1;
            }
            else
            {
                return yPos + 1;
            }
        }
        /*if (entity._gridPos.y == 0)                             //AI is on y = 0 and can only move to 1 (down)                             
        {
            return 1;
        }
        else if (entity._gridPos.y == 1)                        //AI is on y = 1 and can move either up or down
        {
            int temp = Random.Range(0, 2);             //make a random number 0 or 1
            if (temp == 0)                              //if this number is 0, move to 0 (up)
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
        }*/
    }

    IEnumerator MovementClock()
    {
        if (canMove)
        {
            Move();
            float _movementInterval = Random.Range(movementIntervalLower, movementIntervalUpper);
            canMove = false; 
            yield return new WaitForSecondsRealtime(_movementInterval);
            canMove = true; 
        }
    }

    void DodgeAttackVertically(int x, int y)
    {
        int yDirection = y + 1;
        
        if(yDirection > scr_Grid.GridController.rowSizeMax - 1)
        if(yDirection > scr_Grid.GridController.columnSizeMax - 1)
        {
            yDirection = y - 2; 
        }

        entity.SetTransform(x, yDirection);
    }
}
