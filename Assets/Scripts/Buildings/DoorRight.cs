using UnityEngine;
using System.Collections;

public class DoorRight : MonoBehaviour {
	
	private int door_LastIndex;
	private int count;
	public static bool closeDoor;

	void Start () {
		door_LastIndex = 0;
		count = PhotonNetwork.playerList.Length;
		closeDoor = false;
	}

	void Update () {
		if (door_LastIndex == 0) {
				/*count = PhotonNetwork.playerList.Length;
				for (int i = 0; i < PhotonNetwork.playerList.Length; ++i) {
						string p = (string)PhotonNetwork.playerList [i].customProperties ["Ability"];
						if (p != null && p != "none") {
							--count;
						}
				}

				if (count == 0) {*/
						gameObject.animation.Play ("DoorRightOpen");
						door_LastIndex = 2;
				//}
		} else if (door_LastIndex == 1) {
				if (closeDoor == true) {
						gameObject.animation.Play ("DoorRightClose");
						door_LastIndex = 2;
				}
		} else if (door_LastIndex == 2) {
				GameObject mainCam = GameObject.FindWithTag ("MainCamera") as GameObject;
				mainCam.GetComponent<GameController> ().Begin ();
		}
	}
}
