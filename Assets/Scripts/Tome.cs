﻿using UnityEngine;
using System.Collections;

using InControl;
using System;

public enum BaseAbility {FireballAbility, IceAbility, ForcefieldAbility};

public class Tome : MonoBehaviour {

	public GameObject dialog;
	public BaseAbility baseAbility;
	public Material mat;
	/*GameObject doorLeft = GameObject.Find ("DoorLeftPivot");
	DoorLeft doorLscript = doorLeft.GetComponent(typeof(DoorLeft));
	GameObject doorRight = GameObject.Find ("DoorRightPivot");
	DoorRight doorRscript = doorRight.GetComponent(typeof(DoorRight));*/

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
					/*
					if (doorLscript.doorOpen == false && doorRscript.doorOpen == false) {
						doorLscript.playDoorAnim();
						doorRscript.playDoorAnim();
					}*/
				}
				else{
					GameAudio.playChimes();

					string bName = Enum.GetName(typeof(BaseAbility), baseAbility);

					HudScript.addNewMessage("Base ability chosen.", 150, Color.white);
					HudScript.addNewMessage("Press \"R1\" to Fire!", 180, Color.white);

					// update player color
					Wizard w = GLOBAL.myWizard.GetComponent<Wizard>();
					w.swapMat(mat);

					// update's punch color
					PunchAbility pa = GLOBAL.myWizard.GetComponent<PunchAbility>();
					pa.punchMat = w.parts[0].renderer.materials;

					GLOBAL.myWizard.GetComponent<AbilityManagerScript>().changeAbility(bName);

					GLOBAL.that.SuperDestroy(gameObject);
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
