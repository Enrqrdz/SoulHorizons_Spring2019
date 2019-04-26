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

    public override void Activate()
    {
        PlayCardSFX = GameObject.Find("ActionManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = PalmSFX;
        PlayCardSFX.Play();

        if (playerY == 0)
        {
            AttackController.Instance.AddNewAttack(palmAttack, playerX + 1, playerY + 0, player);
            AttackController.Instance.AddNewAttack(palmAttack, playerX + 1, playerY + 1, player);
            AttackController.Instance.AddNewAttack(palmAttack, playerX + 1, playerY + 2, player);
        }
        else if (playerY == scr_Grid.GridController.rowSizeMax - 1)
        {
            AttackController.Instance.AddNewAttack(palmAttack, playerX + 1, playerY + 0, player);
            AttackController.Instance.AddNewAttack(palmAttack, playerX + 1, playerY - 1, player);
            AttackController.Instance.AddNewAttack(palmAttack, playerX + 1, playerY - 2, player);
        }
        else
        {
            AttackController.Instance.AddNewAttack(palmAttack, playerX + 1, playerY + 0, player);
            AttackController.Instance.AddNewAttack(palmAttack, playerX + 1, playerY + 1, player);
            AttackController.Instance.AddNewAttack(palmAttack, playerX + 1, playerY - 1, player);
        }
    }

    public override void Project()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();
        playerX = player._gridPos.x;
        playerY = player._gridPos.y;

        if(playerY == 0)
        {
            scr_Grid.GridController.grid[playerX + 1, playerY + 0].Highlight();
            scr_Grid.GridController.grid[playerX + 1, playerY + 1].Highlight();
            scr_Grid.GridController.grid[playerX + 1, playerY + 2].Highlight();
            scr_Grid.GridController.grid[playerX + 2, playerY + 0].Highlight();
            scr_Grid.GridController.grid[playerX + 2, playerY + 1].Highlight();
            scr_Grid.GridController.grid[playerX + 2, playerY + 2].Highlight();
        }
        else if(playerY == scr_Grid.GridController.rowSizeMax - 1)
        {
            scr_Grid.GridController.grid[playerX + 1, playerY + 0].Highlight();
            scr_Grid.GridController.grid[playerX + 1, playerY - 1].Highlight();
            scr_Grid.GridController.grid[playerX + 1, playerY - 2].Highlight();
            scr_Grid.GridController.grid[playerX + 2, playerY + 0].Highlight();
            scr_Grid.GridController.grid[playerX + 2, playerY - 1].Highlight();
            scr_Grid.GridController.grid[playerX + 2, playerY - 2].Highlight();
        }
        else
        {
            scr_Grid.GridController.grid[playerX + 1, playerY + 0].Highlight();
            scr_Grid.GridController.grid[playerX + 1, playerY + 1].Highlight();
            scr_Grid.GridController.grid[playerX + 1, playerY - 1].Highlight();
            scr_Grid.GridController.grid[playerX + 2, playerY + 0].Highlight();
            scr_Grid.GridController.grid[playerX + 2, playerY + 1].Highlight();
            scr_Grid.GridController.grid[playerX + 2, playerY - 1].Highlight();
        }
    }

    public override void DeProject()
    {
        if (playerY == 0)
        {
            scr_Grid.GridController.grid[playerX + 1, playerY + 0].DeHighlight();
            scr_Grid.GridController.grid[playerX + 1, playerY + 1].DeHighlight();
            scr_Grid.GridController.grid[playerX + 1, playerY + 2].DeHighlight();
            scr_Grid.GridController.grid[playerX + 2, playerY + 0].DeHighlight();
            scr_Grid.GridController.grid[playerX + 2, playerY + 1].DeHighlight();
            scr_Grid.GridController.grid[playerX + 2, playerY + 2].DeHighlight();
        }
        else if (playerY == scr_Grid.GridController.rowSizeMax - 1)
        {
            scr_Grid.GridController.grid[playerX + 1, playerY + 0].DeHighlight();
            scr_Grid.GridController.grid[playerX + 1, playerY - 1].DeHighlight();
            scr_Grid.GridController.grid[playerX + 1, playerY - 2].DeHighlight();
            scr_Grid.GridController.grid[playerX + 2, playerY + 0].DeHighlight();
            scr_Grid.GridController.grid[playerX + 2, playerY - 1].DeHighlight();
            scr_Grid.GridController.grid[playerX + 2, playerY - 2].DeHighlight();
        }
        else
        {
            scr_Grid.GridController.grid[playerX + 1, playerY + 0].DeHighlight();
            scr_Grid.GridController.grid[playerX + 1, playerY + 1].DeHighlight();
            scr_Grid.GridController.grid[playerX + 1, playerY - 1].DeHighlight();
            scr_Grid.GridController.grid[playerX + 2, playerY + 0].DeHighlight();
            scr_Grid.GridController.grid[playerX + 2, playerY + 1].DeHighlight();
            scr_Grid.GridController.grid[playerX + 2, playerY - 1].DeHighlight();
        }
    }
}
