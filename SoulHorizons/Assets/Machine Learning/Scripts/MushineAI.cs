using System;
using System.Collections;
using UnityEngine;

public enum Movement { MoveUp, MoveDown, Attacking, Idle }
public class MushineAI : scr_EntityAI
{
    public AttackData primaryAttack;

    //Attack Values to modify
    public int startingDamage = 4;
    public int damageIncrement = 2;
    private int maxDamage = 20;

    public float startingSpeed = 0.35f;
    public float speedIncrement = 0.05f;
    private float maxSpeed = 0.05f;

    //Movement Values to modify
    public float startingMovementCooldown = 1.75f;
    public float movementCooldown;      //If player accuracy is too high
    public float movementIncrement = 0.25f;
    private float minimumMovementCooldown = 0.25f;

    public int startingIdleFrequency = 15;
    public int idleFrequency;       //If player accuracy is too high
    public int idleIncrement = 2;
    private int minimumIdleFrequency = 1;

    public Movement currentMovement;
    private int moveCounter = 0;
    private bool readyToMove = false;
    private Movement previousMovement;

    private float attackCooldown;
    private int attackCounter = 0;
    private float windUpTime;
    private bool readyToAttack = false;
    private bool enemyOnSameRow = false;

    private Entity player;
    private bool preparingAttack = false;
    private bool preparingMovement = false;

    AudioSource[] SFX_Sources;
    AudioSource Attack_SFX;
    AudioSource Footsteps_SFX;
    public AudioClip[] movements_SFX;
    private AudioClip movement_SFX;
    public AudioClip[] attacks_SFX;
    private AudioClip attack_SFX;


    public void Start()
    {
        AudioSource[] SFX_Sources = GetComponents<AudioSource>();
        Footsteps_SFX = SFX_Sources[0];
        Attack_SFX = SFX_Sources[0];
        anim = gameObject.GetComponentInChildren<Animator>();
        player = ObjectReference.Instance.PlayerEntity;

        primaryAttack.damage = startingDamage;
        primaryAttack.incrementTime = startingSpeed;
        movementCooldown = startingMovementCooldown;
        idleFrequency = startingIdleFrequency;

        scr_Grid.GridController.SetTileOccupied(true, entity._gridPos.x, entity._gridPos.y, this.entity);
    }

    public override void UpdateAI()
    {
        enemyOnSameRow = entity._gridPos.y == player._gridPos.y;

        if (readyToMove)
        {
            if (readyToAttack == false && preparingAttack == false)
            {
                StartCoroutine(PrepareAttack());
            }

            DecideAction();
            readyToMove = false;

            Debug.Log(currentMovement);
            switch (currentMovement)
            {
                case Movement.MoveUp:
                    Move();
                    break;
                case Movement.MoveDown:
                    Move();
                    break;
                case Movement.Attacking:
                    StartAttack1();
                    StartCoroutine(Attack1());
                    break;
                case Movement.Idle:
                    break;
                default:
                    break;
            }
        }
        else if (readyToMove == false && preparingMovement == false)
        {
            currentMovement = Movement.Idle;
            StartCoroutine(PrepareToMove());
        }
    }

    private void DecideAction()
    {
        if (readyToAttack && enemyOnSameRow)
        {
            currentMovement = Movement.Attacking;
            return;
        }
        //Decrease idleFrequency to stop more often
        if(moveCounter % idleFrequency == 0)
        {
            moveCounter++;
            currentMovement = Movement.Idle;
            return;
        }
        if (entity._gridPos.y == 0)
        {
            currentMovement = Movement.MoveUp;
            previousMovement = Movement.MoveUp;
            return;
        }
        if (entity._gridPos.y == scr_Grid.GridController.rowSizeMax - 1)
        {
            currentMovement = Movement.MoveDown;
            previousMovement = Movement.MoveDown;
            return;
        }
        if(currentMovement == Movement.Idle || currentMovement == Movement.Attacking)
        {
            currentMovement = previousMovement;
        }
    }

    private IEnumerator PrepareToMove()
    {
        preparingMovement = true;
        yield return new WaitForSeconds(movementCooldown);
        readyToMove = true;
        preparingMovement = false;
    }

    public override void Move()
    {
        moveCounter++;
        SoundMovement();

        if (currentMovement == Movement.MoveUp)
        {
            entity.SetTransform(entity._gridPos.x, entity._gridPos.y + 1);
        }
        if (currentMovement == Movement.MoveDown)
        {
            entity.SetTransform(entity._gridPos.x, entity._gridPos.y - 1);
        }
    }

    private IEnumerator PrepareAttack()
    {
        preparingAttack = true;
        yield return new WaitForSeconds(attackCooldown);
        readyToAttack = true;
        preparingAttack = false;
    }

    private IEnumerator Attack1()
    {
        attackCounter++;
        PrimeAttackTiles(primaryAttack, entity._gridPos.x, entity._gridPos.y);
        yield return new WaitForSeconds(windUpTime);
        
        AttackSound();
        AttackController.Instance.AddNewAttack(primaryAttack, entity._gridPos.x, entity._gridPos.y, entity);
        readyToAttack = false;
        anim.SetBool("Attack", false);
        currentMovement = previousMovement;
    }

    private void StartAttack1()
    {
        anim.SetBool("Attack", true);
    }

    public void NextPhase()
    {
        Debug.Log("Next Phase");
        primaryAttack.damage += damageIncrement;
        Mathf.Clamp(primaryAttack.damage, startingDamage, maxDamage);

        primaryAttack.incrementTime -= speedIncrement;
        Mathf.Clamp(primaryAttack.incrementTime, startingSpeed, maxDamage);

        movementCooldown -= movementIncrement;
        Mathf.Clamp(movementCooldown, minimumMovementCooldown, startingMovementCooldown);

        idleFrequency -= idleIncrement;
        Mathf.Clamp(idleFrequency, minimumIdleFrequency, startingIdleFrequency);
    }

    private void SoundMovement()
    {
        int index = UnityEngine.Random.Range(0, movements_SFX.Length);
        movement_SFX = movements_SFX[index];
        Footsteps_SFX.clip = movement_SFX;
        Footsteps_SFX.Play();
    }

    private void AttackSound()
    {
        AudioSource[] SFX_Sources = GetComponents<AudioSource>();
        Attack_SFX = SFX_Sources[0];
        int index = UnityEngine.Random.Range(0, attacks_SFX.Length);
        attack_SFX = attacks_SFX[index];
        Attack_SFX.clip = attack_SFX;
        Attack_SFX.Play();
    }

    public override void Die()
    {
        entity.Death();
    }
}
