using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Dad;


public class PickupScript : MonoBehaviour 
{
	public Transform cam;
	private Hand hand = new Hand ();
	public float pickupRange = 2f;


	private void Start() {
		cam = Camera.main.transform;;

		StartCoroutine (CarryPhysics());
		StartCoroutine (ClickAction ());
	}

	IEnumerator CarryPhysics() 
	{
		while (true) {
			if (hand.item != null) {
				hand.Carry ();
			}
			yield return null;
		}
	}

	IEnumerator ClickAction()
	{
		while (true) {
			if (Input.GetMouseButtonDown (0)) {
				if (hand.item != null)
					hand.Drop ();
				else
					FindObj ();
			} 
			yield return null;
		}
	}
		
	private void FindObj()
	{
		RaycastHit[] hits;
        Ray ray = new Ray(cam.position, cam.forward);
        hits = Physics.RaycastAll(ray, pickupRange);
        
		for (int i = 0; i < hits.Length; i++)
        {
			// TODO: get nearest/best hit

			var hitObj = hits [i].collider.gameObject;

			var success = hand.TryToPickup (hitObj);

			if (hitObj.tag == "Interactable")
				hitObj.GetComponent<Interactable> ().Interact ();

			if (success)
				break;
		}
	}
}
