using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/CrashingWave")]
[RequireComponent(typeof(AudioSource))]
public class scr_CrashingWave : ActionData
{
    public AttackData CrashingWave;
    public AudioClip WaveSFX;
    private AudioSource PlayCardSFX;

    public override void Activate()
    {

        Entity player = GameObject.FindGameObjectWithTag("Player").GetComponent<Entity>();

        //add attack to attack controller script
        for(int i = 0; i < scr_Grid.GridController.columnSizeMax; i++)
        {
            AttackController.Instance.AddNewAttack(CrashingWave, (player._gridPos.x + 1), i, player);
        }
    }
}
