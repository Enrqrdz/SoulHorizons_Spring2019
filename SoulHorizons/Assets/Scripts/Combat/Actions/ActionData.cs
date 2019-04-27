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
    public string actionName;
    public Sprite art;
    public Element element;
    public int transformChargeAmount = 5;
    public float castingTime = 0f; 
    public float cooldown = 1f;
    [Multiline]
    public string description;

    public abstract void Activate();
    public abstract void Project();
    /// <summary>
    /// Currently relies on Project funtion to obtain player x and y and uses that as a reference in DeProject.
    /// </summary>
    public abstract void DeProject();
}
