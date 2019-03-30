using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/SoulBlight")]
[RequireComponent(typeof(AudioSource))]

public class Scr_SoulBlight : ActionData 
{
    private AudioSource PlayCardSFX;
    public AudioClip BlightSFX;
    public AttackData blightAttack;

    public override void Activate()
    {

        PlayCardSFX = GameObject.Find("ActionManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = BlightSFX;
        PlayCardSFX.Play();

        Entity player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();

        //add attack to attack controller script
        //does a check to see if the target col is off the map

        AttackController.Instance.AddNewAttack(blightAttack, player._gridPos.x, player._gridPos.y, player);

    }
}
