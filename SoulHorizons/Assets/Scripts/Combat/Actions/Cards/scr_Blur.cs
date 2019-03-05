using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Cards/Blur")]
[RequireComponent(typeof(AudioSource))]

public class scr_Blur : ActionData
{
    private AudioSource PlayCardSFX;
    public AudioClip BlurSFX;

    public float duration;
    public override void Activate()
    {
        PlayCardSFX = GameObject.Find("ActionManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = BlurSFX;
        PlayCardSFX.Play();
        Entity player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
        player.setInvincible(true, duration);

    }
}