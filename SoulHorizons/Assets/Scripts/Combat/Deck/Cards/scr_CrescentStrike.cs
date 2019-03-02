using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
[CreateAssetMenu(menuName = "Cards/CrescentStrike")]
public class scr_CrescentStrike : CardData
{
    public AttackData crescentAttack;
    private AudioSource PlayCardSFX;
    public AudioClip BoomerangSFX;

    public override void Activate()
    {

        ActivateEffects();
        PlayCardSFX = GameObject.Find("DeckManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = BoomerangSFX;
        PlayCardSFX.Play();

        scr_Entity player = GameObject.FindGameObjectWithTag("Player").GetComponent<scr_Entity>();

        //add attack to attack controller script
        //does a check to see if the target col is off the map
        if (player._gridPos.y == scr_Grid.GridController.rowSizeMax - 1)
        {
            scr_AttackController.attackController.AddNewAttack(crescentAttack, player._gridPos.x + 2, player._gridPos.y, player);
        }
        else
        {
            scr_AttackController.attackController.AddNewAttack(crescentAttack, player._gridPos.x + 1, player._gridPos.y + 1, player);
        }
    }

    public override void StartCastingEffects()
    {

    }

    protected override void ActivateEffects()
    {
        //put start effects here
    }
}
