using UnityEngine;
using System.Collections;

public class TouchDoor : MonoBehaviour {

	private int door_LastIndex = 0;
	private int count = 30;
	
	public void playDoorAnim () {
		if (door_LastIndex == 0) {
			gameObject.animation.Play ("DoorRightOpen");
			door_LastIndex = 1;
		} else {
			gameObject.animation.Play ("DoorRightClose");
			door_LastIndex = 0;
		}
	}

	void OnCollisionEnter (Collision collision) {
		Debug.Log (collision.gameObject.tag == "player");
		if (collision.gameObject.tag == "Player" && !gameObject.animation.isPlaying) {
			playDoorAnim ();
		}
	}
	
	void Update () {
		//Debug.Log (coll);
		if (!gameObject.animation.isPlaying && door_LastIndex == 1) {
			--count;
		}

		if (count == 0) {
			playDoorAnim ();
			count = 30;
		}
	}
}
