using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[CreateAssetMenu(menuName = "Cards/SwordCross")]

public class Scr_SwordCross : ActionData
{
    private AudioSource PlayCardSFX;
    public AudioClip SwordSFX;
    public AttackData swordAttack;

    public override void Activate()
    {
        Entity player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();

        PlayCardSFX = GameObject.Find("ActionManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = SwordSFX;
        PlayCardSFX.Play();

        //add attack to attack controller script
        //does a check to see if the target col is off the map
        if (player.GetComponent<Entity>()._gridPos.y == 0)
        {
            AttackController.Instance.AddNewAttack(swordAttack, player._gridPos.x + 1, player._gridPos.y + 1, player);
            AttackController.Instance.AddNewAttack(swordAttack, player._gridPos.x + 3, player._gridPos.y + 1, player);
        }
        else if (player.GetComponent<Entity>()._gridPos.y == scr_Grid.GridController.rowSizeMax - 1)
        {
            AttackController.Instance.AddNewAttack(swordAttack, player._gridPos.x + 1, player._gridPos.y - 1, player);
            AttackController.Instance.AddNewAttack(swordAttack, player._gridPos.x + 3, player._gridPos.y - 1, player);
        }
        else
        {
            AttackController.Instance.AddNewAttack(swordAttack, player._gridPos.x + 1, player._gridPos.y + 1, player);
            AttackController.Instance.AddNewAttack(swordAttack, player._gridPos.x + 1, player._gridPos.y - 1, player);
            AttackController.Instance.AddNewAttack(swordAttack, player._gridPos.x + 3, player._gridPos.y + 1, player);
            AttackController.Instance.AddNewAttack(swordAttack, player._gridPos.x + 3, player._gridPos.y - 1, player);
        }
    }
}
