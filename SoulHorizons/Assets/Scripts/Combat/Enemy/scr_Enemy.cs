using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Enemy : MonoBehaviour {

    private BoxCollider2D bc;
    private Rigidbody2D rb;

    // Use this for initialization
    void Start () {
        bc = gameObject.GetComponent<BoxCollider2D>();

        //rb = gameObject.AddComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("COLLISION");
        if(col.gameObject.tag == "Player_proj")
        {
            Debug.Log("AH!");
            col.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
