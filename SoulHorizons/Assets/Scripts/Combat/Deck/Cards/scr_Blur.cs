using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Cards/Blur")]
[RequireComponent(typeof(AudioSource))]

public class scr_Blur : scr_Card
{
    private AudioSource PlayCardSFX;
    public AudioClip BlurSFX;

    public float duration;
    public override void Activate()
    {
        ActivateEffects();
        PlayCardSFX = GameObject.Find("DeckManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = BlurSFX;
        PlayCardSFX.Play();
        scr_Entity player = GameObject.FindGameObjectWithTag("Player").GetComponent<scr_Entity>();
        player.setInvincible(true, duration);

    }

    public override void StartCastingEffects()
    {

    }

    protected override void ActivateEffects()
    {
        //put start effects here
    }
}