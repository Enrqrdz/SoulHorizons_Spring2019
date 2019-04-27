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
    int transitionNumber;
    Vector2Int[] possibleHeadPositions;
    Vector2Int[] movePattern;
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
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();

        maxHealth = entity._health.hp;
        xRange = scr_Grid.GridController.columnSizeMax - entity.width;
        yRange = scr_Grid.GridController.rowSizeMax - entity.height;
        SetPossibleHeadPositions();
        SetMovePattern();
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

    private void SetPossibleHeadPositions()
    {
        possibleHeadPositions = new[] {
                                        new Vector2Int(xRange - 2, yRange), new Vector2Int(xRange - 1, yRange), new Vector2Int(xRange, yRange),
                                        new Vector2Int(xRange - 2, yRange - 2), new Vector2Int(xRange - 1, yRange - 2), new Vector2Int(xRange, yRange - 2),
                                        new Vector2Int(xRange - 2, yRange - 1), new Vector2Int(xRange - 1, yRange - 1), new Vector2Int(xRange, yRange - 1),
                                      };
    }

    private void SetMovePattern()
    {
        movePattern = new[] {
                                possibleHeadPositions[2], possibleHeadPositions[8], possibleHeadPositions[5],
                                possibleHeadPositions[4], possibleHeadPositions[7], possibleHeadPositions[1],
                              };
    }

    private void SetTransitionNumber()
    {
        //Ronin origin must be on one of the designated positions
        for (int i = 0; i < movePattern.Length; i++)
        {
            if (currentHeadPosition == movePattern[i])
            {
                transitionNumber = i;
                break;
            }
            else if (i == movePattern.Length - 1)
            {
                //Arbitrary preference in position
                currentHeadPosition = movePattern[1];
                transitionNumber = 1;
            }
        }
    }

    public override void Move()
    {
        AudioSource[] SFX_Sources = GetComponents<AudioSource>();
        Footsteps_SFX = SFX_Sources[0];
        movement_SFX = movements_SFX[0];
        Footsteps_SFX.clip = movement_SFX;
        Footsteps_SFX.Play();

        transitionNumber += 1;
        transitionNumber %= movePattern.Length;
        xPosition = (int)movePattern[transitionNumber].x;
        yPosition = (int)movePattern[transitionNumber].y;
        currentHeadPosition = new Vector2Int(xPosition, yPosition);

        if (scr_Grid.GridController.ReturnTerritory(xPosition, yPosition).name == entity.entityTerritory.name && scr_Grid.GridController.CheckIfOccupied(xPosition, yPosition) == false)
        {
            entity.SetLargeTransform(currentHeadPosition);
        }
        else
        {
            transitionNumber += 2;
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

        int playerY = player.GetComponent<Entity>()._gridPos.y;      
        if (playerY == 0)
        {
            xPosition = (int)possibleHeadPositions[3].x;
            yPosition = (int)possibleHeadPositions[3].y;
        }
        else if(playerY <= scr_Grid.GridController.rowSizeMax / 2)
        {
            xPosition = (int)possibleHeadPositions[6].x;
            yPosition = (int)possibleHeadPositions[6].y;
        }
        else
        {
            xPosition = (int)possibleHeadPositions[0].x;
            yPosition = (int)possibleHeadPositions[0].y;
        }

        currentHeadPosition = new Vector2Int(xPosition, yPosition);

        if (scr_Grid.GridController.CheckIfOccupied(xPosition, yPosition) == false)
        {
            entity.SetLargeTransform(currentHeadPosition);
        }
        else
        {
            return;
        }

    }

    public void MoveBack()
    {
        transitionNumber = 5;
        transitionNumber %= movePattern.Length;
        xPosition = (int)movePattern[transitionNumber].x;
        yPosition = (int)movePattern[transitionNumber].y;
        currentHeadPosition = new Vector2Int(xPosition, yPosition);

        if (scr_Grid.GridController.ReturnTerritory(xPosition, yPosition).name == entity.entityTerritory.name && scr_Grid.GridController.CheckIfOccupied(xPosition, yPosition) == false)
        {
            entity.SetLargeTransform(currentHeadPosition);
        }
        else
        {
            transitionNumber += 2;  //This will effectively skip three zig-zag positions before the next check
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
        anim.SetBool("RoninMelee", true);
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
                    int xPosition = (int)movePattern[transitionNumber].x + i;
                    int yPosition = (int)movePattern[transitionNumber].y + j;
                    scr_Grid.GridController.SetTileOccupied(true, xPosition, yPosition, this.entity);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
            Debug.Log("Ronin position is off!");
            Debug.Log("Transition Number: " + transitionNumber);
        }
    }


    bool CheckIfInMeleeRange()
    {
        if (scr_Grid.GridController.ReturnTerritory(player._gridPos.x + 1, player._gridPos.y).name == entity.entityTerritory.name)
        {
            return true;
        }

        return false;
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
                gonnaMelee = CheckIfInMeleeRange();
                yield return new WaitForSeconds(0.75f);
                if (gonnaMelee == true)
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
                PrimeAttackTiles(meleeAttack, entity._gridPos.x, entity._gridPos.y);
                yield return new WaitForSeconds(.25f);
                StartMeleeAttack();
                state = 3;
                yield return new WaitForSeconds(movementInterval - movementIntervalModfier);
                gonnaMelee = false;
                completedTask = true;
                break;
            case 3:
                completedTask = false;
                yield return new WaitForSeconds(movementInterval - movementIntervalModfier);
                MoveBack();
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
