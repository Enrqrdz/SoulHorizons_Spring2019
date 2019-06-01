using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mantras : MonoBehaviour
{
    public static Mantras Instance { get; private set;}

    public const int MANTRASIZE = 4;
    public ActionData a_Mantra;
    public ActionData b_Mantra;
    public ActionData x_Mantra;
    public ActionData y_Mantra;
    public ActionData[] activeMantras = new ActionData[MANTRASIZE];

    private void OnValidate()
    {
        if (activeMantras.Length != MANTRASIZE)
        {
            Debug.LogWarning("Don't change the mantra size!");
            Array.Resize(ref activeMantras, MANTRASIZE);
        }
    }

    private void Awake()
    {
        IntiializeSingleton();
        InitializeMantras();
    }

    private void IntiializeSingleton()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeMantras()
    {
        activeMantras[0] = a_Mantra;
        activeMantras[1] = b_Mantra;
        activeMantras[2] = x_Mantra;
        activeMantras[3] = y_Mantra;
    }

    public void Activate(int index)
    {
        if (activeMantras[index] == null)
        {
            return;
        }

        StartCoroutine(ActivateHelper(index));
    }

    private IEnumerator ActivateHelper(int index)
    {
        if (activeMantras[index].castingTime != 0)
        {
            InputManager.canInputMantras = false;
            yield return new WaitForSeconds(activeMantras[index].castingTime);
            InputManager.canInputMantras = true;
        }
        activeMantras[index].Activate();
    }
}
