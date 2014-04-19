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

	Color btnColor, btnColorInit;

	void Awake(){
		GameObject between_scenes_object = GameObject.FindWithTag("Between_Scenes");

		if(between_scenes_object == null){
			Instantiate(Between_Scenes_Prefab, new Vector3(0, 0, 0), Quaternion.identity);
		}
		
		bouncy = cta.GetComponent<BouncyTitle>();

		btnColorInit = btnColor = new Color(11, 1, 1, 0.85f);
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

			if(ctrl_Start.WasPressed) {	// NOTE: doesn't seem to work when active in TextField
				initStartGame();
			}
		}
	}

	

	void OnGUI(){
		EZGUI.init();

		EZOpt e = new EZOpt();
		e.font = GLOBAL.spookyMagic;

		// title
		EZGUI.placeTxt("Defend Thy Kingdom", 90, EZGUI.HALFW, 220, e);

		// cta text
		EZGUI.drawBox(675, 440, 575, 180, new Color(0.3f, 0.3f, 0.3f, 0.6f));
		EZGUI.placeTxt("Enter your name: ", 45, EZGUI.HALFW, EZGUI.HALFH, e);

		// input box
		GUIStyle gs = new GUIStyle(GUI.skin.textField);
		gs.font = e.font;
		gs.fontSize = 27;
		gs.padding = new RectOffset(8, 5, 3, 4);
		BETWEEN_SCENES.player_name = GUI.TextField(new Rect(EZGUI.HALFW - 236, EZGUI.HALFH - 4, 350, 50), BETWEEN_SCENES.player_name, 18, gs);

		// start button
		EZGUI.drawBox(EZGUI.HALFW + 135, EZGUI.HALFH - 4, 100, 50, btnColor);
		e.color = new Color(0.2f, 0.2f, 0.2f);

		if(!hitStart) {
			e.hoverColor = new Color(0.1f, 0.1f, 0.4f);
			e.activeColor = new Color(0f, 0f, 0.5f);
		}

		EZButton b = EZGUI.placeBtn("Start", 29, EZGUI.HALFW + 185, EZGUI.HALFH + 52, e);

		if(!hitStart) {
			if(b.btn) {
				initStartGame();
			}
			else if(b.hover) {
				btnColor = new Color(0.95f, 0.95f, 0.95f, 0.85f);
			}
			else if(b.active) {
				btnColor = new Color(0.93f, 0.93f, 0.93f, 0.85f);
			}
			else {
				btnColor = btnColorInit;
			}
		}
		else {
			btnColor = btnColorInit;
		}

		//GUI.Label(new Rect(Screen.width / 2 - 80, Screen.height / 2, 200, 20), "What do you go by Stranger?");
		//BETWEEN_SCENES.player_name = GUI.TextField(new Rect(Screen.width / 2 - 80, Screen.height / 2 + 20, 200, 20), BETWEEN_SCENES.player_name);
	}

	void initStartGame() {
		hitStart = true;

		Invoke("startGame", 2.1f);

		GameAudio.stopIntro();
		GameAudio.playChimes();
		GameAudio.playInvMove();
		//bouncy.isActive = true;
	}

	void startGame(){
		Application.LoadLevel("MattScene");
	}
}
