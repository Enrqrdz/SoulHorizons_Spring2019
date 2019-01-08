using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class scr_RadialSelect : MonoBehaviour {

	//This system works off of when the buttons are selected, so the radial menu lazy selection needs to be changed to not select anything when the 
	public float timeToSelect = 1f;
	private float buttonTimer;
	private bool isButtonSelected;
	private Button currentButton;

	void Start () {
		isButtonSelected = false;
	}
	
	void Update () {
		//keep counting up on the timer as long as a button is selected
		if (isButtonSelected)
		{
			buttonTimer += Time.deltaTime;

			if (buttonTimer >= timeToSelect)
			{
				ActivateSelection();
			}
		}
	}

	/// <summary>
	/// Call this when the timer reaches the desired time
	/// </summary>
	private void ActivateSelection()
	{
		Debug.Log("Activating " + currentButton.name);
		//click the selected button
		currentButton.onClick.Invoke();
		buttonTimer = 0f;

	}

	public void OnButtonSelect(Button b)
	{
		Debug.Log("Button " + b.name + " selected");
		currentButton = b;
		isButtonSelected = true;
	}

	public void OnButtonDeselect()
	{
		buttonTimer = 0f;
	}

	public void OnCancelSelect()
	{
		currentButton = null;
		isButtonSelected = false;
		EventSystem.current.SetSelectedGameObject(null);

	}

	/// <summary>
	/// Called on event trigger by the buttons. Makes sure that other buttons are not selected. Note: do we need to worry about this with the new keyboard controls?
	/// </summary>
	public void DeselectOnPointerEnter()
	{
		EventSystem.current.SetSelectedGameObject(null);
	}
}
