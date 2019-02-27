using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[CreateAssetMenu(menuName = "Cards/InnerStrength")]
[RequireComponent(typeof(AudioSource))]

public class scr_InnerStrength : ActionData {

    public float multiplier;
    public float duration;
    private AudioSource PlayCardSFX;
    public AudioClip InnerStrengthSFX;

    public override void Activate()
    {
        PlayCardSFX = GameObject.Find("ActionManager").GetComponent<AudioSource>();
        PlayCardSFX.clip = InnerStrengthSFX;
        PlayCardSFX.Play();
        scr_PlayerBlaster blaster = GameObject.FindGameObjectWithTag("Player").GetComponent<scr_PlayerBlaster>();
        scr_statemanager manager = GameObject.FindGameObjectWithTag("StateManager").GetComponent<scr_statemanager>();
        blaster.setMultiplier(multiplier, duration);
        manager.ChangeEffects("Attack Up: " + multiplier, duration);
    }
}
