﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/CrashingWave")]
[RequireComponent(typeof(AudioSource))]
public class scr_CrashingWave : ActionData
{
    public AttackData CrashingWave;
    public AudioClip WaveSFX;
    private AudioSource PlayCardSFX;

    public override void Activate()
    {
        Entity player = ObjectReference.Instance.PlayerEntity;
        try
        {
            AttackController.Instance.AddNewAttack(CrashingWave, (player._gridPos.x + 1), player._gridPos.y + 1, player);
            AttackController.Instance.AddNewAttack(CrashingWave, (player._gridPos.x + 1), player._gridPos.y, player);
        }
        catch
        {
            AttackController.Instance.AddNewAttack(CrashingWave, (player._gridPos.x + 1), player._gridPos.y, player);
        }
    }

    public override void DeProject()
    {
        throw new System.NotImplementedException();
    }

    public override void Project()
    {
        throw new System.NotImplementedException();
    }
}
