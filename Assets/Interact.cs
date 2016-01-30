using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;


public class Interact : MonoBehaviour 
{
	private Transform cam;
	private Rigidbody heldObj = null;

	private void Start()
	{
		cam = Camera.main.transform;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown (0)) {
			if (heldObj != null) {
				Drop ();
			} else {
				var foundObj = FindObj ();
				if (foundObj)
					Pickup (foundObj);
			}
		}
	}


	private Rigidbody FindObj()
	{
		var pickupRange = 2;
		RaycastHit hit;

		if (Physics.Raycast (cam.position, cam.forward, out hit, pickupRange)) {
			var selectedObj = hit.collider.gameObject.GetComponentInParent<Rigidbody> () ??
							  hit.collider.gameObject.GetComponent<Rigidbody> ();

			if (selectedObj != null && !selectedObj.isKinematic)
				return selectedObj;
			else
				Debug.Log (hit.collider.gameObject.name + " doesn't have a rigidbody or isKinematic.");

			Debug.Log ("hit.distance = " + hit.distance.ToString ());
		}

		return null;
	}

	private void Pickup(Rigidbody obj) {
		obj.isKinematic = true;
		obj.transform.parent = cam;
		heldObj = obj;
	}

	private void Drop() {
		if (heldObj != null) {
			heldObj.isKinematic = false;
			heldObj.transform.parent = null;
			heldObj = null;
		}
	}
}
