using UnityEngine;
using System.Collections;
using InControl;

public class ScrollingGUIText : MonoBehaviour {

	float speed = 0.15f;
	bool scrolling = false;
	public bool hitSquare = false;

	void Start(){
		GameAudio.playGameOver();

		scrolling = true;
		GUIText credits = (GUIText) this.GetComponent (typeof(GUIText));
		string creds = "Defend Thy Kingdom\n\n\n\n\n\n";
		creds += "Production Team:\n";
		creds += "Jon Wiedmann\n";
		creds += "Austin Yarger\n";
		creds += "Matthew Goldhaber\n";
		creds += "Eugene Goh\n\n";
		creds += "Many thanks to EECS494 for your awesome feedback and playtests\n\n";
		creds += "Many more thanks to our awesome Jeremy Gibson and Isaiah Hines\n\n\n";
		creds += "Press \"Start\" to go back to Title Screen\n";
		credits.text = creds;
	}

	void Update () {
		if (scrolling == true) {
			transform.Translate (Vector3.up * Time.deltaTime * speed);
		}

		if (gameObject.transform.position.y > -0.030f) {
			scrolling = false;
		}

		if(hitSquare == false) {
			InputDevice device = InputManager.ActiveDevice;
			InputControl ctrl_Square = device.GetControl(InputControlType.Start);
			if (ctrl_Square.WasPressed) {
				initTitle();
			}
		}
	}

	void initTitle(){
		hitSquare = true;
		Invoke ("startTitle", 2.1f);
		GameAudio.stopIntro();
		GameAudio.playChimes();
		GameAudio.playInvMove();
	}
	
	void startTitle(){
		Application.LoadLevel("Title");
	}
}