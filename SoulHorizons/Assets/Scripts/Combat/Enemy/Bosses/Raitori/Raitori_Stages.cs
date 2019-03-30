using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raitori_Stages : MonoBehaviour
{
    public Phase currentPhase;

    //Stage1
    [Tooltip("Damage required before first phase transition")]
    public int phase1requirement = 100;

    //Stage2
    [Tooltip("Damage required before second phase transition")]
    public int phase2requirement = 100;

    //Stage3
}
