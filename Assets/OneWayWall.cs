using UnityEngine;
using System.Collections;

public class OneWayWall : MonoBehaviour {

	void OnCollisionEnter (Collision collision) {

	}

	void OnTriggerExit(Collider collider) {
		gameObject.collider.isTrigger = false;
		MeshRenderer m = gameObject.GetComponent<MeshRenderer> ();
		m.enabled = true;
	}

	void OnTriggerEnter(Collider collider) {
		if (AbilityManagerScript.currentAbility == null) {
			HudScript.addNewMessage("Get a spell!", 120, Color.red);
		} 
	}
}
