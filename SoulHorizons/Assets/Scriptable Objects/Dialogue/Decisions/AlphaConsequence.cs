using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaConsequence : MonoBehaviour, IConsequence
{
    public void Consequence()
    {
        scr_SceneManager.globalSceneManager.ChangeScene(SceneNames.REGION);
    }
}
