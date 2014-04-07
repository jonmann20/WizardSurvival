using UnityEngine;
using System.Collections;

using InControl;
using System;

public enum BaseAbility {FireballAbility, Forcefield, Levitation, Enchantment};

public class Tome : MonoBehaviour {

	public GameObject dialog;
	public BaseAbility baseAbility;

	bool dialogVisible = false;

	void Update(){
		if(dialogVisible){
			// dialog
			dialog.transform.rotation = Quaternion.LookRotation(GameObject.Find("MainCamera").transform.forward);

			InputDevice device = InputManager.ActiveDevice;
			InputControl ctrl_Action = device.GetControl(InputControlType.Action3);

			if(ctrl_Action.WasPressed){
				if(AbilityManagerScript.currentAbility){
					// alert!! can't pickup more than one base ability
					GameAudio.playInvNoMove();
				}
				else {
					AbilityManagerScript.that.changeAbility(Enum.GetName(typeof(BaseAbility), baseAbility));
					Destroy(gameObject);
				}
			}
		}
	}

	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "Player"){
			dialogVisible = true;
			dialog.SetActive(true);
		}
	}

	void OnTriggerExit(Collider col){
		if(col.gameObject.tag == "Player"){
			dialogVisible = false;
			dialog.SetActive(false);
		}
	}
}
