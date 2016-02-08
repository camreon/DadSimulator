using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;


public class ChildBehavior : Interactable {

    public bool hasEaten = false;
    public bool tookBath = false;
	private Rigidbody heldBody = null;
	private Animation anime = null;
	private BehaviorTree bTree = null;


	public override void OnStart () {
		bTree = gameObject.GetComponent<BehaviorTree> ();
		anime = gameObject.GetComponentInChildren<Animation> ();
		anime.wrapMode = WrapMode.Loop;
	}

	public override void Interact() {
		if (heldBody == null)
			StartCoroutine( Pickup ());
	}

	IEnumerator Pickup()
	{
		heldBody = gameObject.GetComponent<Rigidbody> ();
		bTree.enabled = false;
		PlayAnimation ("flying");
		Debug.Log ("child was picked up");

		while (gameObject.transform.parent != null) {
			yield return new WaitForFixedUpdate ();
		}

		PlayAnimation ("idle");
		bTree.enabled = true;
		heldBody = null;
		Debug.Log ("child was dropped");
	}

	private void PlayAnimation(string name)
	{
		anime.Stop ();
		anime.Play (name);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Goal")
        {
            GameObject goalItem = other.gameObject.transform.parent.gameObject;

            if (goalItem.tag == "Food")
            {
                FoodBehavior food = goalItem.GetComponent<FoodBehavior>();
                if (food.isCooked && !food.isBurned)
                {
                    hasEaten = true;
                    GoalManager.instance.CompleteFood();
                }
                else
                {
                   GoalManager.instance.Alert("The porkchops are either under or over cooked!");
                }
            }
            else if (goalItem.name == "BathTub Faucet")
            {
                faucetBehavior faucet = goalItem.GetComponent<faucetBehavior>();
                if (faucet.isFull)
                {
                    tookBath = true;
                    GoalManager.instance.CompleteBath();
                }

            }
        }
    }
}
