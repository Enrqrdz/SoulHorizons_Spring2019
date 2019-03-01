using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Decision", menuName = "Decision")]
public class Decision : ScriptableObject
{
    public Button[] choices;
    public Consequence[] consequences;
}
