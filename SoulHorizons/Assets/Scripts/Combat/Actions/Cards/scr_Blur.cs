using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Cards/Blur")]
[RequireComponent(typeof(AudioSource))]

public class scr_Blur : ActionData
{
    private AudioSource PlayCardSFX;
    public AudioClip BlurSFX;
    private Entity player;

    public float duration;
    public override void Activate()
    {
        PlayCardSFX = GameObject.Find("ActionManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = BlurSFX;
        PlayCardSFX.Play();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
        player.SetInvincible(true, duration);

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
