using UnityEngine;
using System.Collections;

public class TalkingText : MonoBehaviour {
	
	public float MAX_DISTANCE = 21;
	
	const int CHARACTER_RATE = 2;
	int character_timer = CHARACTER_RATE;
	
	string desiredText = "";
	string desiredTextCopy = "";
	string currentText = "";
	string[] manyStrings;
	float temp = 0;
	int count;
	
	TextMesh tm;
	
	GameObject MainCamera;
	
	void Start () {
		MainCamera = GameObject.Find("MainCamera");
		tm = GetComponent<TextMesh>();
		//Debug.Log (this.tag);
		if (this.tag == "WizardKing") {
			desiredTextCopy = desiredText = tm.text;
			MAX_DISTANCE = 34;
		} else if(this.tag == "WizardJumpy") {
			MAX_DISTANCE = 21;
			manyStrings = new string[4];
			manyStrings [0] = "No jumping in the pool!";
			manyStrings [1] = "Press F1 to cast your spells!";
			manyStrings [2] = "L2 and F2 switches between inventories.";
			manyStrings [3] = "Stay Alive!";
		} else if (this.tag == "CreepyKing") {
			MAX_DISTANCE = 34;
			desiredTextCopy = desiredText = "Seriously...";
		} else if (this.tag == "CreepyJumpy") {
			MAX_DISTANCE = 21;
			manyStrings = new string[3];
			manyStrings[0] = "You can't share your late days";
			manyStrings[1] = "There's no one during office hours";
			manyStrings[2] = "I think this game is awesome";
		}
		tm.text = "";
		currentText = "";
		count = 200;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Vector3.Distance(MainCamera.transform.position, transform.position) > MAX_DISTANCE)
		{
			renderer.enabled = false;
			count = 200;
		}
		else
		{
			if (this.tag == "WizardJumpy" || this.tag == "CreepyJumpy") {
				if (renderer.enabled == false || count == 0) {
					count = 200;
					reset ();
				}

				renderer.enabled = true;
				--count;
			} else if (this.tag == "WizardKing" || this.tag == "CreepyKing") {
				if (renderer.enabled == false) {
					reset ();
				}
				renderer.enabled = true;
			}
		}
		
		transform.rotation = Quaternion.LookRotation(MainCamera.transform.forward);
		
		if(desiredTextCopy.Length > 0)
		{
			character_timer --;
			if(character_timer <= 0)
			{
				character_timer = CHARACTER_RATE;
				currentText += desiredTextCopy[0];
				desiredTextCopy = desiredTextCopy.Remove(0, 1);
				GetComponent<TextMesh>().text = "" + currentText;
			}
			character_timer --;
			
		}
	}
	
	public void reset()
	{
		currentText = "";
		if (this.tag == "WizardJumpy" || this.tag == "CreepyJumpy") {
			temp = Random.Range (0.0f, manyStrings.Length - 1);
			desiredTextCopy = manyStrings[(int) Mathf.Round(temp)];
		} else if (this.tag == "WizardKing" || this.tag == "CreepyKing") {
			desiredTextCopy = desiredText;
		}
		tm.text = "";
		character_timer = CHARACTER_RATE;
	}
}
