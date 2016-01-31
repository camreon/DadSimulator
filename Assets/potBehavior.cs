using UnityEngine;
using System.Collections;

public class potBehavior : MonoBehaviour {

    public GameObject water;

	void OnCollisionEnter(Collision c)
    {
        if(c.gameObject.tag == "Water")
        {
            water.SetActive(true);
        }
    }
}
