using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Updraft")]
[RequireComponent(typeof(AudioSource))]

public class scr_UpDraft : ActionData
{
    public AttackData UpdraftMain;
    public AttackData UpdraftSide;
    public AudioClip UpdraftSFX;
    private AudioSource PlayCardSFX;

    public override void Activate()
    {

        Entity player = ObjectReference.Instance.PlayerEntity;

        if (scr_Grid.GridController.LocationOnGrid((player._gridPos.x + 3), player._gridPos.y) == true)
        {
            player.isStunned = true;
            player.spr.color = Color.blue;
            player.invincible = true;
            AttackController.Instance.AddNewAttack(UpdraftMain, (player._gridPos.x + 3), player._gridPos.y, player);
            try
            {
                AttackController.Instance.AddNewAttack(UpdraftSide, (player._gridPos.x + 4), player._gridPos.y, player);
            }
            catch
            { }
            try
            {
                AttackController.Instance.AddNewAttack(UpdraftSide, (player._gridPos.x + 3), player._gridPos.y + 1, player);
            }
            catch
            { }
            try
            {
                AttackController.Instance.AddNewAttack(UpdraftSide, (player._gridPos.x + 3), player._gridPos.y - 1, player);
            }
            catch
            { }
            try
            {
                AttackController.Instance.AddNewAttack(UpdraftSide, (player._gridPos.x + 2), player._gridPos.y, player);
            }
            catch
            { }
        }
        
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
