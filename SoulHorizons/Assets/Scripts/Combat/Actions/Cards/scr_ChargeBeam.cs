﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/ChargeBeam")]
[RequireComponent(typeof(AudioSource))]

public class scr_ChargeBeam : ActionData
{
    public AttackData ChargeBeam;
    public AudioClip BeamSFX;
    private AudioSource PlayCardSFX;

    public override void Activate()
    {

        Entity player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();

        AttackController.Instance.AddNewAttack(ChargeBeam, (player._gridPos.x + 1), player._gridPos.y, player);
    }
}
