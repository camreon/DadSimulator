/* based on http://docs.unity3d.com/ScriptReference/Cursor-lockState.html */

using UnityEngine;
using System.Collections;

public class CursorLockScript : MonoBehaviour {

	void Start ()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update ()
	{
		// Release cursor on escape keypress
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}

		// Confine cursor on tilde/back quote keypress
		if (Input.GetKeyDown (KeyCode.BackQuote)) {
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;

			Debug.Log ("Cursor.lockstate = " + Cursor.lockState);
		}
	}
}