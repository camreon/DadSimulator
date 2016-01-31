using UnityEngine;
using System.Collections;

public abstract class Interactable : MonoBehaviour {


    public void Start()
    {
        gameObject.tag = "Interactable";
        OnStart();
    }
    public abstract void Interact();
    public abstract void OnStart();
}
