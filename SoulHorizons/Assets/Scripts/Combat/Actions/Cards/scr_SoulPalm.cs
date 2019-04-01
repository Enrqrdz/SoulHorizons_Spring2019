using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Cards/SoulPalm")]
[RequireComponent(typeof(AudioSource))]

public class scr_SoulPalm : ActionData
{
    private AudioSource PlayCardSFX;
    public AudioClip PalmSFX;
    public AttackData palmAttack;

    public override void Activate()
    {

        PlayCardSFX = GameObject.Find("ActionManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = PalmSFX;
        PlayCardSFX.Play();

        Entity player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();

        //add attack to attack controller script
        //does a check to see if the target col is off the map

        AttackController.Instance.AddNewAttack(palmAttack, player._gridPos.x + 1, player._gridPos.y, player);



    }
}
