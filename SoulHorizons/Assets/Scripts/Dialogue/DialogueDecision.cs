using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Decision", menuName = "Decision")]
public class DialogueDecision : ScriptableObject
{
    [HideInInspector]
    public const int choiceSize = 4;

    public GameObject[] choices = new GameObject[choiceSize];
}
