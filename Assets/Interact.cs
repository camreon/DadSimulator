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

    public float pickupRange = 2f;
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

			Debug.Log ("hit.distance = " + hit.distance.ToString ());
		}

		return null;
	}

	private void Pickup(Rigidbody obj) {
		obj.isKinematic = true;
        obj.useGravity = false;
		obj.transform.parent = cam;
		heldObj = obj;
	}

	private void Drop() {
		if (heldObj != null) {
			heldObj.isKinematic = false;
            heldObj.useGravity = true;
			heldObj.transform.parent = null;
			heldObj = null;
		}
	}
}
