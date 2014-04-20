using UnityEngine;
using System.Collections;

public class NDTrigger1 : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter (Collider collider) {
		if (collider.gameObject.tag == "Player" && TouchDoor.anim1 == false) {
			TouchDoor.anim1 = true;
		}
	}
}
