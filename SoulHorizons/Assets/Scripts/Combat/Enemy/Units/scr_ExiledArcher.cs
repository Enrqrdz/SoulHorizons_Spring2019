using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class scr_ExiledArcher : scr_EntityAI {

    //AKA Bird Bow Boi

    public AttackData hunterShot;
    public float hSChargeTime;
    private bool hSOnCD = false;   //On Cooldown 
    private float hSCooldownTime = 1.5f; 

    public AttackData arrowRain;
    public float aRInterval;
    public float movementIntervalLower;
    public float movementIntervalUpper;
    public float stunTime = .5f;

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

        try
        {
            yPos = PickYCoord(yPos, movePosition, goBackwards);
            xPos = PickXCoord(xPos, movePosition, goBackwards);
            if (!scr_Grid.GridController.CheckIfOccupied(xPos, yPos) && (scr_Grid.GridController.ReturnTerritory(xPos, yPos).name == entity.entityTerritory.name))
            {
                entity.SetTransform(xPos, yPos); 
                if (movePosition < 3)
                {
                    movePosition++;
                }
                else
                {
                    movePosition = 0;
                }
                return;
            }
            else
            {
                goBackwards = !goBackwards;
                yPos = PickYCoord(yPos, movePosition, goBackwards);
                xPos = PickXCoord(xPos, movePosition, goBackwards);
                if (!scr_Grid.GridController.CheckIfOccupied(xPos, yPos) && (scr_Grid.GridController.ReturnTerritory(xPos, yPos).name == entity.entityTerritory.name))
                {
                    entity.SetTransform(xPos, yPos);   //move to new position
                    if (movePosition < 3)
                    {
                        movePosition++;
                    }
                    else
                    {
                        movePosition = 0;
                    }
                    return;
                }
            }

        }
        catch //If the new position is out of range of the grid, move the archer to the middle of the back column
        {
            yPos = yRange / 2;
            xPos = xRange - 1;
            if (!scr_Grid.GridController.CheckIfOccupied(xPos, yPos) && (scr_Grid.GridController.ReturnTerritory(xPos, yPos).name == entity.entityTerritory.name))
            {
                entity.SetTransform(xPos, yPos);   //move to new position
                movePosition = 0;
                return;
            }
        }
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
            if (backwards == false)
            {
                return xPos - 1;
            }
            else
            {
                return xPos - 1;
            }
        }
        else if (movePosition == 1)
        {
            if (backwards == false)
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
            if (backwards == false)
            {
                return xPos + 1;
            }
            else
            {
                return xPos + 1;
            }
        }
        else
        {
            if (backwards == false)
            {
                return xPos + 1;
            }
            else
            {
                return xPos + 1;
            }
        }

    }

    int PickYCoord(int yPos, int movePosition, bool backwards)
    {

        if (movePosition == 0)
        {
            if (backwards == false)
            {
                return yPos + 1;
            }
            else
            {
                return yPos - 1;
            }
        }
        else if (movePosition == 1)
        {
            if (backwards == false)
            {
                return yPos - 1;
            }
            else
            {
                return yPos + 1;
            }
        }
        else if (movePosition == 2)
        {
            if (backwards == false)
            {
                return yPos + 1;
            }
            else
            {
                return yPos + 1;
            }
        }
        else
        {
            if (backwards == false)
            {
                return yPos - 1;
            }
            else
            {
                return yPos - 1;
            }
        }
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
}
