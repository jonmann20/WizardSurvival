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

		for(int i=0; i < 13; ++i){
			for(int j= 0; j < 13; ++j){
				Vector3 pos = new Vector3(i * 4 - 47, 1, j * 4 + 95);
				GameObject newMarcher = Instantiate(MarchingObjectPrefab, pos, Quaternion.identity) as GameObject;
			}
		}
	}

	void Update(){
		if(transform.position.z > 10){
			float diff = (-10 - transform.position.z) * 0.045f;//0.005f;
			transform.Translate(0, 0, diff * Time.deltaTime);
			//transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + diff);
		}

		InputDevice device = InputManager.ActiveDevice;
		InputControl ctrl_Start = device.GetControl(InputControlType.Start);
		if((ctrl_Start.WasPressed)) {
			Application.LoadLevel("NetworkSample");
		}
	}
}
