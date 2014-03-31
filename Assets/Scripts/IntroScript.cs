using UnityEngine;
using System.Collections;
using InControl;

public class IntroScript : MonoBehaviour {

	const float RATE_OF_MARCH = 1;
	public GameObject MarchingObjectPrefab;

	void Start(){
		GameAudio.playIntro();

		for(int i = 0; i < 15; i++)
		{
			for(int j = 0; j < 15; j++)
			{
				Vector3 pos = new Vector3(i * 3 - 80, 1, j * 3 - 130);
				GameObject newMarcher = Instantiate(MarchingObjectPrefab, pos, Quaternion.identity) as GameObject;
			}
		}
	}

	void Update(){
		Vector3 pos = new Vector3(Random.Range(-100.0f, 50.0f), 1, -12 - Random.Range(0.0f, 5.0f));
		//GameObject newMarcher = Instantiate(MarchingObjectPrefab, pos, Quaternion.identity) as GameObject;

		if(transform.position.z > 10)
		{
			float diff = (-10 - transform.position.z) * 0.005f;
			transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + diff);
		}

		InputDevice device = InputManager.ActiveDevice;
		InputControl ctrl_Start = device.GetControl(InputControlType.Start);
		if(ctrl_Start.IsPressed && ctrl_Start.LastValue == 0) {
			Application.LoadLevel("NetworkSample");
		}
		/*
		if(Input.GetKeyDown("space"))
		{
			Application.LoadLevel("NetworkSample");
		}*/
	}
}
