using UnityEngine;

public enum AgentStates { MoveUp, MoveDown, Attacking, Idle }
public class MushineAI : scr_EntityAI
{

    public float momvementFrequency;
    public bool enemyOnSameRow;
    public AgentStates currentState;

    public AttackData primaryAttack;
    private int attackCounter = 0;

    public float attackFrequency;

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
        scr_Grid.GridController.SetTileOccupied(true, entity._gridPos.x, entity._gridPos.y, this.entity);
    }

    public override void UpdateAI()
    {

    }

    public override void Move()
    {
        int index = Random.Range(0, movements_SFX.Length);
        movement_SFX = movements_SFX[index];
        Footsteps_SFX.clip = movement_SFX;
        Footsteps_SFX.Play();
    }

    public override void Die()
    {
        entity.Death();
    }

    void Attack1()
    {
        AudioSource[] SFX_Sources = GetComponents<AudioSource>();
        Attack_SFX = SFX_Sources[0];
        int index = Random.Range(0, attacks_SFX.Length);
        attack_SFX = attacks_SFX[index];
        Attack_SFX.clip = attack_SFX;
        Attack_SFX.Play();
        AttackController.Instance.AddNewAttack(primaryAttack, entity._gridPos.x, entity._gridPos.y, entity);
    }

    void StartAttack1()
    {
        anim.SetBool("Attack", true);
        PrimeAttackTiles(primaryAttack, entity._gridPos.x, entity._gridPos.y);
    }
}
