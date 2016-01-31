using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GoalManager : MonoBehaviour {


    private string foodGoalComplete = "Timmy has eaten!  Agent Wife will be so happy!";
    private string bathGoalComplete = "Timmy had his bath! Agent Wife will be pleased!";
    private string goalHint1 = "Timmy never goes anywhere without his toys";

    public Text goalText;
    public string currentText;
    public static GoalManager instance;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
        goalText.CrossFadeAlpha(0f, 5f, false);
        StartCoroutine(RandomHint());
    }
	
    public void Alert(string s)
    {
        currentText = s;
        StartCoroutine(GoalAchieved());
    }
    public void CompleteFood()
    {
        currentText = foodGoalComplete;
        StartCoroutine(GoalAchieved());
    }

    public void CompleteBath()
    {
        currentText = bathGoalComplete;
        StartCoroutine(GoalAchieved());
    }
    IEnumerator GoalAchieved()
    {
        goalText.text = currentText;
        goalText.CrossFadeAlpha(1f, .5f, false);
        yield return new WaitForSeconds(5f);
        goalText.CrossFadeAlpha(0f, 3f, false);
        yield return new WaitForSeconds(15f);
    }


    IEnumerator RandomHint()
    {
        while (true)
        {
            float r = Random.Range(0, 1f);
            if(r > .15f)
            {
                goalText.text = goalHint1;
                goalText.CrossFadeAlpha(1f, .5f, false);
                yield return new WaitForSeconds(5f);
                goalText.CrossFadeAlpha(0f, 3f, false);
                yield return new WaitForSeconds(15f);
            }
            yield return new WaitForSeconds(10f);
        }
    }
}
