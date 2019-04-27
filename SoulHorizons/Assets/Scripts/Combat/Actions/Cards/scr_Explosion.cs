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
        PlayCardSFX = ObjectReference.Instance.ActionManager;
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

    public override void Project()
    {
        throw new System.NotImplementedException();
    }

    public override void DeProject()
    {
        throw new System.NotImplementedException();
    }
}
