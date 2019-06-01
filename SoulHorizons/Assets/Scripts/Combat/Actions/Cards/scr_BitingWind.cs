using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Biting Wind")]
[RequireComponent(typeof(AudioSource))]
public class scr_BitingWind : ActionData
{
    public AttackData bitingWindAttack;
    private AudioSource PlayCardSFX;
    public AudioClip BitingWindSFX;

    private Entity player;
    private int playerX, playerY;
    private int impactX;

    public override void Activate()
    {
        if (player == null)
        {
            player = ObjectReference.Instance.PlayerEntity;
        }

        playerX = player._gridPos.x;
        playerY = player._gridPos.y;

        PlayCardSFX = ObjectReference.Instance.ActionManager;
        PlayCardSFX.clip = BitingWindSFX;
        PlayCardSFX.Play();

        AttackController.Instance.AddNewAttack(bitingWindAttack, playerX, playerY, player);
    }

    public override void Project()
    {
        if (player == null)
        {
            player = ObjectReference.Instance.PlayerEntity;
        }

        playerX = player._gridPos.x;
        playerY = player._gridPos.y;

        impactX = Mathf.Min(playerX + bitingWindAttack.maxIncrementRange + 1, scr_Grid.GridController.columnSizeMax);

        for (int i = playerX + 1; i < impactX; i++)
        {
            scr_Grid.GridController.grid[i, playerY].Highlight();
        }
    }

    public override void DeProject()
    {
        for (int i = playerX + 1; i < impactX; i++)
        {
            scr_Grid.GridController.grid[i, playerY].DeHighlight();
        }
    }
}
