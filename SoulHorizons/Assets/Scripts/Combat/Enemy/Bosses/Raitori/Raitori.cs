using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaitoriPosition
{
}

public class Raitori : scr_EntityAI
{
    [Tooltip("Damage required before first phase transition")]
    public int phase1requirement = 100;

    [Tooltip("Damage required before second phase transition")]
    public int phase2requirement = 100;

    [SerializeField]
    [Tooltip("Width in number of tiles")]
    private int width = 2;
    [SerializeField]
    [Tooltip("Height in number of tiles")]
    private int height = 3;

    public Phase currentPhase;

    private int maxHealth;
    private int currentHealth;

    AudioSource Attack_SFX;
    public AudioClip[] attacks_SFX;
    private AudioClip attack_SFX;
    

    void Start()
    {
        anim = gameObject.GetComponentInChildren<Animator>();
        AudioSource[] SFX_Sources = GetComponents<AudioSource>();
        Attack_SFX = SFX_Sources[1];
        maxHealth = entity._health.hp;
    }

    public override void Move()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateAI()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                scr_Grid.GridController.SetTileOccupied(true, entity._gridPos.x + i, entity._gridPos.y + j, this.entity);
            }
        }  
    }

    public override void Die()
    {
        throw new System.NotImplementedException();
    }
}
