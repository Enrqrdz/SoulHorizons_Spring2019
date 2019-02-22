using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class scr_Critter : EntityAI
{
    public float decisionTime;
    public float decisionTimeLower;
    //public float burrowedTime;
    // public float burrowedTimeLower;
    bool movingDown = false;
    bool movingUp = false;
    bool taskComplete = true;
    int state = 1;

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

        int xCoord = entity._gridPos.x;
        int yCoord = entity._gridPos.y;
        int tries = 0;

        xCoord = GenerateCoord(Grid.Instance.columnSizeMax / 2, Grid.Instance.columnSizeMax);
        //xCoord = PickXCoord();

        if (xCoord == entity._gridPos.x && yCoord == entity._gridPos.y)
        {
        //We picked the spot we are on, do the check again 
         Move();
         return;
        }
        else
        {
            while (tries < 10)
            {
                if (!Grid.Instance.CheckIfOccupied(xCoord, yCoord) && (Grid.Instance.ReturnTerritory(xCoord, yCoord).name == entity.entityTerritory.name))
                {
                    //if the tile is not occupied
                    Grid.Instance.SetTileOccupied(true, xCoord, yCoord, entity);          //set it to be occupied  
                    entity.SetTransform(xCoord, yCoord);
                    return;
                }
                else
                {
                    //it is occupied, perform the check again
                    tries++;
                    if(tries >= 10)
                    {
                        MoveAlongColumn(xCoord, yCoord);
                        state = 1;
                    }


                }
            }
        }
    }

    public override void UpdateAI()
    {
        Grid.Instance.SetTileOccupied(true, entity._gridPos.x, entity._gridPos.y, this.entity);
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

    int PickXCoord()
    {
        //must return int 
        int moveRange = Grid.Instance.columnSizeMax;
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

    int PickYCoord()
    {
        int currY = entity._gridPos.y;
        int moveRange = Grid.Instance.rowSizeMax;

        if (currY == 0)   //AI is on y = 0 and can only move to 1 (down)                             
        {
            return moveRange - 1;
        }
        else if (currY == 2)    //the AI is on the bottom and can only move to up
        {
    
            return 0;
        }
        else    //otherwise, the AI is on 2 and can only move to 1 (up)
        {
            int temp = Random.Range(0, 2);  //make a random number 0 or 1
            if (temp == 0)  //if this number is 0, move up
            {
                return 0;
            }
            else     //if this number is 1, move to down
            {
                return 2;
            }
        }
    }

    void MoveAlongColumn(int xCoord, int yCoord)
    {
        yCoord = PickYCoord();
        if (!Grid.Instance.CheckIfOccupied(xCoord, yCoord) && (Grid.Instance.ReturnTerritory(xCoord, yCoord).name == entity.entityTerritory.name))
        {
            //if the tile is not occupied   
            Grid.Instance.SetTileOccupied(true, xCoord, yCoord, entity);          //set it to be occupied  
            entity.SetTransform(xCoord, yCoord);
            return;
        }
        else
        {
            //it is occupied, perform the check again
            Move();
            return;

        }
    } 

    IEnumerator Brain()
    {
        switch (state)
        {
            case 0:                                                             //Move the entity's x position,
                taskComplete = false;
                Move();                                                     //Set new position
                state = 2;                                         
                taskComplete = true;
                break;
            case 1:                                                             //On a tile, waiting to do a thing
                taskComplete = false;
                float waitTime;
                waitTime = Random.Range(decisionTimeLower, decisionTime);
                yield return new WaitForSecondsRealtime(waitTime);
                state = 0;                                                   //move                                           
                taskComplete = true;
                break;
            case 2:
                taskComplete = false;
                float waitTimeAgain;
                waitTimeAgain = Random.Range(decisionTimeLower, decisionTime);
                yield return new WaitForSecondsRealtime(waitTimeAgain);
                state = 3;                                                   //move                                           
                taskComplete = true;
                break;
            case 3:
                taskComplete = false;
                MoveAlongColumn(entity._gridPos.x, entity._gridPos.y);
                state = 1;
                taskComplete = true;
                break;

        }
    }

}

