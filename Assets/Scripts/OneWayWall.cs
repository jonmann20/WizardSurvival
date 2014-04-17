using UnityEngine;
using System.Collections;

public class OneWayWall : MonoBehaviour {

	public bool closeDoor = false;

	void OnTriggerExit(Collider collider) {
		gameObject.collider.isTrigger = false;
		MeshRenderer m = gameObject.GetComponent<MeshRenderer> ();
		m.enabled = true;
		if (closeDoor == true) {
			DoorLeft doorleft = (DoorLeft) GameObject.Find("DoorLeftPivot").GetComponent(typeof(DoorLeft));
			DoorRight doorright = (DoorRight) GameObject.Find("DoorRightPivot").GetComponent(typeof(DoorRight));
			doorleft.playDoorAnim ();
			doorright.playDoorAnim ();
		}
	}

	void OnTriggerEnter(Collider collider) {
		if (AbilityManagerScript.currentAbility == null) {
			HudScript.addNewMessage("Get a spell!", 120, Color.red);
		} 
	}
}
