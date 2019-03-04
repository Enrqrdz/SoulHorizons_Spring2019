using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class scr_Critter : scr_EntityAI
{
    public float movementIntervalLower = .75f;
    public float movementIntervalUpper = 1.5f;
    bool leftOrRight = false; //false left, true right
    bool taskComplete = true;
    bool isStuck = false;
    int state = 0;


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

        try
        {
            yPos = PickYCoord(yPos);
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
            yPos = PickYCoord(yPos);
            if (!scr_Grid.GridController.CheckIfOccupied(xPos, yPos) && (scr_Grid.GridController.ReturnTerritory(xPos, yPos).name == entity.entityTerritory.name))
            {
                //if the tile is not occupied
                scr_Grid.GridController.SetTileOccupied(true, xPos, yPos, entity);          //set it to be occupied  
                entity.SetTransform(xPos, yPos);
                return;
            }
        }
    }

    public override void UpdateAI()
    {
        scr_Grid.GridController.SetTileOccupied(true, entity._gridPos.x, entity._gridPos.y, this.entity);
        if (taskComplete)
        {
            StartCoroutine(Brain());
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

        if (!direction)
        {
            xPos--;
            if (!scr_Grid.GridController.CheckIfOccupied(xPos, yPos) && (scr_Grid.GridController.ReturnTerritory(xPos, yPos).name == entity.entityTerritory.name) && xPos > xLimit)
            {
                //if the tile is not occupied
                scr_Grid.GridController.SetTileOccupied(true, xPos, yPos, entity);          //set it to be occupied  
                entity.SetTransform(xPos, yPos);
                return;
            }
            else
            {
                isStuck = true;
                return;
            }
        }
        else
        {
            xPos++;
            if (!scr_Grid.GridController.CheckIfOccupied(xPos, yPos) && (scr_Grid.GridController.ReturnTerritory(xPos, yPos).name == entity.entityTerritory.name) && xPos < xRange)
            {
                //if the tile is not occupied
                scr_Grid.GridController.SetTileOccupied(true, xPos, yPos, entity);          //set it to be occupied  
                entity.SetTransform(xPos, yPos);
                return;
            }
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

    IEnumerator Brain()
    {
        switch (state)
        {
            case 0:                                  //Move to a new Row
                taskComplete = false;
                Move();
                state = 2;
                float moveInterval = Random.Range(movementIntervalLower, movementIntervalUpper);
                yield return new WaitForSecondsRealtime(moveInterval);
                taskComplete = true;
                break;

            case 2: //Move along the row
                taskComplete = false;
                int startPos = entity._gridPos.x;
                int xRange = scr_Grid.GridController.columnSizeMax-1;
                int xLimit = GetXLimit(startPos);
                int random = Random.Range(0, 2);
                if (random == 0)
                {
                    leftOrRight = false;
                }
                else
                {
                    leftOrRight = true;
                }
                for (int i = 0; i < xRange; i++) //moves along the row either left or right then 
                {
                    try
                    {
                        MoveAlongRow(entity._gridPos.x, entity._gridPos.y, xLimit, leftOrRight);
                    }
                    catch
                    {
                        MoveAlongRow(entity._gridPos.x, entity._gridPos.y, xLimit, !leftOrRight);
                        leftOrRight = !leftOrRight;
                    }
                    float moveInterval2 = Random.Range(movementIntervalLower, movementIntervalUpper);
                    yield return new WaitForSecondsRealtime(moveInterval2);
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
                for (int i = 0; i < xRange; i++)
                {
                    try
                    {
                        MoveAlongRow(entity._gridPos.x, entity._gridPos.y, xLimit, leftOrRight);
                    }
                    catch
                    {
                        MoveAlongRow(entity._gridPos.x, entity._gridPos.y, xLimit, !leftOrRight);
                        leftOrRight = !leftOrRight;
                    }
                    float moveInterval2 = Random.Range(movementIntervalLower, movementIntervalUpper);
                    yield return new WaitForSecondsRealtime(moveInterval2);
                    if (entity._gridPos.x == startPos)
                    {
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

