using UnityEngine;
using System.Collections;

public class DoorLeft : MonoBehaviour {
	
	private int door_LastIndex = 0;
	private int count = 0;
	public static bool closeDoor = false;
	
	void Update () {
		if (door_LastIndex == 0) {
			count = 4;
			for (int i = 0; i < PhotonNetwork.playerList.Length; ++i) {
				string p = (string) PhotonNetwork.playerList[i].customProperties["Ability"];
				if (p != null && p != "none") {
					--count;
				}
			}
			
			if (count == 0) {
				gameObject.animation.Play ("DoorLeftOpen");
				door_LastIndex = 1;
			}
		} else if (door_LastIndex == 1) {
			if (closeDoor == true) {
				gameObject.animation.Play ("DoorLeftClose1");
				door_LastIndex = 2;
			}
		} else if ( door_LastIndex == 2 )
		{
			GameObject mainCam = GameObject.FindWithTag("MainCamera") as GameObject;
			mainCam.GetComponent<GameController>().Begin();
		}
	}
}
