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
    public float movementInterval;

    private bool canArrowRain = true;
    private bool canMove = true;
    private bool goBackwards = false;
    private int movePosition = 0;

    [HideInInspector]
    public int hunterShotDecider;

    AudioSource Attack_SFX;
    public AudioClip[] attacks_SFX;
    private AudioClip attack_SFX;

    private void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        AudioSource[] SFX_Sources = GetComponents<AudioSource>();
        Attack_SFX = SFX_Sources[1];
        hunterShotDecider = Random.Range(0, 6);
        Debug.Log(hunterShotDecider + " Start");
    }

    public override void Move()
    {
        int xPos = entity._gridPos.x;
        int yPos = entity._gridPos.y;
        int xRange = scr_Grid.GridController.columnSizeMax;
        int yRange = scr_Grid.GridController.rowSizeMax;
        Vector2 newPosition;
        try
        {
            newPosition = PickMovePosition(xPos, yPos, movePosition, goBackwards);
            if (!scr_Grid.GridController.CheckIfOccupied((int)newPosition.x, (int)newPosition.y) && (scr_Grid.GridController.ReturnTerritory((int)newPosition.x, (int)newPosition.y).name == entity.entityTerritory.name))
            {
                entity.SetTransform((int)newPosition.x, (int)newPosition.y);
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
                newPosition = PickMovePosition(xPos, yPos, movePosition, goBackwards);
                if (!scr_Grid.GridController.CheckIfOccupied((int)newPosition.x, (int)newPosition.y) && (scr_Grid.GridController.ReturnTerritory((int)newPosition.x, (int)newPosition.y).name == entity.entityTerritory.name))
                {
                    entity.SetTransform((int)newPosition.x, (int)newPosition.y);
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
                goBackwards = false;
                return;
            }
        }
    }

    public override void UpdateAI()
    {
        scr_Grid.GridController.SetTileOccupied(true, entity._gridPos.x, entity._gridPos.y, this.entity);
        if (!hSOnCD && HunterShotCheck())
        {
            hunterShotDecider = Random.Range(0, 6);
            StartCoroutine(HunterShot());
        }
        else if (canMove)
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
        int playerY = player.GetComponent<Entity>()._gridPos.y;
        if (entity._gridPos.y == playerY)
        {
            return true;
        }
        return false;
    }

    void StartHunterShot()
    {
        int randomVal;
        randomVal = Random.Range(0, 6); //The arrow has a 3/5 chance to come out straight, and a 1/5 chance to come out either one tile below or above the archer
        Debug.Log(hunterShotDecider + "ActualAttack");
        
        if (randomVal == 0 || randomVal == 6)
        {
            PrimeAttackTiles(hunterShot, entity._gridPos.x, entity._gridPos.y + 1);
            PrimeAttackTiles(hunterShot, entity._gridPos.x, entity._gridPos.y - 1);
            AttackController.Instance.AddNewAttack(hunterShot, entity._gridPos.x, entity._gridPos.y + 1, entity);
            AttackController.Instance.AddNewAttack(hunterShot, entity._gridPos.x, entity._gridPos.y - 1, entity);
        }
        else
        {
            PrimeAttackTiles(hunterShot, entity._gridPos.x, entity._gridPos.y);
            AttackController.Instance.AddNewAttack(hunterShot, entity._gridPos.x, entity._gridPos.y, entity);
        }
    }

    

    private IEnumerator HunterShot()
    {
        hSOnCD = true;
        yield return new WaitForSeconds(hSChargeTime);
        int index = Random.Range(0, attacks_SFX.Length);
        attack_SFX = attacks_SFX[index];
        Attack_SFX.clip = attack_SFX;
        Attack_SFX.Play();
        anim.SetBool("Attack", true);
        Debug.Log(hunterShotDecider + "Coroutine");
        /*if (hunterShotDecider == 0 || hunterShotDecider == 6)
        {
            yield return new WaitForSeconds(.85f);
            PrimeAttackTiles(hunterShot, entity._gridPos.x, entity._gridPos.y + 1);
            PrimeAttackTiles(hunterShot, entity._gridPos.x, entity._gridPos.y - 1);
        }
        else
        {
            yield return new WaitForSeconds(.85f);
            PrimeAttackTiles(hunterShot, entity._gridPos.x, entity._gridPos.y);
        }*/
        yield return new WaitForSeconds(hSCooldownTime);
        hSOnCD = false;
    }

    private IEnumerator ArrowRain(float _aRInterval) //Maybe one day we'll put this in
    {
        //TELEGRAPH 
        canArrowRain = false;
        yield return new WaitForSeconds(1f);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        int playerXPos = player.GetComponent<Entity>()._gridPos.x;
        AttackController.Instance.AddNewAttack(arrowRain, playerXPos, scr_Grid.GridController.rowSizeMax - 1, entity);
        yield return new WaitForSeconds(_aRInterval);
        canArrowRain = true;
    }

    Vector2 PickMovePosition (int xPos, int yPos, int movePosition, bool backwards)
    {
        if (movePosition == 0)
        {
            if (backwards == false)
            {
                return new Vector2( xPos - 1 , yPos - 1);
            }
            else
            {
                return new Vector2(xPos - 1, yPos + 1);
            }
        }
        else if (movePosition == 1)
        {
            if (backwards == false) 
            {
                return new Vector2(xPos - 1, yPos + 1);
            }
            else
            {
                return new Vector2(xPos + 1, yPos - 1);
            }
        }
        else if (movePosition == 2)
        {
            if (backwards == false)
            {
                return new Vector2(xPos + 1, yPos + 1);
            }
            else
            {
                return new Vector2(xPos + 1, yPos - 1);
            }
        }
        else
        {
            if (backwards == false)
            {
                return new Vector2(xPos + 1, yPos - 1);
            }
            else
            {
                return new Vector2(xPos + 1, yPos + 1);
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

    IEnumerator MovementClock()
    {
        if (canMove)
        {
            canMove = false;
            yield return new WaitForSeconds(movementInterval);
            Move();
            canMove = true;
        }
    }
}
