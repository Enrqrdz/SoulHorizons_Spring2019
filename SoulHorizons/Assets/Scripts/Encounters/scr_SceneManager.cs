using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class scr_SceneManager : MonoBehaviour {

    public Encounter currentEncounter;
    public static scr_SceneManager globalSceneManager;
    public int currentEncounterNumber;
    public static bool canSwitch = true;
	
	void Start ()
    {
        SetGlobalSceneManager();
    }

    private void SetGlobalSceneManager()
    {
        //If there is a scene manager and it is not this game object, destroy this game object
        if (globalSceneManager != null && globalSceneManager != this)
        {
            Destroy(gameObject);
        }
        else
        {
            globalSceneManager = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void ChangeScene(string sceneName){
		if(canSwitch)SceneManager.LoadScene (sceneName);
	}

    public void QuitGame()
    {
        Application.Quit();
    }

    public static void ClearSaveFiles()
    {
        //Deletes save files if you press the quit button - Used for debugging, delete line when done
        SaveLoad.Clear();
    }

    public string ReturnSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}
