using UnityEngine;
using System.Collections;
using System;

public class ovenBehavior : Interactable
{

    public GameObject hinge;
    public float rotateTime;
    private bool isOpen = false;
    public override void Interact()
    {
        if (!isOpen)
        {
            isOpen = true;
            StartCoroutine(Open());
        }
        else
        {
            isOpen = false;
            StartCoroutine(Close());
        }
    }

    public override void OnStart()
    {

    }

    IEnumerator Open()
    {
        float startTime = Time.time;
        while (Time.time < startTime + rotateTime)
        {
            transform.RotateAround(hinge.transform.position, Vector3.right, -1f);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator Close()
    {
        float startTime = Time.time;
        while (Time.time < startTime + rotateTime)
        {
            transform.RotateAround(hinge.transform.position, Vector3.right, 1f);
            yield return new WaitForFixedUpdate();
        }
    }

}
