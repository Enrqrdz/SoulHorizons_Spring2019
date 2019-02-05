using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class scr_InputManager {

	public static bool cannotInput = false; //set to true to prevent the player from getting input
	public static bool cannotMove = false; //set to true to prevent the player from inputting movement

	/// <summary>
	/// Xbox one - L stick / D-pad
	/// Keyboard - AD
	/// </summary>
	/// <returns>returns -1 for left, 1 for right, 0 for neither, only on the frame the axis is pressed</returns>
	public static int MainHorizontal()
	{
		if(cannotInput || cannotMove)
		{
			return 0;
		}

		float r = 0.0f;
		r += Input.GetAxis("J_MainHorizontal");
		r += Input.GetAxis("J_DHorizontal");
		r += Input.GetAxis("K_MainHorizontal");

		if (r < 0f)
		{
			return -1;
		}
		else if (r > 0f)
		{
			return 1;
		}
		return 0;
	}

	public static int MenuHorizontal()
	{

		float r = 0.0f;
		r += Input.GetAxis("J_DHorizontal");
		r += Input.GetAxis("K_MainHorizontal");

		if (r < 0f)
		{
			return -1;
		}
		else if (r > 0f)
		{
			return 1;
		}
		return 0;
	}

	/// <summary>
	/// Xbox one - L stick / D-pad
	/// Keyboard - WS
	/// </summary>
	/// <returns>returns -1 for down, 1 for up, 0 for neither</returns>
	public static int MainVertical()
	{
		if(cannotInput || cannotMove)
		{
			return 0;
		}

		float r = 0.0f;
		r += Input.GetAxis("J_MainVertical");
		r += Input.GetAxis("J_DVertical");
		r += Input.GetAxis("K_MainVertical");
		if (r < 0f)
		{
			return 1;
		}
		else if (r > 0f)
		{
			return -1;
		}
		return 0;
	}

	public static int MenuVertical()
	{

		float r = 0.0f;
		r += Input.GetAxis("J_DVertical");
		r += Input.GetAxis("K_MainVertical");
		if (r < 0f)
		{
			return 1;
		}
		else if (r > 0f)
		{
			return -1;
		}
		return 0;
	}

	/// <summary>
	/// Xbox one - Right Analog
	/// Keyboard - 1/2/3
	/// </summary>
	/// <returns>returns true if any of these buttons were pressed this frame</returns>
	public static bool SoulFusion()
	{
		if(cannotInput)
		{
			return false;
		}

		return Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3);
	}

	/// <summary>
	/// Used with keyboard controls.
	/// </summary>
	/// <returns>returns either 1,2, or 3 to indicate a selection, or 0 if none was selected</returns>
	public static int K_SoulFusion()
	{
		if(cannotInput)
		{
			return 0;
		}

		if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
		{
			return 1;
		}

		if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
		{
			return 2;
		}

		if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
		{
			return 3;
		}

		return 0;
	}

	static float falseRadius = 0.2f; //values within falseRadius of 0 will give a false reading for the button being pressed. Used with the axis to account for uncertainty with float equality

	/// <summary>
	/// XBox one - B button
	/// Keyboard - LMB
	/// </summary>
	/// <returns>returns true if the blast button is down.</returns>
	public static bool Blast()
	{
		if(cannotInput)
		{
			return false;
		}

		return Input.GetButton("Blast_Button") || (Input.GetAxis("Blast_Axis") > falseRadius) || (Input.GetAxis("Blast_Axis") < -falseRadius);
	}

	/// <summary>
	/// Xbox one - X, Y, A, B buttons
	/// Keyboard - q, w, e, r buttons
	/// </summary>
	/// <returns>returns 0-3 the frame a play card button is pressed or -1 if none is pressed</returns>
	public static int PlayCard()
	{
		if(cannotInput)
		{
			return -1;
		}
		else if (Input.GetButtonDown("PlayCard1_Button"))
		{
			return 0;
		}
		else if (Input.GetButtonDown("PlayCard2_Button"))
		{
			return 1;
		}
		else if (Input.GetButtonDown("PlayCard3_Button"))
		{
			return 2;
		}
		else if (Input.GetButtonDown("PlayCard4_Button"))
		{
			return 3;
		}

		return -1;

	}

	/// <summary>
	/// Xbox one - Right trigger
	/// Keyboard - Tab button
	/// </summary>
	/// <returns>returns true if currently pressed</returns>
	public static bool IsDashPressed()
	{
		return (Input.GetAxis("Dash_Axis") > 0.02f) || Input.GetButton("Dash_Button");
	}

	/// <summary>
	/// Xbox one - Left trigger
	/// Keyboard - Space button
	/// </summary>
	/// <returns>returns true if currently pressed</returns>
	public static bool IsCardSwapPressed()
	{
		return (Input.GetAxis("CardSwap_Axis") > 0.02f) || Input.GetButton("CardSwap_Button");
	}

}
