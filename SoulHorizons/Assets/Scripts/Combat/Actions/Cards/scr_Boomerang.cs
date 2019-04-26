﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

[CreateAssetMenu(menuName = "Cards/Boomerang")]
public class scr_Boomerang : ActionData
{
    public AttackData boomerangAttack;
    private AudioSource PlayCardSFX;
    public AudioClip BoomerangSFX;

    public override void Activate()
    {
        PlayCardSFX = GameObject.Find("ActionManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = BoomerangSFX;
        PlayCardSFX.Play();

        Entity player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();

        //add attack to attack controller script
        //does a check to see if the target col is off the map 
        AttackController.Instance.AddNewAttack(boomerangAttack, player._gridPos.x + 1, player._gridPos.y, player);
    }

    public override void Project()
    {
        throw new System.NotImplementedException();
    }

    public override void DeProject()
    {
        throw new System.NotImplementedException();
    }
}
