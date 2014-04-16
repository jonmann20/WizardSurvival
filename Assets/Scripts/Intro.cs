using UnityEngine;
using System.Collections;
using InControl;
using ExitGames.Client.Photon;

public class Intro : MonoBehaviour {

	const float RATE_OF_MARCH = 1;
	public GameObject MarchingObjectPrefab;
	public GameObject cta;
	BouncyTitle bouncy;

	bool hitStart = false;

	void Awake(){
		bouncy = cta.GetComponent<BouncyTitle>();
	}

	void Start(){
		PhotonNetwork.Disconnect();
		GameAudio.playIntro();

		for(int i=0; i < 13; ++i){
			for(int j= 0; j < 13; ++j){
				Vector3 pos = new Vector3(i * 4 - 47, 1, j * 4 + 95);
				Instantiate(MarchingObjectPrefab, pos, Quaternion.identity);
			}
		}
	}

	void Update(){
		if(transform.position.z > 10){
			float diff = (-10 - transform.position.z) * 0.045f;
			transform.Translate(0, 0, diff * Time.deltaTime);
		}

		if(!hitStart){
			InputDevice device = InputManager.ActiveDevice;
			InputControl ctrl_Start = device.GetControl(InputControlType.Start);

			if((ctrl_Start.WasPressed)){
				hitStart = true;

				Invoke("startGame", 2.1f);

				GameAudio.stopIntro();
				GameAudio.playChimes();
				GameAudio.playInvMove();
				bouncy.active = true;
			}
		}
	}

	void startGame(){
		Application.LoadLevel("MattScene");
	}
}
