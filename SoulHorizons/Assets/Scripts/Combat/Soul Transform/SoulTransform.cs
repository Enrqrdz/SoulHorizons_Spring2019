using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "SoulTransform")]
public class SoulTransform : ScriptableObject {

    public Element element;
    //script references
    //public MonoScript basicAttack;
    public bool hasMovement = false; //indicates whether this transform has movement that differs from normal player movement, and thus whether the movement variable will be null or not.
    //public MonoScript movement;
    public GameObject scriptHolder; //this will be a prefab that holds the scripts

    //List<Monobehavior> misc = new List<MonoBehavior<()

    //common attributes
    [Tooltip("The percentage of max hp added to the shield")]
    [Range(0, 200)]
    [SerializeField] int shieldGain = 50; //the amount of shield to add when performing this transform (Percentage of max health?)
    [Tooltip("Shield loss per second")]
    [SerializeField] int shieldDrainRate = 1; //the shield loss per second that this transform inflicts

    public int GetShieldGain()
    {
        return shieldGain;
    }

    public int GetShieldDrainRate()
    {
        return shieldDrainRate;
    }
}
