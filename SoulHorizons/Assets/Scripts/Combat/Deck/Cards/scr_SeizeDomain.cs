using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Cards/SeizeDomain")]
[RequireComponent(typeof(AudioSource))]

public class scr_SeizeDomain : scr_Card
{
    public float duration;
    //public Color newColor;
    private AudioSource PlayCardSFX;
    public AudioClip SeizeDomainSFX;
    public override void Activate()
    {
      
        ActivateEffects();
        PlayCardSFX = GameObject.Find("DeckManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = SeizeDomainSFX;
        PlayCardSFX.Play();
        Debug.Log("SEIZE!");
        scr_Grid.GridController.seizeDomain(duration);
        
       

    }

    public override void StartCastingEffects()
    {

    }

    protected override void ActivateEffects()
    {
        //put start effects here
    }
}
