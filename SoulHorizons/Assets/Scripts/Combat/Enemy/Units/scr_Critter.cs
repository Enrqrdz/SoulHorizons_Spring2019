using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class scr_Critter : scr_EntityAI
{
    public float movementIntervalLower = .75f;
    public float movementIntervalUpper = 1.5f;
    public float decisionTime;
    public float decisionTimeLower;
    //public float burrowedTime;
    // public float burrowedTimeLower;
    bool leftOrRight = false; //false left, true right
    bool taskComplete = true;
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
        /*xPos = GenerateCoord(scr_Grid.GridController.columnSizeMax / 2, scr_Grid.GridController.columnSizeMax);
        //xCoord = PickXCoord();

        if (xPos == entity._gridPos.x && yPos == entity._gridPos.y)
        {
        //We picked the spot we are on, do the check again 
         Move();
         return;
        }
        else
        {
            while (tries < 10)
            {
                if (!scr_Grid.GridController.CheckIfOccupied(xPos, yPos) && (scr_Grid.GridController.ReturnTerritory(xPos, yPos).name == entity.entityTerritory.name))
                {
                    //if the tile is not occupied
                    scr_Grid.GridController.SetTileOccupied(true, xPos, yPos, entity);          //set it to be occupied  
                    entity.SetTransform(xPos, yPos);
                    return;
                }
                else
                {
                    //it is occupied, perform the check again
                    tries++;
                    if(tries >= 10)
                    {
                        MoveAlongRow(xPos, yPos);
                        state = 1;
                    }


                }
            }
        }*/
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

    int GenerateCoord(int lowerLim, int upperLim)
    {
        int x = Random.Range(lowerLim, upperLim);
        return x;
    }

    int PickXCoord( int xPos)
    {
        //must return int 
        int moveRange = scr_Grid.GridController.columnSizeMax;
        int currX = entity._gridPos.x;

        if (currX == moveRange - 1)
        {
            return (currX - 1);
        }
        else if (currX == 0)
        {
            return currX + 1;
        }
        else
        {
            int temp = Random.Range(0, 2);
            if (temp == 0)
            {
                return currX + 1;
            }
            else if (temp == 1)
            {
                return currX - 1;
            }

            return 0;
        }

    }

    int PickYCoord(int yPos)
    {
        int currY = entity._gridPos.y;
        int moveRange = scr_Grid.GridController.rowSizeMax;
        int rand = Random.Range(0, 2);

        if(rand == 0)
        {
            return yPos;
        }
        else
        {
            return yPos;
        }
        
    }

    void MoveAlongRow(int xPos, int yPos)
    {
        int xRange = scr_Grid.GridController.columnSizeMax;
        int random = Random.Range(0, 2);

        
    } 

    IEnumerator Brain()
    {
        switch (state)
        {
            case 0:                                                             //Move the entity's x position,
                taskComplete = false;
                Move();                                                    
                state = 2;                                         
                taskComplete = true;
                break;

            case 1:                                                             //On a tile, waiting to do a thing
                taskComplete = false;
                float waitTime;
                waitTime = Random.Range(decisionTimeLower, decisionTime);
                yield return new WaitForSecondsRealtime(waitTime);
                state = 0;                                                                                             
                taskComplete = true;
                break;

            case 2:
                taskComplete = false;
                float waitTimeAgain;
                waitTimeAgain = Random.Range(decisionTimeLower, decisionTime);
                yield return new WaitForSecondsRealtime(waitTimeAgain);
                state = 3;                                                                                           
                taskComplete = true;
                break;

            case 3: //Move on row case
                taskComplete = false;
                int startPos = entity._gridPos.x;
                int xRange = scr_Grid.GridController.columnSizeMax;
                int random = Random.Range(0, 2);
                if(random == 0)
                {
                    leftOrRight = false;
                }
                else
                {
                    leftOrRight = true;
                }

                for (int i = 0; i < xRange; i++) // Make the function increment through the whole row and shit
                {
                    MoveAlongRow(entity._gridPos.x, entity._gridPos.y);
                    float moveInterval = Random.Range(movementIntervalLower, movementIntervalUpper);
                    yield return new WaitForSecondsRealtime(moveInterval);
                }
                state = 1;
                taskComplete = true;
                break;

            case 4: //Stunned Case
                waitTimeAgain = Random.Range(decisionTimeLower, decisionTime);
                yield return new WaitForSecondsRealtime(waitTimeAgain);
                break;

        }
    }

}

