using System;
using System.Collections;
using UnityEngine;


namespace Dad
{
	public class Hand : MonoBehaviour
	{ 
		public Rigidbody item;
		public Vector3 holdPos = new Vector3 (0.5f, 0.5f, 1.0f);


		public Hand() {
			item = null;
		}

		public bool TryToPickup(GameObject obj) {
			var new_item = obj.GetComponent<Rigidbody> ();
			if (new_item == null)
				new_item = obj.GetComponentInParent<Rigidbody> ();

			if (new_item != null && !new_item.isKinematic) {
				Pickup (new_item);
				return true;
			} else {
				Debug.Log ("Can't pick up " + obj.name);
				return false;
			}
		}

		private void Pickup(Rigidbody new_item) {
			item = new_item;
			item.transform.parent = Camera.main.transform;
			item.useGravity = false;
			item.freezeRotation = true;
		}

		public void Drop() {
			item.transform.parent = null;
			item.useGravity = true;
			item.freezeRotation = false;
			item = null;
		}

		public void Carry() {
			item.MovePosition (Camera.main.ViewportToWorldPoint (holdPos));
		}
	}
}

