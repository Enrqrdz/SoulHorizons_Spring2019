using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Cards/SeizeDomain")]
[RequireComponent(typeof(AudioSource))]

public class scr_SeizeDomain : ActionData
{
    public float duration;
    //public Color newColor;
    private AudioSource PlayCardSFX;
    public AudioClip SeizeDomainSFX;
    public override void Activate()
    {    
        PlayCardSFX = GameObject.Find("ActionManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = SeizeDomainSFX;
        PlayCardSFX.Play();
        scr_Grid.GridController.seizeDomain(duration);
    }
}
