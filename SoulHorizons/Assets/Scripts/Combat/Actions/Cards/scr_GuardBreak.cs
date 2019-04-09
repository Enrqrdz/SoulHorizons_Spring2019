using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/GuardBreak")]
[RequireComponent(typeof(AudioSource))]

public class scr_GuardBreak : ActionData
{
    private Entity player;
    private Entity enemy;
    private AudioSource PlayCardSFX;
    public AudioClip BreakSFX;
    public AttackData BreakAttack;
    public int GuardbreakDamage = 5;
    public float teleportTime = 0.5f;
    int playerX;
    int playerY;

    public override void Activate()
    {

        PlayCardSFX = GameObject.Find("ActionManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = BreakSFX;
        PlayCardSFX.Play();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();

        playerX = player._gridPos.x;
        playerY = player._gridPos.y;

        Debug.Log("Player X" + playerX);
        Debug.Log("Player Y" + playerY);

        for (int i = 0; i < scr_Grid.GridController.columnSizeMax; i++)
        {
            enemy = scr_Grid.GridController.GetEntityAtPosition(i, playerY);

            if (enemy != null && enemy.type == EntityType.Enemy)
            {
                player.SetTransform(enemy._gridPos.x - 1, enemy._gridPos.y);
                Debug.Log("Teleport!");
                player.StartCoroutine(player.Teleport(teleportTime, GuardbreakDamage, playerX, playerY, enemy));
                break;
            }
        }
        //AttackController.Instance.AddNewAttack(BreakAttack, player._gridPos.x + 1, player._gridPos.y, player);
        //scr_Grid.GridController.activeEntities[i].type == EntityType.Enemy
    }
}
