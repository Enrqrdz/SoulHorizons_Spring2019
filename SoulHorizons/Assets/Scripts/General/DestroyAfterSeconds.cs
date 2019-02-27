using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour {

	public float secondsTilDestroyed = 1f;
	
	void Start ()
    {
		Invoke("Destroy", secondsTilDestroyed);
	}
	
	public void Destroy()
    {
		GameObject.Destroy(gameObject);
	}
}
