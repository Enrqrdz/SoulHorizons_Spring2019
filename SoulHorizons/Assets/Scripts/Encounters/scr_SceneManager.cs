using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class scr_SceneManager : MonoBehaviour {

    public Encounter currentEncounter;
    public static scr_SceneManager globalSceneManager;
    public int currentEncounterNumber;
    public static bool canSwitch = true;
	
	void Start () {

        if(globalSceneManager != null && globalSceneManager != this)
        {
            Destroy(gameObject); 
        }
        else
        {
            globalSceneManager = this;
            DontDestroyOnLoad(this.gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
        LockCursor(); 
	}

	public void ChangeScene(string sceneName){
		if(canSwitch)
            SceneManager.LoadScene(sceneName);
	}

    public void EnableSettings()
    {
        Debug.Log("SETTINGS ARE OPEN");

    }
    public void DisableSettings()
    {
        Debug.Log("settings are closed");
    }

    public void QuitGame()
    {
        Application.Quit();
        //Deletes save files if you press the quit button - Used for debugging, delete line when done
        SaveManager.Clear();
        Debug.Log("quit"); 
    }

    void LockCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; 
    }
    public string ReturnSceneName()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        return SceneManager.GetActiveScene().name;
    }
}
