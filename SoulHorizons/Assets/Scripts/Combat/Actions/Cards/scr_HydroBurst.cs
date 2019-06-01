using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Hydro Burst")]
[RequireComponent(typeof(AudioSource))]
public class scr_HydroBurst : ActionData
{
    public AttackData boltAttack;
    private AudioSource PlayCardSFX;
    public AudioClip BoltSFX;

    public float secondBoltDelay;
    [Tooltip("Add the second bolt delay to this time")]
    public float thirdBoltDelay;

    private Entity player;
    private int playerX, playerY;
    private int impactX;

    public override void Project()
    {
        if (player == null)
        {
            player = ObjectReference.Instance.PlayerEntity;
        }

        playerX = player._gridPos.x;
        playerY = player._gridPos.y;

        impactX = Mathf.Min(playerX + boltAttack.maxIncrementRange + 1, scr_Grid.GridController.columnSizeMax);

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
        if (player == null)
        {
            player = ObjectReference.Instance.PlayerEntity;
        }

        playerX = player._gridPos.x;
        playerY = player._gridPos.y;

        PlayCardSFX = ObjectReference.Instance.ActionManager;
        PlayCardSFX.clip = BoltSFX;
        PlayCardSFX.Play();

        AttackController.Instance.AddNewAttack(boltAttack, playerX, playerY, player);
        player.StartCoroutine(FireBolt(secondBoltDelay, boltAttack));
        player.StartCoroutine(FireBolt(thirdBoltDelay, boltAttack));
    }
    public IEnumerator FireBolt(float delay, AttackData boltAttack)
    {
        yield return new WaitForSeconds(delay);
        playerX = player._gridPos.x;
        playerY = player._gridPos.y;
        AttackController.Instance.AddNewAttack(boltAttack, playerX, playerY, ObjectReference.Instance.PlayerEntity);
    }

}
