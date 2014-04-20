using UnityEngine;
using System.Collections;

public class TouchDoor : MonoBehaviour {

	private int door_LastIndex = 0;
	private int count = 40;
	public static bool anim1 = false;
	public static bool anim2 = false;
	
	public void playDoorAnim1 () {
		if (door_LastIndex == 0) {
			gameObject.animation.Play ("HNDoorOpen");
			door_LastIndex = 1;
		} else if (door_LastIndex == 1) {
			gameObject.animation.Play ("HNDoorClose");
			door_LastIndex = 0;
		}
	}

	public void playDoorAnim2 () {
		if (door_LastIndex == 0) {
			gameObject.animation.Play ("HNDoorOpen2");
			door_LastIndex = 1;
		} else if (door_LastIndex == 1) {
			gameObject.animation.Play ("HNDoorClose2");
			door_LastIndex = 0;
		}
	}

	/*void OnCollisionEnter (Collision collision) {
		Debug.Log (collision.gameObject.tag == "player");
		if (collision.gameObject.tag == "Player" && !gameObject.animation.isPlaying) {
			playDoorAnim2 ();
		}
	}*/
	
	void Update () {
		if (!gameObject.animation.isPlaying && door_LastIndex == 0) {
			if (anim1 == true) {
				playDoorAnim1 ();
			} else if (anim2 == true) {
				playDoorAnim2 ();
			}
		} else if (!gameObject.animation.isPlaying && door_LastIndex != 0) {
			--count;
		}

		if (count == 0) {
			if (anim1 == true) {
				playDoorAnim1 ();
				anim1 = false;
			} else if (anim2 == true) {
				playDoorAnim2 ();
				anim2 = false;
			}
			count = 40;
		}
	}
}
