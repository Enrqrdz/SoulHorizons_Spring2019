using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Cards/Bolt")]
[RequireComponent(typeof(AudioSource))]

public class scr_Bolt : CardData {

    public AttackData boltAttack;
    private AudioSource PlayCardSFX;
    public AudioClip BoltSFX;

    public override void Activate()
    {
        ActivateEffects();
        PlayCardSFX = GameObject.Find("DeckManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = BoltSFX;
        PlayCardSFX.Play();
        Entity player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();

        //add attack to attack controller script
        scr_AttackController.attackController.AddNewAttack(boltAttack, player._gridPos.x, player._gridPos.y, player);
    }

    public override void StartCastingEffects()
    {
        
    }

        protected override void ActivateEffects()
    {
        //put start effects here
    }

}
