using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Cards/Pull")]
[RequireComponent(typeof(AudioSource))]

public class scr_Pull : ActionData
{
    private AudioSource PlayCardSFX;
    public AudioClip PullSFX;
    public AttackData pull;

    public override void Activate()
    {
        PlayCardSFX = GameObject.Find("ActionManager").GetComponent<AudioSource>();
        PlayCardSFX.clip =PullSFX;
        PlayCardSFX.Play();
         Entity player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();

        //add attack to attack controller script
        AttackController.Instance.AddNewAttack(pull, player._gridPos.x, player._gridPos.y, player);
    }

}
