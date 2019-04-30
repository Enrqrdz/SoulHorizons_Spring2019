using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/MeditationCircle")]
[RequireComponent(typeof(AudioSource))]

public class scr_MeditationCircle : ActionData
{
    private AudioSource PlayCardSFX;
    public AudioClip MeditationSFX;
    public GameObject MedCircleAnim;

    public float activeDuration;
    public float damageMultiplier;
    public float damageReducer;

    private Entity player;
    private int playerX, playerY;

    public override void Project()
    {
        if(player == null)
        {
            player = ObjectReference.Instance.PlayerEntity;
        }
        playerX = player._gridPos.x;
        playerY = player._gridPos.y;

        scr_Grid.GridController.grid[playerX, playerY].Highlight();
    }

    public override void DeProject()
    {
        scr_Grid.GridController.grid[playerX, playerY].DeHighlight();
    }

    public override void Activate()
    {
        if (PlayCardSFX == null)
        {
            PlayCardSFX = ObjectReference.Instance.ActionManager;
        }
        PlayCardSFX.clip = MeditationSFX;
        PlayCardSFX.Play();
        if (player == null)
        {
            player = ObjectReference.Instance.PlayerEntity;
        }
        playerX = player._gridPos.x;
        playerY = player._gridPos.y;

        scr_Grid.GridController.grid[playerX, playerY].BuffTile(activeDuration, damageMultiplier, damageReducer);
        GameObject MedCircleAnimation = Instantiate(MedCircleAnim, new Vector2(player.transform.position.x, player.transform.position.y+0.3f),Quaternion.identity);
        Destroy(MedCircleAnimation, activeDuration);
    }
}
