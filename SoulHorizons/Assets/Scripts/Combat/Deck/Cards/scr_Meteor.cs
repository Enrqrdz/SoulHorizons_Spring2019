﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Cards/Meteor")]
public class scr_Meteor : CardData {

    public AttackData meteorAttack;

    public override void Activate()
    {
        
        ActivateEffects();

        Entity player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();

        //add attack to attack controller script
        //does a check to see if the target col is off the map 
        AttackController.Instance.AddNewAttack(meteorAttack, Mathf.Min(player._gridPos.x + 3,(scr_Grid.GridController.columnSizeMax-1)), 1, player);
    }

    public override void StartCastingEffects()
    {

    }

    protected override void ActivateEffects()
    {
        //put start effects here
    }
}
