using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class scr_Critter : scr_EntityAI
{
    public float movementInterval = .75f;
    public float attackInterval = .25f;
    public AttackData Scratch;
    bool leftOrRight = false; //false left, true right
    bool taskComplete = true;
    bool isStuck = false;
    bool canAttack = false;
    int state = 0;
    int attempts = 0;


    AudioSource Footsteps_SFX;
    public AudioClip[] movements_SFX;
    private AudioClip movement_SFX;

    private void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
    }

    public override void Move()
    {
        AudioSource[] SFX_Sources = GetComponents<AudioSource>();
        Footsteps_SFX = SFX_Sources[0];
        int index = Random.Range(0, movements_SFX.Length);
        movement_SFX = movements_SFX[index];
        Footsteps_SFX.clip = movement_SFX;
        Footsteps_SFX.Play();
        int xPos = entity._gridPos.x;
        int yPos = entity._gridPos.y;


        while (attempts < 20)
        {
            yPos = PickYCoord(yPos);
            try
            {
                if (!scr_Grid.GridController.CheckIfOccupied(xPos, yPos) && (scr_Grid.GridController.ReturnTerritory(xPos, yPos).name == entity.entityTerritory.name))
                {
                    //if the tile is not occupied
                    entity.SetTransform(xPos, yPos);
                    return;
                }
                else
                {
                    attempts++;
                    Move();
                }
            }
            catch
            {
                attempts++;
                Move();
            }
        }
        return;
    }

    public override void UpdateAI()
    {
        scr_Grid.GridController.SetTileOccupied(true, entity._gridPos.x, entity._gridPos.y, this.entity);
        if (taskComplete == true)
        {
            StartCoroutine(Brain());
        }
        if(canAttack == true)
        {
            StartCoroutine(ScratchAttack(attackInterval));
            canAttack = false;
        }
        
    }

    public override void Die()
    {
        entity.Death();
    }

    int PickYCoord(int yPos)
    {
        int moveRange = scr_Grid.GridController.rowSizeMax;
        int rand = Random.Range(0, 2);

        if (rand == 0)
        {
            int tempVar = yPos - 1;
            if (tempVar >= 0)
            {
                return yPos - 1;
            }
            else
            {
                return yPos + 1;
            }
        }
        else
        {
            int tempVar = yPos + 1;
            if (tempVar < moveRange)
            {
                return yPos + 1;
            }
            else
            {
                return yPos - 1;
            }
        }

    }

    void MoveAlongRow(int xPos, int yPos, int xLimit, bool direction)
    {
        AudioSource[] SFX_Sources = GetComponents<AudioSource>();
        Footsteps_SFX = SFX_Sources[0];
        int index = Random.Range(0, movements_SFX.Length);
        movement_SFX = movements_SFX[index];
        Footsteps_SFX.clip = movement_SFX;
        Footsteps_SFX.Play();
        int xRange = scr_Grid.GridController.columnSizeMax;
        try
        {
            while (attempts < 20)
            {
                if (!direction)
                {
                    xPos--;
                    if (!scr_Grid.GridController.CheckIfOccupied(xPos, yPos) && (scr_Grid.GridController.ReturnTerritory(xPos, yPos).name == entity.entityTerritory.name) && xPos > xLimit)
                    {
                        //if the tile is not occupied
                        entity.SetTransform(xPos, yPos);
                        return;
                    }
                    else
                    {
                        attempts++;
                        if(attempts >= 20)
                        {
                            isStuck = true;
                        }
                        return;
                    }
                }
                else
                {
                    xPos++;
                    if (!scr_Grid.GridController.CheckIfOccupied(xPos, yPos) && (scr_Grid.GridController.ReturnTerritory(xPos, yPos).name == entity.entityTerritory.name) && xPos < xRange)
                    {
                        //if the tile is not occupied
                        entity.SetTransform(xPos, yPos);
                        return;
                    }
                    else
                    {
                        attempts++;
                        if (attempts >= 20)
                        {
                            isStuck = true;
                        }
                        return;
                    }
                }
            }

        }
        catch
        {
            isStuck = true;
            return;
        }
    }

    int GetXLimit(int xPos)
    {
        int xRange = scr_Grid.GridController.columnSizeMax;
        int xLimit = xPos;
        int tempX = xPos;
        for (int i = 0; i < xRange; i++)
        {
            tempX--;
            if (scr_Grid.GridController.grid[xLimit, entity._gridPos.y].territory.name != TerrName.Player)
            {
                xLimit = tempX;
            }
            else
            {
                return xLimit;
            }
        }
        return xLimit;
    }

    IEnumerator ScratchAttack (float attackInterval)
    {
        PrimeAttackTiles(Scratch, entity._gridPos.x - 1, entity._gridPos.y);
        AttackController.Instance.AddNewAttack(Scratch, entity._gridPos.x - 1, entity._gridPos.y, entity);
        yield return new WaitForSecondsRealtime(attackInterval);      

    }

    bool CheckAbleToAttack ()
    {
        if(scr_Grid.GridController.ReturnTerritory(entity._gridPos.x - 1, entity._gridPos.y).name == TerrName.Player)
        {
            return true;
        }
        return false;
    }

    IEnumerator Brain()
    {
        switch (state)
        {
            case 0:                                  //Move to a new Row
                taskComplete = false;
                attempts = 0;
                Move();
                state = 1;
                yield return new WaitForSecondsRealtime(movementInterval);
                canAttack = CheckAbleToAttack();
                taskComplete = true;
                
                break;

            case 1: //Move along the row
                taskComplete = false;
                attempts = 0;
                int startPos = entity._gridPos.x;
                int xRange = scr_Grid.GridController.columnSizeMax;
                int xLimit = GetXLimit(startPos);
                int random = Random.Range(0, 3);
                
                if (random == 1)
                {
                    leftOrRight = false;
                }
                else
                {
                    leftOrRight = true;
                }
                for (int i = 0; i < xRange; i++) //moves along the row either left or right then 
                {
                    MoveAlongRow(entity._gridPos.x, entity._gridPos.y, xLimit, leftOrRight);
                    yield return new WaitForSecondsRealtime(movementInterval);
                    if (entity._gridPos.x == startPos)
                    {
                        leftOrRight = !leftOrRight;
                        break;
                    }
                    if(isStuck)
                    {
                        break;
                    }
                }
                canAttack = CheckAbleToAttack();
                for (int i = 0; i < xRange; i++)
                {
                    MoveAlongRow(entity._gridPos.x, entity._gridPos.y, xLimit, leftOrRight);
                    yield return new WaitForSecondsRealtime(movementInterval);
                    if (entity._gridPos.x == startPos)
                    {
                        leftOrRight = !leftOrRight;
                        break;
                    }
                    if (isStuck)
                    {
                        break;
                    }
                }
                state = 0;
                taskComplete = true;
                break;
        }
    }
}

