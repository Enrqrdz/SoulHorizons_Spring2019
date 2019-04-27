using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/WaterPrison")]//Jutsu
[RequireComponent(typeof(AudioSource))]
public class Scr_WaterPrison : ActionData
{
    public AttackData WaterPrison;
    public AudioClip WaterPrisonSFX;
    private AudioSource PlayCardSFX;

    public override void Activate()
    {

        Entity player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();

        //add attack to attack controller script
        AttackController.Instance.AddNewAttack(WaterPrison, player._gridPos.x, player._gridPos.y, player);
    }

    public override void DeProject()
    {
        throw new System.NotImplementedException();
    }

    public override void Project()
    {
        throw new System.NotImplementedException();
    }
}
