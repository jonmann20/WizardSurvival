using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

	public Texture2D crosshairTexture;
	public Rect position;
	static bool crosshairOn = true;

	// Update is called once per frame
	void Update () {
		position = new Rect((Screen.width - crosshairTexture.width) / 2f - 25f, 
		                    (Screen.height - crosshairTexture.height) / 2f + 30f, 
		                    crosshairTexture.width, 
		                    crosshairTexture.height);
	}

	void OnGUI () {
		if (crosshairOn == true) {
			GUI.DrawTexture(position, crosshairTexture);
		}
	}
}
