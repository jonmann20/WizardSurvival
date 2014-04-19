using UnityEngine;
using System.Collections;
using InControl;
using ExitGames.Client.Photon;

public class Intro : MonoBehaviour {

	const float RATE_OF_MARCH = 1;
	public GameObject MarchingObjectPrefab;
	public GameObject Between_Scenes_Prefab;
	public GameObject cta;
	BouncyTitle bouncy;

	bool hitStart = false;

	void Awake(){

		GameObject between_scenes_object = GameObject.FindWithTag("Between_Scenes");
		if(between_scenes_object == null)
			Instantiate(Between_Scenes_Prefab, new Vector3(0, 0, 0), Quaternion.identity);
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
				bouncy.isActive = true;
			}
		}
	}

	void OnGUI(){
		GUI.Label(new Rect(Screen.width / 2 - 80, Screen.height / 2, 200, 20), "What do you go by Stranger?");
		BETWEEN_SCENES.player_name = GUI.TextField(new Rect(Screen.width / 2 - 80, Screen.height / 2 + 20, 200, 20), BETWEEN_SCENES.player_name);
	}

	void startGame(){
		Application.LoadLevel("MattScene");
	}
}
