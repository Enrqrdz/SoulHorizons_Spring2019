using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Cards/HardenSoul")]
[RequireComponent(typeof(AudioSource))]

public class scr_HardenSoul : scr_Card {

    private AudioSource PlayCardSFX;
    public AudioClip HardenSoulSFX;
    public int Shield_hp; //How much shield hp
    public override void Activate()

    {
        ActivateEffects();
        PlayCardSFX = GameObject.Find("DeckManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = HardenSoulSFX;
        PlayCardSFX.Play();
        scr_Entity player = GameObject.FindGameObjectWithTag("Player").GetComponent<scr_Entity>();
        player._health.shield += Shield_hp;
        
    }

    public override void StartCastingEffects()
    {

    }

    protected override void ActivateEffects()
    {
        //put start effects here
    }
}
