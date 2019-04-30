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
    public AudioSource[] SFX_Sources;

    AudioSource Attack_SFX;
    AudioSource Footsteps_SFX;
    public AudioClip[] movements_SFX;
    private AudioClip movement_SFX;
    public AudioClip[] attacks_SFX;
    private AudioClip attack_SFX;

    private void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        AudioSource[] SFX_Sources = GetComponents<AudioSource>();
        Attack_SFX = SFX_Sources[0];
        Footsteps_SFX = SFX_Sources[0];
        scr_Grid.GridController.SetTileOccupied(true, entity._gridPos.x, entity._gridPos.y, this.entity);
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
                    if (movePosition > 0)
                    {
                        movePosition--;
                    }
                    else
                    {
                        movePosition = 3;
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
        if (!hSOnCD && HunterShotCheck())
        {
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
        GameObject player = ObjectReference.Instance.Player;
        int playerY = player.GetComponent<Entity>()._gridPos.y;
        if (entity._gridPos.y == playerY)
        {
            return true;
        }
        return false;
    }

    void StartHunterShot()
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>()._gridPos.y > entity._gridPos.y || GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>()._gridPos.y < entity._gridPos.y)
        {
            AttackController.Instance.AddNewAttack(hunterShot, entity._gridPos.x, entity._gridPos.y + 1, entity);
            AttackController.Instance.AddNewAttack(hunterShot, entity._gridPos.x, entity._gridPos.y - 1, entity);
        }
        else
        {
            AttackController.Instance.AddNewAttack(hunterShot, entity._gridPos.x, entity._gridPos.y, entity);
        }
        anim.SetBool("Attack", false);
    }



    private IEnumerator HunterShot()
    {
        hSOnCD = true;
        yield return new WaitForSeconds(hSChargeTime);
        anim.SetBool("Attack", true);
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>()._gridPos.y > entity._gridPos.y || GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>()._gridPos.y < entity._gridPos.y)
        {
            AudioSource[] SFX_Sources = GetComponents<AudioSource>();
            Attack_SFX = SFX_Sources[0];
            attack_SFX = attacks_SFX[1];
            Attack_SFX.clip = attack_SFX;
            Attack_SFX.Play();
            PrimeAttackTiles(hunterShot, entity._gridPos.x, entity._gridPos.y + 1);
            PrimeAttackTiles(hunterShot, entity._gridPos.x, entity._gridPos.y - 1);
        }
        else
        {
            AudioSource[] SFX_Sources = GetComponents<AudioSource>();
            Attack_SFX = SFX_Sources[0];
            attack_SFX = attacks_SFX[1];
            Attack_SFX.clip = attack_SFX;
            Attack_SFX.Play();
            PrimeAttackTiles(hunterShot, entity._gridPos.x, entity._gridPos.y);
        }
        yield return new WaitForSeconds(hSCooldownTime);
        hSOnCD = false;
    }

    private IEnumerator ArrowRain(float _aRInterval) //Maybe one day we'll put this in
    {
        //TELEGRAPH
        canArrowRain = false;
        yield return new WaitForSeconds(1f);
        GameObject player = ObjectReference.Instance.Player;
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
                return new Vector2(xPos + 1, yPos + 1);
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
                return new Vector2(xPos - 1, yPos + 1);
            }
        }

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
