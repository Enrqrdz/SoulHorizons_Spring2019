﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Cards/StaticShield")]
[RequireComponent(typeof(AudioSource))]

public class scr_StaticShield : ActionData
{
    private AudioSource PlayCardSFX;
    public AudioClip shieldSFX;
    public AttackData StaticShield;
    public int shieldProtection = 0; //the amount of damage the shield is reducing damage by
    public int shieldProtectionIncrement = 1; //the rate the damage reduction of the shield increasesby when you move
    public int shieldProtectionMax = 50;
    private Entity player;

    public float duration;
    public override void Activate()
    {
        PlayCardSFX = GameObject.Find("ActionManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = shieldSFX;
        PlayCardSFX.Play();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();

        player.SetShield(true, duration, shieldProtection,shieldProtectionIncrement, shieldProtectionMax);

    }

    public override void Project()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
        player.Highlight();
    }

    public override void DeProject()
    {
        player.DeHighlight();
    }
}
