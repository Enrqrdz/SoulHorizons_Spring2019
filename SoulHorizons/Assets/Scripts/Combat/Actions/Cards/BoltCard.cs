using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Bolt")]
[RequireComponent(typeof(AudioSource))]

public class BoltCard : ActionData
{
    public AttackData boltAttack;
    private AudioSource PlayCardSFX;
    public AudioClip BoltSFX;
    private Entity player;
    private int playerX, playerY;
    private int impactX;

    public override void Project()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
        }

        playerX = player._gridPos.x;
        playerY = player._gridPos.y;

        impactX = Mathf.Min(playerX + boltAttack.maxIncrementRange, scr_Grid.GridController.columnSizeMax);

        for (int i = playerX + 1; i < impactX; i++)
        {
            if (scr_Grid.GridController.CheckIfOccupied(i, playerY) == false)
            {
                scr_Grid.GridController.grid[i, playerY].Highlight();
            }
            else
            {
                scr_Grid.GridController.grid[i, playerY].Highlight();
                break;
            }
        }
    }

    public override void DeProject()
    {
        for (int i = playerX + 1; i < impactX; i++)
        {
            if (scr_Grid.GridController.CheckIfOccupied(i, playerY) == false)
            {
                scr_Grid.GridController.grid[i, playerY].DeHighlight();
            }
            else
            {
                scr_Grid.GridController.grid[i, playerY].DeHighlight();
                break;
            }
        }
    }

    public override void Activate()
    {
        PlayCardSFX = GameObject.Find("ActionManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = BoltSFX;
        PlayCardSFX.Play();

        AttackController.Instance.AddNewAttack(boltAttack, playerX, playerY, player);
    }
}