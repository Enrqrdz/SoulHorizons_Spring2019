using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Put this on an object if you want it to be destroyed after a certain amount of time
/// </summary>
public class scr_DestroyAfterSeconds : MonoBehaviour {

	public float seconds = 1f;
	
	void Start () {
		Invoke("Destroy", seconds);
	}
	
	// Destroy this object
	void Destroy () {
		GameObject.Destroy(gameObject);
	}
}
