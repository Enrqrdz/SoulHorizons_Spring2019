using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/LifeLeech")]
[RequireComponent(typeof(AudioSource))]

public class scr_LifeLeech : ActionData
{
    public AttackData LifeLeech;
    public AudioClip LeechSFX;
    private AudioSource PlayCardSFX;

    public override void Activate()
    {

        Entity player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();

         AttackController.Instance.AddNewAttack(LifeLeech, (player._gridPos.x + 1), player._gridPos.y, player);
    }
}
