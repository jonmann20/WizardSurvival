using UnityEngine;
using System.Collections;

public class OneWayWallMessage : MonoBehaviour {

	void OnCollisionEnter(Collision col){
		if(col.gameObject.tag == "Player"){
			HudScript.addNewMessage("Outside of our realm", 40, Color.red);
		}
	}
}
