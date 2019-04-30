using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class RoamingRonin : scr_EntityAI
{
    //Roaming Ronin
    //Two Attacks: a long range attack and an upclose melee when player is close
    //AI: Two Phases: Changes phase when health is 40%
    // Phase 1: Moves every 3 seconds, tries to be in the same row as the player
    // Phase 2: Moves every 1 second, melee attack also strikes twice

    public AttackData rangedAttack;
    public AttackData rangedAttack2; //stronger version of ranged attack
    public AttackData meleeAttack;
    public AttackData meleeAttack2; //stronger version of melee

    int maxHealth = 0;
    int currHealth = 0;

    int xRange = 0;
    int yRange = 0;
    Vector2Int currentHeadPosition;
    int xPosition;
    int yPosition;

    int attackPhase = 0; //0 for normal phase, 1 for broken armor phase
    public int damageThreshold = 40; //when Ronin reaches 40% health, switch to next phase
    bool gonnaMelee = false;

    public float movementInterval = .75f;
    float movementIntervalModfier = 0;
    public float rangedAttackInterval = 1f;

    int state = 0;
    bool completedTask = true;
    int moveDirection = 0; //0 means up, 1 right, 2, down, 3 left

    AudioSource[] SFX_Sources;
    AudioSource Attack_SFX;
    AudioSource Footsteps_SFX;
    public AudioClip[] movements_SFX;
    private AudioClip movement_SFX;
    public AudioClip[] attacks_SFX;
    private AudioClip attack_SFX;

    Entity player; 

    void Start()
    {
        AudioSource[] SFX_Sources = GetComponents<AudioSource>();
        //Attack_SFX = SFX_Sources[1];
        Footsteps_SFX = SFX_Sources[0];
        anim = gameObject.GetComponentInChildren<Animator>();
        player = ObjectReference.Instance.PlayerEntity;
        maxHealth = entity._health.hp;
        xRange = scr_Grid.GridController.columnSizeMax - entity.width;
        yRange = scr_Grid.GridController.rowSizeMax - entity.height;
        xPosition = entity._gridPos.x;
        yPosition = entity._gridPos.y;
        currentHeadPosition = new Vector2Int(xPosition, yPosition);
        moveDirection = 0;
        scr_Grid.GridController.SetTileOccupied(true, entity._gridPos.x, entity._gridPos.y, this.entity);
    }

    public override void UpdateAI()
    {
        currHealth = entity._health.hp;
        PhaseManager();
        if (completedTask)
        {
            StartCoroutine(Brain());
        }

    }

    public override void Move()
    {
        AudioSource[] SFX_Sources = GetComponents<AudioSource>();
        Footsteps_SFX = SFX_Sources[0];
        movement_SFX = movements_SFX[0];
        Footsteps_SFX.clip = movement_SFX;
        Footsteps_SFX.Play();
        Vector2Int StartPosition = currentHeadPosition;
        try
        {
            if (moveDirection == 0)
            {
                xPosition = currentHeadPosition.x;
                yPosition = currentHeadPosition.y + 1;
            }
            else if (moveDirection == 1)
            {
                xPosition = currentHeadPosition.x + 1;
                yPosition = currentHeadPosition.y;
            }
            else if (moveDirection == 2)
            {
                xPosition = currentHeadPosition.x;
                yPosition = currentHeadPosition.y - 1;
            }
            else if (moveDirection == 3)
            {
                xPosition = currentHeadPosition.x - 1;
                yPosition = currentHeadPosition.y;
            }
            currentHeadPosition = new Vector2Int(xPosition, yPosition);
            bool isSpotOccupied = CheckIfOccupied(xPosition, yPosition);
            if (scr_Grid.GridController.ReturnTerritory(xPosition, yPosition).name == entity.entityTerritory.name && isSpotOccupied == false)
            {
                entity.SetLargeTransform(currentHeadPosition);
                DecideDirection(currentHeadPosition);
            }
            else if (isSpotOccupied == true)
            {
                currentHeadPosition = StartPosition;
                if (moveDirection < 3)
                {
                    moveDirection++;
                }
                else
                {
                    moveDirection = 0;
                }
            }
        }
        catch
        {
            currentHeadPosition = StartPosition;
            DecideDirection(currentHeadPosition);
        }
    }

    public void DecideDirection (Vector2Int currentPosition)
    {
        bool isOccupied = CheckIfOccupied(currentPosition.x, currentPosition.y);
        if (currentPosition.y == yRange && currentPosition.x < xRange)
        {
            moveDirection = 1;
            return;
        }
        else if (currentPosition.y > 0)
        {
            moveDirection = 2;
            return;
        }
        else if(currentPosition.x != DomainManager.Instance.columnToBeSeized )
        {
            moveDirection = 3;
            return;
        }
        else
        {
            moveDirection = 0;
            return;
        }

    }

    bool CheckIfOccupied (int xPos, int yPos)
    {
        if  (
            scr_Grid.GridController.CheckIfOccupied(xPos, yPos) == false 
            && scr_Grid.GridController.CheckIfOccupied(xPos + 1, yPos) == false || scr_Grid.GridController.GetEntityAtPosition(xPos + 1, yPos) == entity)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    public override void Die()
    {
        entity.Death();
    }

    public void GoToMelee()
    {
        AudioSource[] SFX_Sources = GetComponents<AudioSource>();
        Footsteps_SFX = SFX_Sources[0];
        movement_SFX = movements_SFX[0];
        Footsteps_SFX.clip = movement_SFX;
        Footsteps_SFX.Play();

        Vector2Int StartPosition = currentHeadPosition;

        int playerY = player.GetComponent<Entity>()._gridPos.y;
        xPosition = DomainManager.Instance.columnToBeSeized;
        if (playerY == 0)
        {
            yPosition = 0;
        }
        else if(playerY <= scr_Grid.GridController.rowSizeMax / 2)
        {
            yPosition = 1;
        }
        else
        {
            yPosition = 2;
        }

        currentHeadPosition = new Vector2Int(xPosition, yPosition);

        bool isSpotOccupied = CheckIfOccupied(xPosition, yPosition);
        if (isSpotOccupied == false)
        {
            gonnaMelee = true;
            entity.SetLargeTransform(currentHeadPosition);
        }
        else
        {
           currentHeadPosition = StartPosition;
           gonnaMelee = false;
        }

    }

    public void MoveBack()
    {
        Vector2Int StartPosition = currentHeadPosition;
        xPosition = currentHeadPosition.x + 1;
        currentHeadPosition = new Vector2Int(xPosition, yPosition);

        bool isOccupied = CheckIfOccupied(xPosition, yPosition);
        if (scr_Grid.GridController.ReturnTerritory(xPosition, yPosition).name == entity.entityTerritory.name && isOccupied == false)
        {
            entity.SetLargeTransform(currentHeadPosition);
        }
        else
        {
           currentHeadPosition = StartPosition;
        }
    }

    private void PhaseManager()
    {
        int healthWhenArmorBreaks = (maxHealth * damageThreshold) / 100;
        if (currHealth <= healthWhenArmorBreaks)
        {
            attackPhase = 1;
            movementIntervalModfier = .25f;
            entity.spr.color = Color.red;
        }
        else
        {
            attackPhase = 0;
        }
    }

    void StartRangedAttack()
    {
        anim.SetBool("Attack", true);
        AudioSource[] SFX_Sources = GetComponents<AudioSource>();
        Attack_SFX = SFX_Sources[0];
        attack_SFX = attacks_SFX[0];
        Attack_SFX.clip = attack_SFX;
        Attack_SFX.Play();
        if (attackPhase == 0)
        {
            AttackController.Instance.AddNewAttack(rangedAttack, entity._gridPos.x, entity._gridPos.y, entity);
        }
        else
        {
            AttackController.Instance.AddNewAttack(rangedAttack2, entity._gridPos.x, entity._gridPos.y, entity);
        }

    }

    void RangedAttack()
    {

        PrimeAttackTiles(rangedAttack, entity._gridPos.x, entity._gridPos.y);
        if (attackPhase == 0)
        {
            AttackController.Instance.AddNewAttack(rangedAttack, entity._gridPos.x, entity._gridPos.y, entity);
        }
        else
        {
            AttackController.Instance.AddNewAttack(rangedAttack2, entity._gridPos.x, entity._gridPos.y, entity);
        }
    }

    void StartMeleeAttack()
    {
        //insert animation here
        anim.SetBool("Attack2", true);
        AudioSource[] SFX_Sources = GetComponents<AudioSource>();
        Attack_SFX = SFX_Sources[0];
        attack_SFX = attacks_SFX[0];
        Attack_SFX.clip = attack_SFX;
        Attack_SFX.Play();
        if (attackPhase == 0)
        {
            AttackController.Instance.AddNewAttack(meleeAttack, currentHeadPosition.x - 1, currentHeadPosition.y, entity);
        }
        else
        {
            AttackController.Instance.AddNewAttack(meleeAttack2, currentHeadPosition.x - 1, currentHeadPosition.y, entity);
        }

    }

    void MeleeAttack()
    {
        Debug.Log("BACK SLASH");
        if (attackPhase == 0)
        {
            AttackController.Instance.AddNewAttack(meleeAttack, entity._gridPos.x, entity._gridPos.y, entity);
        }
        else
        {
            AttackController.Instance.AddNewAttack(meleeAttack2, entity._gridPos.x, entity._gridPos.y, entity);
        }
    }

    private void SetTilesOccupied()
    {
        try
        {
            for (int i = 0; i < entity.width; i++)
            {
                for (int j = 0; j < entity.height; j++)
                {
                    int xPosition = currentHeadPosition.x + i;
                    int yPosition = currentHeadPosition.y + j;
                    scr_Grid.GridController.SetTileOccupied(true, xPosition, yPosition, this.entity);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private IEnumerator Brain()
    {
        switch (state)
        {
            case 0:
                completedTask = false;
                yield return new WaitForSeconds(movementInterval - movementIntervalModfier);
                Move();
                state = 1;
                completedTask = true;
                break;
            case 1:
                completedTask = false;
                yield return new WaitForSeconds(0.75f);
                if (player._gridPos.x == DomainManager.Instance.columnToBeSeized - 1)
                {
                    state = 2;
                }
                else
                {
                    state = 4;
                }
                completedTask = true;
                break;
            case 2:
                completedTask = false;
                GoToMelee();
                if (gonnaMelee == true)
                {
                    PrimeAttackTiles(meleeAttack, entity._gridPos.x, entity._gridPos.y);
                    PrimeAttackTiles(meleeAttack, entity._gridPos.x, entity._gridPos.y + 1);
                    yield return new WaitForSeconds(.55f);
                    StartMeleeAttack();
                    state = 3;
                    yield return new WaitForSeconds(movementInterval - movementIntervalModfier);
                    completedTask = true;
                }
                else
                {
                    completedTask = true;
                    state = 4;
                }
                break;
            case 3:
                completedTask = false;
                yield return new WaitForSeconds(movementInterval - movementIntervalModfier);
                MoveBack();
                gonnaMelee = false;
                state = 4;
                completedTask = true;
                break;
            case 4:
                completedTask = false;
                PrimeAttackTiles(rangedAttack, entity._gridPos.x, entity._gridPos.y);
                yield return new WaitForSeconds(rangedAttackInterval);
                StartRangedAttack();
                state = 0;
                completedTask = true;
                break;
        }
        yield return null;
    }
}
