using UnityEngine;
using System.Collections;

public class DoorLeft : MonoBehaviour {

	private int door_LastIndex = 0;
	private int count = 0;

	public void playDoorAnim () {
		if (door_LastIndex == 0) {
			gameObject.animation.Play ("DoorLeftOpen");
			door_LastIndex = 1;
		} else {
			//gameObject.animation.Play ("DoorLeftClose");
			//door_LastIndex = 0;
		}
	}

	void Update () {
		if (door_LastIndex == 0) {
			count = PhotonNetwork.playerList.Length;
			for (int i = 0; i < PhotonNetwork.playerList.Length; ++i) {
				string p = (string) PhotonNetwork.playerList[i].customProperties["Ability"];
				if (p != null && p != "none") {
					--count;
				}
			}
			
			if (count == 0) {
				playDoorAnim ();
			}
		}
	}
}
