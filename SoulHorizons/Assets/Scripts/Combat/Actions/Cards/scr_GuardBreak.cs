using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/GuardBreak")]
[RequireComponent(typeof(AudioSource))]

public class scr_GuardBreak : ActionData
{
    private AudioSource PlayCardSFX;
    public AudioClip BreakSFX;
    public AttackData BreakAttack;

    public override void Activate()
    {

        PlayCardSFX = GameObject.Find("ActionManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = BreakSFX;
        PlayCardSFX.Play();

        Entity player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();

        //add attack to attack controller script
        //does a check to see if the target col is off the map

        AttackController.Instance.AddNewAttack(BreakAttack, player._gridPos.x, player._gridPos.y, player);

    }
}
