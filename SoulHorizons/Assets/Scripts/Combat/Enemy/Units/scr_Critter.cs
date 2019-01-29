using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class scr_Critter : scr_EntityAI {




    public float decisionTime;
    public float decisionTimeLower; 
    public float burrowedTime;
    public float burrowedTimeLower; 
    public Color burrowedColor;
    public Color normalColor; 
    bool taskComplete = true; 
    int state = 1;
    float t = 0;
    float t2 = 0;
    float lerpDuration = .5f;
    Color tempColor;

    AudioSource Footsteps_SFX;
    public AudioClip[] movements_SFX;
    private AudioClip movement_SFX;

    public override void Move()
    {

        

    }
    public override void Attack()
    {
        
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
        Debug.Log("ARGHH");
        entity.Death();
    }




    int GenerateCoord(int lowerLim,int upperLim)
    {
        int _x = Random.Range(lowerLim, upperLim);
        return _x; 
    }





    private void Movement()
    {
        AudioSource[] SFX_Sources = GetComponents<AudioSource>();
        Footsteps_SFX = SFX_Sources[0];
        int index = Random.Range(0, movements_SFX.Length);
        movement_SFX = movements_SFX[index];
        Footsteps_SFX.clip = movement_SFX;
        Footsteps_SFX.Play();
        int _x = GenerateCoord(scr_Grid.GridController.columnSizeMax/2, scr_Grid.GridController.columnSizeMax);
        int _y = GenerateCoord(0, scr_Grid.GridController.rowSizeMax);

        if(_x == entity._gridPos.x  &&  _y == entity._gridPos.y)
        {
            //We picked the spot we are on, do the check again 
            Movement();
            return; 
 
        }
        else
        {
            
            if (!scr_Grid.GridController.CheckIfOccupied(_x, _y) && (scr_Grid.GridController.ReturnTerritory(_x, _y).name == entity.entityTerritory.name))                       //if the tile is not occupied 
            {
                scr_Grid.GridController.SetTileOccupied(true, _x, _y, entity);          //set it to be occupied 
                entity.SetTransform(_x, _y);                                            //move here 
                return;

            }
            else
            {
                //it is occupied, perform the check again
                Movement();
                return;
             
            }

                 
        }

        
    }


    IEnumerator Brain()
    {
        switch (state)
        {
            case 0:                                                             //Move the entity,
                entity.spr.color = burrowedColor;
                taskComplete = false; 
                Movement();                                                     //Set new position
                float _thatTime;
                _thatTime = Random.Range(burrowedTimeLower, burrowedTime);
                yield return new WaitForSecondsRealtime(_thatTime);          //in case we want him to be hidden/burrowed for an amount of time
                state = 3;                                                      //go to un-burrowing
                entity.spr.color = burrowedColor;
                taskComplete = true; 
                break;
            case 1:                                                             //On a tile, waiting to do a thing
                entity.invincible = false;
                taskComplete = false;
                float _thisTime;
                _thisTime = Random.Range(decisionTimeLower, decisionTime);
                yield return new WaitForSecondsRealtime(_thisTime);      
                state = 2;                                                      //go to burrowing                                            
                taskComplete = true; 
                break;
            case 2:                                                             //burrows and is hidden
                t = 0;
                taskComplete = false; 
                while (t < 1)
                { // while t below the end limit...
                  // increment it at the desired rate every update:
                    entity.spr.color = Color.Lerp(normalColor, burrowedColor,t);   //fades the color of the sprite to black, this is a placeholder for a burrowing animation
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
                    entity.spr.color = Color.Lerp(burrowedColor,normalColor, t2);   //fades the color of the sprite to black, this is a placeholder for a burrowing animation
                    t2 += Time.deltaTime / lerpDuration;
                    yield return new WaitForSecondsRealtime(.001f);
                }
                entity.invincible = false;                                     //make the entity mortal again
                state = 1;                                                      //go to waiting 
                taskComplete = true; 
                break;
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


}
