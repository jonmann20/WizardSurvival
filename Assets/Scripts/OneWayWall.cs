using UnityEngine;
using System.Collections;

public class OneWayWall : MonoBehaviour {

	void OnTriggerExit(Collider collider) {
		DoorLeft.closeDoor = true;
		DoorRight.closeDoor = true;
	}
}
