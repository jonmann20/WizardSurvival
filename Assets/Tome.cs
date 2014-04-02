using UnityEngine;
using System.Collections;

using InControl;

public class Tome : MonoBehaviour {

	public GameObject dialog;

	void Update(){
		InputDevice device = InputManager.ActiveDevice;
		InputControl ctrl_Action = device.GetControl(InputControlType.Action3);

		if(ctrl_Action.WasPressed){
			// TODO: add base ability spell to player

			Destroy(gameObject);
		}
	}

	void OnTriggerEnter(Collider col){
		showDialog(col);
	}

	void OnTriggerStay(Collider col){
		showDialog(col);
	}

	void OnTriggerExit(){
		dialog.SetActive(false);
	}

	void showDialog(Collider col){
		if(col.gameObject.tag == "Player"){
			dialog.SetActive(true);
			dialog.transform.rotation = Quaternion.LookRotation(GameObject.Find("MainCamera").transform.forward);
		}
	}
}
