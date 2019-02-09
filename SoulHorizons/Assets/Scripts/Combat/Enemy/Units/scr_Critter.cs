using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class scr_Critter : scr_EntityAI
{
    public float decisionTime;
    public float decisionTimeLower;
    //public float burrowedTime;
   // public float burrowedTimeLower;
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

        xCoord = GenerateCoord(scr_Grid.GridController.maxColumnSize / 2, scr_Grid.GridController.maxColumnSize);
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
                if (!scr_Grid.GridController.CheckIfOccupied(xCoord, yCoord) && (scr_Grid.GridController.ReturnTerritory(xCoord, yCoord).name == entity.entityTerritory.name))
                {
                    //if the tile is not occupied
                    scr_Grid.GridController.SetTileOccupied(true, xCoord, yCoord, entity);          //set it to be occupied  
                    entity.SetTransform(xCoord, yCoord);
                    MoveAlongColumn(xCoord, yCoord);
                    return;
                }
                else
                {
                    //it is occupied, perform the check again
                    tries++;
                    if(tries >= 10)
                    {
                        MoveAlongColumn(xCoord, yCoord);
                    }


                }
            }
        }
    }

    public override void UpdateAI()
    {
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
        int moveRange = scr_Grid.GridController.maxColumnSize;
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
        int moveRange = scr_Grid.GridController.maxRowSize;

        if (currY == 0)   //AI is on y = 0 and can only move to 1 (down)                             
        {
            return 2;
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
        if (!scr_Grid.GridController.CheckIfOccupied(xCoord, yCoord) && (scr_Grid.GridController.ReturnTerritory(xCoord, yCoord).name == entity.entityTerritory.name))
        {
            //if the tile is not occupied   
            scr_Grid.GridController.SetTileOccupied(true, xCoord, yCoord, entity);          //set it to be occupied  
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
                state = 1;                                         
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

        }
    }

}

/* case 0:                                                             //Move the entity,
               entity.spr.color = burrowedColor;
               taskComplete = false;
               Move();                                                     //Set new position
               float burrowTime;
               burrowTime = Random.Range(burrowedTimeLower, burrowedTime);
               yield return new WaitForSecondsRealtime(burrowTime);          //in case we want him to be hidden/burrowed for an amount of time
               state = 3;                                                      //go to un-burrowing
               entity.spr.color = burrowedColor;
               taskComplete = true;
               break;
           case 1:                                                             //On a tile, waiting to do a thing
               entity.invincible = false;
               taskComplete = false;
               float waitTime;
               waitTime = Random.Range(decisionTimeLower, decisionTime);
               yield return new WaitForSecondsRealtime(waitTime);
               state = 2;                                                      //go to burrowing                                            
               taskComplete = true;
               break;
           case 2:                                                             //burrows and is hidden
               t = 0;
               taskComplete = false;
               while (t < 1)
               { // while t below the end limit...
                 // increment it at the desired rate every update:
                   entity.spr.color = Color.Lerp(normalColor, burrowedColor, t);   //fades the color of the sprite to black, this is a placeholder for a burrowing animation
                   t += Time.deltaTime / lerpDuration;
                   yield return new WaitForSecondsRealtime(.001f);
               }
               entity.invincible = true;                                      //give the entity i frames
               state = 0;                                                      //go to movement 
               taskComplete = true;
               break;
           case 3:                                                             //pops out of burrow in new tile
               t2 = 0;
               taskComplete = false;
               while (t2 < 1)
               { // while t below the end limit...
                 // increment it at the desired rate every update:
                   entity.spr.color = Color.Lerp(burrowedColor, normalColor, t2);   //fades the color of the sprite to black, this is a placeholder for a burrowing animation
                   t2 += Time.deltaTime / lerpDuration;
                   yield return new WaitForSecondsRealtime(.001f);
               }
               entity.invincible = false;                                     //make the entity mortal again
               state = 1;                                                      //go to waiting 
               taskComplete = true;
               break; */
//moveAlongColumn
/*int moveRange = scr_Grid.GridController.rowSizeMax;
    int newCoord;
    int temp;         //Pick a number between 0 and 1
    temp = Random.Range(0, 2);
    if (temp == 0)            //if that number == 0, then we're moving up
    {
        if (yCoord != 0)
        {
             newCoord = 0;
            if (!scr_Grid.GridController.CheckIfOccupied(xCoord, yCoord) && (scr_Grid.GridController.ReturnTerritory(xCoord, yCoord).name == entity.entityTerritory.name))
            {
                //if the tile is not occupied
                scr_Grid.GridController.SetTileOccupied(true, xCoord, yCoord, entity);          //set it to be occupied     
                entity.SetTransform(xCoord, newCoord);
                return;
            }
            else
            {
                //it is occupied, perform the check again
                MoveAlongColumn(xCoord, yCoord);
                return;

            }
        }
    }
    else if (temp == 1) //the number is 1 so we are moving down
    {
        if (yCoord != moveRange - 1)
        {
            newCoord = moveRange - 1;
            if (!scr_Grid.GridController.CheckIfOccupied(xCoord, yCoord) && (scr_Grid.GridController.ReturnTerritory(xCoord, yCoord).name == entity.entityTerritory.name))
            {
                //if the tile is not occupied
                scr_Grid.GridController.SetTileOccupied(true, xCoord, yCoord, entity);          //set it to be occupied     
                entity.SetTransform(xCoord, newCoord);
                return;
            }
            else
            {
                //it is occupied, perform the check again
                MoveAlongColumn(xCoord, yCoord);
                return;
            }
        }
    } */
