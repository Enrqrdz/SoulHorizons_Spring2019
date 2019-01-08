using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Cards/Cleave")]
[RequireComponent(typeof(AudioSource))]

public class scr_Cleave : scr_Card
{
	public Attack attack;
    private AudioSource PlayCardSFX;
    public AudioClip CleaveSFX;

    public override void Activate()
    {
        PlayCardSFX = GameObject.Find("DeckManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = CleaveSFX;
        PlayCardSFX.Play();
        scr_Entity player = GameObject.FindGameObjectWithTag("Player").GetComponent<scr_Entity>();
		if(scr_Grid.GridController.LocationOnGrid(player._gridPos.x + 1, player._gridPos.y - 1))
		{
        	scr_AttackController.attackController.AddNewAttack(attack, player._gridPos.x + 1, player._gridPos.y - 1, player);
		}
		else
		{
			scr_AttackController.attackController.AddNewAttack(attack, player._gridPos.x + 1, player._gridPos.y , player);
		}
    }

    public override void StartCastingEffects()
    {
        
    }

    protected override void ActivateEffects()
    {
        //trigger the cleave effect here
    }
}
