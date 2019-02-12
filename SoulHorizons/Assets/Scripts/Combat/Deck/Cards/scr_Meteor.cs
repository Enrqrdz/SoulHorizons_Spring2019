using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Cards/Meteor")]
public class scr_Meteor : CardData {

    public AttackData meteorAttack;
    public int meteorOffset = 3;

    public override void Activate()
    {
        scr_Entity player = GameObject.FindGameObjectWithTag("Player").GetComponent<scr_Entity>();

        //add attack to attack controller script
        int columnPosition = Mathf.Min(player._gridPos.x + meteorOffset, scr_Grid.GridController.maxColumnSize - 1);
        int rowPosition = player._gridPos.y;

        if(rowPosition == scr_Grid.GridController.maxRowSize - 1)
        {
            rowPosition = scr_Grid.GridController.maxRowSize - 2;
        }
        if (rowPosition == 0)
        {
            rowPosition = 2;
        }

        scr_AttackController.attackController.AddNewAttack(meteorAttack, columnPosition, rowPosition, player);
    }

    public override void StartCastingEffects()
    {

    }

    protected override void ActivateEffects()
    {
        //put start effects here
    }
}
