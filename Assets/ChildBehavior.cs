using UnityEngine;
using System.Collections;

public class ChildBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Animation>().wrapMode = WrapMode.Loop;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
