using UnityEngine;
using System.Collections;

public class bgmBehavior : MonoBehaviour {

    public AudioSource start;
    public AudioSource loop;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(!start.isPlaying && !loop.isPlaying)
        {
            loop.Play();
        }
	}
}
