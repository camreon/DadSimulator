using UnityEngine;
using System.Collections;

public class PropBehavior : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("test");
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    void OnCollisionExit(Collision collision)
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }
}
