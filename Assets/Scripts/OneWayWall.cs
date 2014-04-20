using UnityEngine;
using System.Collections;

public class OneWayWall : MonoBehaviour {

	public bool closeDoor = false;
	public bool done = false;

	void OnTriggerExit(Collider collider) {
		if (closeDoor == true && done == false) {
			DoorLeft doorleft = (DoorLeft) GameObject.Find("DoorLeftPivot").GetComponent(typeof(DoorLeft));
			DoorRight doorright = (DoorRight) GameObject.Find("DoorRightPivot").GetComponent(typeof(DoorRight));
			doorleft.playDoorAnim ();
			doorright.playDoorAnim ();
			done = true;
		}
	}
}
