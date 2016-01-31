using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using System.Collections.Generic;

public class ChooseRandom : Action
{

    public SharedGameObjectList objects;
    public SharedGameObject chosenObject;

    // Use this for initialization
    public override void OnStart()
    {
        List<GameObject> targetList = objects.Value;
        int index = Random.Range(0, targetList.Count);
        chosenObject.Value = targetList[index];
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }
}
