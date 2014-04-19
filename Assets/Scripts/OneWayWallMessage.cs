using UnityEngine;
using System.Collections;

public class OneWayWallMessage : MonoBehaviour {

	void OnCollisionEnter(Collision col){
		if(col.gameObject.tag == "Player"){
			HudScript.addNewMessage("Outside of our realm", 70, Color.red);
		}
	}
}
