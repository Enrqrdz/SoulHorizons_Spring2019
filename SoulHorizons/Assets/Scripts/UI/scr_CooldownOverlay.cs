using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class scr_CooldownOverlay : MonoBehaviour {

    private Image overlay;
    private GameObject overlayGameObject;
    private bool onCooldown = false;
    private float time = 1.5f;

	// Use this for initialization
	void Start () {
        overlayGameObject = FindGameObjectInChildWithTag(gameObject, "CooldownOverlay");
        overlay = overlayGameObject.GetComponent<Image>(); 
	}
	
	// Update is called once per frame
	void Update () {
        if (onCooldown)
        {
            overlay.fillAmount -= 1.0f / time * Time.deltaTime;

            if (overlay.fillAmount <= 0f)
            {
                onCooldown = false;
            }
        }
	}

    public static GameObject FindGameObjectInChildWithTag(GameObject parent, string tag)
    {
        Transform t = parent.transform;

        for (int i = 0; i < t.childCount; i++)
        {
            if (t.GetChild(i).gameObject.tag == tag)
            {
                return t.GetChild(i).gameObject;
            }

        }

        return null;
    }

    /// <summary>
    /// sets the time that the cooldown takes.
    /// </summary>
    /// <param name="time"></param>
    public void SetTime(float time)
    {
        this.time = time;
    }

    public void StartCooldown()
    {
        overlay.fillAmount = 1;
        onCooldown = true;
    }
}
