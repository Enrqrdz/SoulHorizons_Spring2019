using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue: ScriptableObject
{
    [Tooltip("Who is talking for the entire scene.")]
    public List<Portrait> characters = new List<Portrait>();

    [Tooltip("Script for the entire scene.")]
    public List<string> text = new List<string> ();

    [Tooltip("Decisions for the entire scene.")]
    public List<Decision> decisions = new List<Decision>();

    [Tooltip("Indexes for the decisions.")]
    public List<int> decisionIndexes = new List<int>();
}