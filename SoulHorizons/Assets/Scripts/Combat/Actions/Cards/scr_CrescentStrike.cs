using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

[CreateAssetMenu(menuName = "Cards/CrescentStrike")]
public class scr_CrescentStrike : ActionData
{
    private AudioSource PlayCardSFX;
    public AudioClip CrescentSFX;
    public AttackData crescentAttack;

    public override void Activate()
    {
        
        PlayCardSFX = GameObject.Find("ActionManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = CrescentSFX;
        PlayCardSFX.Play();

        Entity player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();

        //add attack to attack controller script
        //does a check to see if the target col is off the map
        if (player.GetComponent<Entity>()._gridPos.y == 0)
        {
            AttackController.Instance.AddNewAttack(crescentAttack, player._gridPos.x + 1, player._gridPos.y, player);
        }
        else
        {
            AttackController.Instance.AddNewAttack(crescentAttack, player._gridPos.x + 1, player._gridPos.y - 1, player);
        }

    }
}
