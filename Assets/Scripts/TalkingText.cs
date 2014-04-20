using UnityEngine;
using System.Collections;

public class TalkingText : MonoBehaviour {

	public float MAX_DISTANCE = 18;

	const int CHARACTER_RATE = 3;
	int character_timer = CHARACTER_RATE;

	string desiredText = "";
	string desiredTextCopy = "";
	string currentText = "";
	public string[] manyStrings;
	float temp;

	TextMesh tm;

	GameObject MainCamera;

	void Start () {
		MainCamera = GameObject.Find("MainCamera");
		tm = GetComponent<TextMesh>();
		desiredText = manyStrings[0];
		desiredTextCopy = desiredText;
		tm.text = "";
		currentText = "";

		//manyStrings [1] = "";
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if(Vector3.Distance(MainCamera.transform.position, transform.position) > MAX_DISTANCE)
		{
			renderer.enabled = false;
		}
		else
		{
			if(renderer.enabled == false)
				reset();
			renderer.enabled = true;
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

		}*/
	}

	public void reset()
	{
		currentText = "";
		temp = Random.Range (0.0f, manyStrings.Length - 1);
		desiredTextCopy = desiredText;
		tm.text = "";
		character_timer = CHARACTER_RATE;
	}
}
