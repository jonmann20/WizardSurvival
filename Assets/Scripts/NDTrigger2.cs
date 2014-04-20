using UnityEngine;
using System.Collections;

public class NDTrigger2 : MonoBehaviour {

	void OnTriggerEnter (Collider collider) {
		if (collider.gameObject.tag == "Player" && TouchDoor.anim2 == false) {
			TouchDoor.anim2 = true;
		}
	}
}
