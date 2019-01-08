using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Use this in menus to move through menus using the controller or keyboard.
/// </summary>
public class scr_ButtonManager : MonoBehaviour {

	public Button[] buttons;
	int currentButton = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		UserInput();
	}

	bool axisPressed = false;

	private void UserInput()
	{
		//vertical movement only right now
		int axis = 0;
        if (!axisPressed)
        {
            //just pressed the joystick
            axis = scr_InputManager.MenuVertical() * -1;
            axisPressed = true;
        }

        if(scr_InputManager.MenuVertical() == 0)
        {
            //joystick is not pressed
            axisPressed = false;
        }

		if (axis < 0)
        {
            currentButton--;
            if(currentButton < 0)
            {
                currentButton = buttons.Length - 1;
            }

			buttons[currentButton].Select();
			axisPressed = true;
        }
        else if (axis > 0)
        {
            currentButton = (currentButton + 1) % buttons.Length;
			buttons[currentButton].Select();
			axisPressed = true;
        }

		//check for press
		if (Input.GetButtonDown("Menu_Select"))
		{
			Debug.Log("Pressing button");
			buttons[currentButton].onClick.Invoke();
		}
	}

	/// <summary>
	/// Called on event trigger by the buttons. Makes sure that other buttons are not selected
	/// </summary>
	public void DeselectOnPointerEnter()
	{
		EventSystem.current.SetSelectedGameObject(null);
	}
}
