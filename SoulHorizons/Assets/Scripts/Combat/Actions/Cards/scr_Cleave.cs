using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Cards/Cleave")]
[RequireComponent(typeof(AudioSource))]

public class scr_Cleave : ActionData
{
	public AttackData attack;
    private AudioSource PlayCardSFX;
    public AudioClip CleaveSFX;

    public override void Activate()
    {
        PlayCardSFX = GameObject.Find("ActionManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = CleaveSFX;
        PlayCardSFX.Play();
        Entity player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
		if(scr_Grid.GridController.LocationOnGrid(player._gridPos.x + 1, player._gridPos.y - 1))
		{
        	AttackController.Instance.AddNewAttack(attack, player._gridPos.x + 1, player._gridPos.y - 1, player);
		}
		else
		{
			AttackController.Instance.AddNewAttack(attack, player._gridPos.x + 1, player._gridPos.y , player);
		}
    }
}
