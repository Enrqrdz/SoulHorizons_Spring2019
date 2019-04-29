using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (menuName = "Cards/LightningBolt")]
[RequireComponent(typeof(AudioSource))]

public class scr_LightningBolt : ActionData {

    public float damage = 6f;
    private AudioSource PlayCardSFX;
    public AudioClip LightningBoltSFX;

    public override void Activate()
    {
        PlayCardSFX = ObjectReference.Instance.ActionManager;
        PlayCardSFX.clip = LightningBoltSFX;
        PlayCardSFX.Play();
        //implement functionality here
        Debug.Log(actionName + ": Zap!");
    }

    public override void Project()
    {
        throw new System.NotImplementedException();
    }

    public override void DeProject()
    {
        throw new System.NotImplementedException();
    }
}
