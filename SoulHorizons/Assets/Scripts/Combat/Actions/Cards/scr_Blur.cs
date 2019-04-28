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
        if(PlayCardSFX == null)
        {
            PlayCardSFX = ObjectReference.Instance.ActionManager;
        }
        PlayCardSFX.clip = BlurSFX;
        PlayCardSFX.Play();
        player = ObjectReference.Instance.PlayerEntity;
        player.setInvincible(true, duration);

    }

    public override void Project()
    {
        player = ObjectReference.Instance.PlayerEntity;
        player.Highlight();
    }

    public override void DeProject()
    {
        player.DeHighlight();
    }
}
