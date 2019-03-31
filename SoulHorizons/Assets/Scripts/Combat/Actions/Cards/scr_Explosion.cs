using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Explosion")]
[RequireComponent(typeof(AudioSource))]

public class scr_Explosion : ActionData
{
    private AudioSource PlayCardSFX;
    public AudioClip ExplosionSFX;
    public AttackData ExplosionMain;
    public AttackData ExplosionSide;

    public override void Activate()
    {
        PlayCardSFX = GameObject.Find("ActionManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = ExplosionSFX;
        PlayCardSFX.Play();
        Entity player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();

        //add attack to attack controller script

        try
        {
            AttackController.Instance.AddNewAttack(ExplosionMain, player._gridPos.x + 3, player._gridPos.y, player);
        }
        catch
        { }
        try
        {
            AttackController.Instance.AddNewAttack(ExplosionSide, player._gridPos.x + 3, player._gridPos.y + 1, player);
        }
        catch
        { }
        try
        {
            AttackController.Instance.AddNewAttack(ExplosionSide, player._gridPos.x + 3, player._gridPos.y - 1, player);
        }
        catch
        { }
        try
        {
            AttackController.Instance.AddNewAttack(ExplosionSide, player._gridPos.x + 4, player._gridPos.y, player);
        }
        catch
        { }

        try
        {
            AttackController.Instance.AddNewAttack(ExplosionSide, player._gridPos.x + 2, player._gridPos.y, player);
        }
        catch
        { }   
    }

}
