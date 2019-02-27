﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (menuName = "Cards/LightningBolt")]
[RequireComponent(typeof(AudioSource))]

public class scr_LightningBolt : CardData {

    public float damage = 6f;
    private AudioSource PlayCardSFX;
    public AudioClip LightningBoltSFX;

    public override void Activate()
    {
        ActivateEffects();
        PlayCardSFX = GameObject.Find("ActionManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = LightningBoltSFX;
        PlayCardSFX.Play();
        //implement functionality here
        Debug.Log(name + ": Zap!");
    }

    public override void StartCastingEffects()
    {
        
    }

    protected override void ActivateEffects()
    {
        //put start effects here
    }
}
