using UnityEngine;
using System.Collections;

public class ovenBodyBehavior : MonoBehaviour
{

    public GameObject ovenContents = null;
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Food")
        {
            Debug.Log("Food is in the oven!");
            ovenContents = collider.gameObject;
            //ovenContents.transform.SetParent(transform, true);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject == ovenContents)
        {
            Debug.Log("Food left the oven!");
            ovenContents = null;
        }
    }
}
