using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Portrait", menuName = "Portrait")]
public class Dialogue: ScriptableObject
{
    [Tooltip("Who is talking and what they're saying.")]
    public List<DialogueBox> dialogueBox = new List<DialogueBox>();

    [Tooltip("Decisions for the entire scene.")]
    public List<Decision> decisions = new List<Decision>();

    [Tooltip("Indexes for the decisions")]
    public List<int> decisionIndexes = new List<int>();
}