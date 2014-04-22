using UnityEngine;
using System.Collections;

public class ScrollingGUIText : MonoBehaviour {

	float speed = 0.15f;
	bool scrolling = false;

	void Start () {
		GameAudio.playIntro ();
		scrolling = true;
		GUIText credits = (GUIText) this.GetComponent (typeof(GUIText));
		string creds = "Defend Thy Kingdom\n\n\n\n\n";
		creds += "Production Team:\n\n";
		creds += "Jon Wiedmann\n";
		creds += "Austin Yarger\n";
		creds += "Matthew Goldhaber\n";
		creds += "Eugene Goh\n\n\n";
		creds += "Many thanks to EECS494 for your awesome feedback and playtests\n\n";
		creds += "Many more thanks to our awesome Jeremy Gibson and Isaiah Hines\n\n";
		credits.text = creds;
	}

	void Update () {
		if (scrolling == false) {
			return;
		} else {
		transform.Translate (Vector3.up * Time.deltaTime * speed);
		}
		if (gameObject.transform.position.y > -0.030f) {
			scrolling = false;
		}
	}
}