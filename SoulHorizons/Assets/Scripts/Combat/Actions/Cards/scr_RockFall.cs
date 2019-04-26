using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/RockFall")]
[RequireComponent(typeof(AudioSource))]

public class scr_RockFall : ActionData
{
    public AttackData RockFallAttack;
    private AudioSource PlayCardSFX;
    public AudioClip RockFallSFX;

    public override void Activate()
    {

        Entity player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();

        //add attack to attack controller script
        if (player._gridPos.x + 2 < scr_Grid.GridController.columnSizeMax - 1)
        {
            AttackController.Instance.AddNewAttack(RockFallAttack, player._gridPos.x + 2, player._gridPos.y, player);
        }
        else
        {

        }
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
