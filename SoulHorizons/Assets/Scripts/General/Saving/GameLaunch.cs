using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Colin 9/15/18

/// <summary>
/// Run this when the game starts.
/// </summary>
public class GameLaunch : MonoBehaviour {

	void Start () {	
	}

    public void NewGame()
    {
        SaveLoad.NewGame();
    }

    public void Play()
    {
        SaveLoad.Load();
    }

}
