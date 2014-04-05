using UnityEngine;
using System.Collections;
using InControl;
using ExitGames.Client.Photon;

public class IntroScript : MonoBehaviour {

	const float RATE_OF_MARCH = 1;
	public GameObject MarchingObjectPrefab;

	void Start(){
		PhotonNetwork.Disconnect();
		GameAudio.playIntro();

		for(int i = 0; i < 15; i++)
		{
			for(int j = 0; j < 15; j++)
			{
				Vector3 pos = new Vector3(i * 3 - 47, 1, j * 3 - 30);
				GameObject newMarcher = Instantiate(MarchingObjectPrefab, pos, Quaternion.identity) as GameObject;
			}
		}
	}

	void Update(){
		if(transform.position.z > 10)
		{
			float diff = (-10 - transform.position.z) * 0.005f;
			transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + diff);
		}

		//Check for press Start
		InputDevice device = InputManager.ActiveDevice;
		InputControl ctrl_Start = device.GetControl(InputControlType.Start);
		if((ctrl_Start.WasPressed)) {
			Application.LoadLevel("NetworkSample");
		}
	}

//	void OnGUI(){
//		EZGUI.init();
//
//		EZGUI.placeTxt("Press \"Start\"", 51, EZGUI.HALFW, EZGUI.FULLH - 200);
//	}
}
