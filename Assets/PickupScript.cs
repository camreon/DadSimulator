using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;


public class PickupScript : MonoBehaviour 
{
	private Transform cam;
	private Rigidbody heldObj = null;
	public float pickupRange = 2f;
	public float held_x = 0.5f;
	public float held_y = 0.5f;
	public float held_z = 1.0f;


	private void Start()
	{
		cam = Camera.main.transform;
	}

	private void FixedUpdate()
	{
		// held object physics
		if (heldObj != null)
			heldObj.MovePosition (Camera.main.ViewportToWorldPoint (new Vector3 (held_x, held_y, held_z)));
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
		RaycastHit[] hits;
        Ray ray = new Ray(cam.position, cam.forward);
        hits = Physics.RaycastAll(ray, pickupRange);
        
		for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            var selectedObj = hit.collider.gameObject.GetComponentInParent<Rigidbody> () ??
							  hit.collider.gameObject.GetComponent<Rigidbody> ();
            if(hit.collider.gameObject.tag == "Interactable")
            {
                hit.collider.gameObject.GetComponent<Interactable>().Interact();
                return null;
            }

			if (selectedObj != null && !selectedObj.isKinematic)
				return selectedObj;
			else
				Debug.Log (hit.collider.gameObject.name + " doesn't have a rigidbody or isKinematic.");
		}

		return null;
	}

	private void Pickup(Rigidbody obj) {
        obj.useGravity = false;
		obj.transform.parent = cam;
		obj.freezeRotation = true;
		heldObj = obj;
	}

	private void Drop() {
        heldObj.useGravity = true;
		heldObj.transform.parent = null;
		heldObj.freezeRotation = false;
		heldObj = null;
	}
}
