using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element
{
    Earth,
    Soul,
    Sun,
    Void,
    Wind
}

[System.Serializable]
public abstract class ActionData : ScriptableObject
{
    public string name;
    public Sprite art;
    public Element element;
    public int soulTransformCharge = 5;
    public float delayBeforeCast = 0f;
    public float cooldown = 1f;
    [Multiline]
    public string description;

    public abstract void StartCastingEffects();
    public abstract void Activate();
    protected abstract void ActivateEffects();
}
