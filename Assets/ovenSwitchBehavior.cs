﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ovenSwitchBehavior : Interactable
{
    public bool isOvenOn = false;
    public float cookTime;
    public float burnTime;
    private float startTime;
    public GameObject ovenLight;
    public GameObject ovenBody;
    public Puffy_Emitter emitter;

    public override void Interact()
    {
        isOvenOn = !isOvenOn;

        if (isOvenOn)
        {
            startTime = Time.time;
            ovenLight.SetActive(true);
            ovenBodyBehavior ovenBodyC = ovenBody.GetComponent<ovenBodyBehavior>();
            if(ovenBodyC.ovenContents != null)
            {
                StartCoroutine("Cook");
            }
        }
        else
        {
            emitter.autoEmit = false;
            ovenLight.SetActive(false);
            StopCoroutine("Cook");
        }
    }

    IEnumerator Cook()
    {
        float startTime = Time.time;
        while(Time.time < startTime + cookTime)
        {
            yield return new WaitForFixedUpdate();
        }
        ovenBody.GetComponent<ovenBodyBehavior>().ovenContents.GetComponent<FoodBehavior>().isCooked = true;
        Debug.Log("Finished Cooking!");
        while(Time.time < startTime + cookTime + burnTime)
        {
            yield return new WaitForFixedUpdate();
        }
        ovenBody.GetComponent<ovenBodyBehavior>().ovenContents.GetComponent<FoodBehavior>().isBurned = true;
        emitter.autoEmit = true;

        Debug.Log("Food burned!");
    }

    public override void OnStart()
    {
        
    }

}
