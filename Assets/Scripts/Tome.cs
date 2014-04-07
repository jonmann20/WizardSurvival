using UnityEngine;
using System.Collections;

using InControl;
using System;

public enum BaseAbility {FireballAbility, Forcefield, Levitation, Enchantment};

public class Tome : MonoBehaviour {

	public GameObject dialog;
	public BaseAbility baseAbility;

	void Update(){
		InputDevice device = InputManager.ActiveDevice;
		InputControl ctrl_Action = device.GetControl(InputControlType.Action3);

		if(ctrl_Action.WasPressed){
			if(AbilityManagerScript.currentAbility){
				// alert!! can't pickup more than one base ability
			}
			else {
				AbilityManagerScript.that.changeAbility(Enum.GetName(typeof(BaseAbility), baseAbility));
				Destroy(gameObject);
			}
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
