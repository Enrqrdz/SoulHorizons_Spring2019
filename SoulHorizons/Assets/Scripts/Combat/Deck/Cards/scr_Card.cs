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
public abstract class scr_Card : ScriptableObject {


    public List<string> keywords;
    public string cardName;
    public Element element;
    public int chargeAmount = 5; //the amount that the soul transform for this element is charged when the card is played
    public float castingTime = 1f; //the delay before the card is cast
    public float cooldown = 1.5f; //the cooldown after casting this card in seconds, after which another card may be cast
    [Multiline]
    public string description;
    public Sprite art;
    public abstract void StartCastingEffects(); //call this before waiting the casting time in deck script. This could be used to trigger certain player animations indicating that
                                                //the spell is about to be cast
    public abstract void Activate();
    protected abstract void ActivateEffects(); //call in activate to trigger any visual/audio effects
    
}
