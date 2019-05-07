using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Cards/SoulPalm")]
[RequireComponent(typeof(AudioSource))]

public class scr_SoulPalm : ActionData
{
    private AudioSource PlayCardSFX;
    public AudioClip PalmSFX;
    public AttackData palmAttack;
    private Entity player;
    private int playerX, playerY;
    private int[] xPositions = new int[2];
    private int[] yPositions = new int[3];

    public override void Activate()
    {
        PlayCardSFX = ObjectReference.Instance.ActionManager;
        PlayCardSFX.clip = PalmSFX;
        PlayCardSFX.Play();

        playerX = player._gridPos.x;
        playerY = player._gridPos.y;

        for (int i = 0; i < xPositions.Length; i++)
        {
            xPositions[i] = playerX + i + 1;
        }

        if (playerY == 0)
        {
            for (int i = 0; i < yPositions.Length; i++)
            {
                yPositions[i] = playerY + i;
            }
        }
        else if (playerY == scr_Grid.GridController.rowSizeMax - 1)
        {
            for (int i = 0; i < yPositions.Length; i++)
            {
                yPositions[i] = playerY + i - 2;
            }
        }
        else
        {
            for (int i = 0; i < yPositions.Length; i++)
            {
                yPositions[i] = playerY + i - 1;
            }
        }

        //AttackController.Instance.AddNewAttack(palmAttack, playerX + 1, playerY, player);

        for (int i = 0; i < xPositions.Length; i++)
        {
            for (int j = 0; j < yPositions.Length; j++)
            {
                AttackController.Instance.AddNewAttack(palmAttack, xPositions[i], yPositions[j], player);
            }
        }
    }

    public override void Project()
    {
        player = ObjectReference.Instance.PlayerEntity;
        playerX = player._gridPos.x;
        playerY = player._gridPos.y;

        for (int i = 0; i < xPositions.Length; i++)
        {
            xPositions[i] = playerX + i + 1;
        }

        if (playerY == 0)
        {
            for (int i = 0; i < yPositions.Length; i++)
            {
                yPositions[i] = playerY + i;
            }
        }
        else if(playerY == scr_Grid.GridController.rowSizeMax - 1)
        {
            for (int i = 0; i < yPositions.Length; i++)
            {
                yPositions[i] = playerY + i - 2;
            }
        }
        else
        {
            for (int i = 0; i < yPositions.Length; i++)
            {
                yPositions[i] = playerY + i - 1;
            }
        }

        for (int i = 0; i < xPositions.Length; i++)
        {
            for (int j = 0; j < yPositions.Length; j++)
            {
                scr_Grid.GridController.grid[xPositions[i], yPositions[j]].Highlight();
            }
        }
    }

    public override void DeProject()
    {
        for (int i = 0; i < xPositions.Length; i++)
        {
            for (int j = 0; j < yPositions.Length; j++)
            {
                scr_Grid.GridController.grid[xPositions[i], yPositions[j]].DeHighlight();
            }
        }
    }
}
