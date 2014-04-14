using UnityEngine;
using System.Collections;

public class DoorRight : MonoBehaviour {
	
	private int door_LastIndex;
	public bool doorOpen = false;
	private int count = 0;
	// Use this for initialization
	
	public void playDoorAnim () {
		animation.Play("DoorRightOpen");
		doorOpen = true;
	}
	
	void Update () {
		if (doorOpen == false) {
			
			/*
			count = 0;
			for( int i = 0; i < PhotonNetwork.playerList.Length; i++ ) {
				if( PhotonNetwork.playerList[i].customProperties.ContainsKey("Ability") ) {
					++count;
				}
			}
			if (count == PhotonNetwork.playerList.Length - 1) {
				doorOpen = true;
				animation.Play("DoorLeftOpen");
				animation.Play("DoorRightOpen");
				count = 0;
			}
			*/
		}
	}
}
