using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamingRonin : scr_EntityAI
{
    //Roaming Ronin
    //Two Attacks: a long range attack and an upclose melee when player is close
    //AI: Two Phases: Changes phase when health is 40%
    // Phase 1: Moves every 3 seconds, tries to be in the same row as the player
    // Phase 2: Moves every 1 second, melee attack also strikes twice

   

    public AttackData rangedAttack;
    public AttackData meleeAttack;

    public int meleeDamage1 = 6;
    public int rangedDamage1 = 2;
    public int meleeDamage2 = 6;
    public int rangedDamage2 = 4;
    int phase = 0; //0 for normal phase, 1 for broken armor phase
    bool gonnaMelee = false;

    public float movementIntervalLower;
    public float movementIntervalUpper;
    int state = 0;
    bool completedTask = false;

    AudioSource Attack_SFX;
    public AudioClip[] attacks_SFX;
    private AudioClip attack_SFX;
    AudioSource Footsteps_SFX;
    public AudioClip[] movements_SFX;
    private AudioClip movement_SFX;

    void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        AudioSource[] SFX_Sources = GetComponents<AudioSource>();
        Footsteps_SFX = SFX_Sources[0];
    }


    public override void UpdateAI()
    {

    }

    public override void Move()
    {
        int xPos = entity._gridPos.x;
        int yPos = entity._gridPos.y;
        int tries = 0;

        while (tries < 10)
        {
            if (gonnaMelee)
            {
                xPos = PickXCoord(xPos);
            }
            else
            {
                yPos = PickYCoord(yPos);
            }

            if (!scr_Grid.GridController.CheckIfOccupied(xPos, yPos) && (scr_Grid.GridController.ReturnTerritory(xPos, yPos).name == entity.entityTerritory.name))
            {
                entity.SetTransform(xPos, yPos);
                return;
            }
            else
            {
                tries++;
                if(tries >= 10)
                {
                    xPos = PickXCoord(xPos);
                    if (!scr_Grid.GridController.CheckIfOccupied(xPos, yPos) && (scr_Grid.GridController.ReturnTerritory(xPos, yPos).name == entity.entityTerritory.name))
                    {
                        entity.SetTransform(xPos, yPos);
                        return;
                    }
                }

            }
        }

    }

    public override void Die()
    {
        Debug.Log("And now my life has ended ... I have no regrets... except that I could not hold my wife and child another time ");
    }

    public int PickXCoord (int xPos)
    {
        
        if(gonnaMelee)
        {
            xPos = 0;
        }
        else
        {
            xPos++;
        }
        return xPos;
    }

    public int PickYCoord(int yPos)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        int playerYPos = player.GetComponent<scr_Entity>()._gridPos.y;
        if (!gonnaMelee)
        {
            if (yPos != playerYPos)
            {
                yPos = playerYPos;
            }
            return yPos;
        }
        else
        {
            return yPos;
        }
    }

    public void MoveBack ()
    {
        int randomDir;
        randomDir  = Random.Range(0, 2);
        int xPos = entity._gridPos.x;
        int yPos = entity._gridPos.y;

        if (randomDir == 0)
        {
            xPos++;
            yPos--;
        }
        else
        {
            xPos++;
            yPos++;
        }

        if (!scr_Grid.GridController.CheckIfOccupied(xPos, yPos) && (scr_Grid.GridController.ReturnTerritory(xPos, yPos).name == entity.entityTerritory.name))
        {
            entity.SetTransform(xPos, yPos);
            return;
        }
        else
        {
            MoveBack();
        }

    }
    private void PhaseManager()
    {
        
    }
    void StartRangedAttack()
    {
        Debug.Log("AIR SLASH");
    }

    void StartMeleeAttack()
    {
        Debug.Log("BACK SLASH");
    }
    private IEnumerator Brain ()
    {
        switch (state)
        {
            case 0:
                completedTask = false;
                Move();
                state = 1;
                completedTask = true;
                break;

            case 1:
                completedTask = false;
                float _movementInterval = Random.Range(movementIntervalLower, movementIntervalUpper);
                yield return new WaitForSecondsRealtime(_movementInterval);
                state = 0;
                completedTask = true;
                break;

            case 2:
                completedTask = false;
                gonnaMelee = true;
                Move();
                state = 3;
                completedTask = true;
                break;
            case 3:
                completedTask = false;
                yield return new WaitForSecondsRealtime(.5f);
                StartMeleeAttack();
                yield return new WaitForSecondsRealtime(1);
                state = 4;
                completedTask = true;
                gonnaMelee = false;
                break;

            case 4:
                completedTask = false;
                MoveBack();
                state = 5;
                completedTask = true;
                break;

            case 5:
                completedTask = false;
                yield return new WaitForSecondsRealtime(.75f);
                StartRangedAttack();
                yield return new WaitForSecondsRealtime(2);
                state = 0;
                completedTask = true;
                break;
        }

        yield return null;
    }
}
