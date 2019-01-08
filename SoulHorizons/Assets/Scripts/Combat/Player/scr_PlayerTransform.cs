using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_PlayerTransform : MonoBehaviour {

    bool powerup; //whether player is in soul form or not
    float timer; //timer that keeps track of time elapsed to end soul form
    public int soultime = 3; //amount of seconds player spends in soul form
	// Use this for initialization
	void Start () {
        timer = 0;
        powerup = false;
	}
	
	// Update is called once per frame
	void Update () {
       if(timer != 0)
        {
            timer-=Time.deltaTime;
            if(timer <= 0)
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.black;
                powerup = false;
                Debug.Log("Tranform Done.");
            }
        }
        if (Input.GetKey(KeyCode.R) && powerup == false)
        {
            Debug.Log("Transform!");
            gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
            powerup = true;
            timer = soultime * 60 * Time.deltaTime;
        }

    }
    private void FixedUpdate()
    {
        
    }
}
