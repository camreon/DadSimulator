using UnityEngine;
using System.Collections;

public class ChildBehavior : MonoBehaviour {

    public GameObject animationGO;
    public bool hasEaten = false;
    public bool tookBath = false;

	// Use this for initialization
	void Start () {
        animationGO.GetComponent<Animation>().wrapMode = WrapMode.Loop;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Goal")
        {
            GameObject goalItem = other.gameObject.transform.parent.gameObject;
            
            if(goalItem.tag == "Food")
            {
                FoodBehavior food = goalItem.GetComponent<FoodBehavior>();
                if(food.isCooked && !food.isBurned)
                {
                    hasEaten = true;
                    Debug.Log("Child ate!");
                }
                else
                {
                    Debug.Log("The food isn't cooked or is burned!");
                }
            }else if(goalItem.name == "BathTub Faucet")
            {
                faucetBehavior faucet = goalItem.GetComponent<faucetBehavior>();
                if (faucet.isFull)
                {
                    tookBath = true;
                    Debug.Log("Bath!");
                }
                
            }
        }
    }
        
}
