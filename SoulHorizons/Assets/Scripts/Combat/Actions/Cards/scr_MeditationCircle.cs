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

    public override void Activate()
    {
        PlayCardSFX = GameObject.Find("ActionManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = MeditationSFX;
        PlayCardSFX.Play();

        Entity player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();

        scr_Grid.GridController.grid[player._gridPos.x, player._gridPos.y].BuffTile(activeDuration, damageMultiplier, damageReducer);
        GameObject MedCircleAnimation = Instantiate(MedCircleAnim, new Vector2(player._gridPos.x-0.3f, player._gridPos.y-0.2f),Quaternion.identity);
        Destroy(MedCircleAnimation, activeDuration);
    }
}
