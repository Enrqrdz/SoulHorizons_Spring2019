using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLaunch : MonoBehaviour {

	void Start () {	
	}

    public void NewGame()
    {
        SaveManager.NewSave();
    }

    public void Play()
    {
        SaveManager.Load();
    }

}
