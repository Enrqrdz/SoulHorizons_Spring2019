﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Cards/Mend")]
[RequireComponent(typeof(AudioSource))]

public class scr_Mend : ActionData
{

    public int Mend_hp;
    private AudioSource PlayCardSFX;
    public AudioClip MendSFX;
    public override void Activate()
    {
        PlayCardSFX = ObjectReference.Instance.ActionManager;
        PlayCardSFX.clip = MendSFX;
        PlayCardSFX.Play();
        Entity player = ObjectReference.Instance.PlayerEntity;

        player._health.Heal(Mend_hp);

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