using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class DamageNumbers : MonoBehaviour
{
	public float riseSpeed = 0.03f;
	public float sideDist = 0.03f;

	public bool isActive = false;

	private TextMeshPro myTextObj;
	private Transform camToLook;
	private float moveX;
	private float moveZ;
	private float alpha = 0f;
	private bool fade = false;
	private Vector3 startScale;
	// Use this for initialization

	void Start ()
    {
		myTextObj = gameObject.GetComponent<TextMeshPro>();
		camToLook = Camera.main.transform;
		myTextObj.color = new Color(0f,0f,0f,0f);
		startScale = transform.localScale;
	}

	/// <summary>
	/// Sets the number details.
	/// </summary>
	/// <param name="text">Text.</param>
	/// <param name="textColor">Text color.</param>
	/// <param name="scaleMulti">Scale Multi, 1.0f is native size.</param>
	public void SetNumberDetails(string text, Color textColor, float scaleMulti)
	{
		isActive = true;
		moveX = Random.Range (-sideDist, sideDist);
		moveZ = Random.Range (-sideDist, sideDist);
		myTextObj.text = text;
		myTextObj.color = textColor;
		transform.localScale = startScale * scaleMulti;
		alpha = 1f;
		fade = false;
		StartCoroutine(FadeNumbers());
	}

	// Update is called once per frame
	void Update () 
	{
		if(isActive == false)
			return;

		Vector3 newLocation = transform.position;
		newLocation.y += riseSpeed * Time.deltaTime;
		newLocation.x += moveX * Time.deltaTime;
		newLocation.z += moveZ * Time.deltaTime;
		transform.position = newLocation;
		transform.forward = camToLook.forward;

		if(fade)
		{
			alpha -= 1f * Time.deltaTime;
			myTextObj.color =  new Color(myTextObj.color.r, myTextObj.color.g, myTextObj.color.b, alpha);
			if(alpha <= 0f)
				isActive = false;
		}
	}

	IEnumerator FadeNumbers()
	{
		yield return new WaitForSeconds(0.25f);
		fade = true;
	}
}
