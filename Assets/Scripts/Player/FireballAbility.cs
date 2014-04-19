﻿using UnityEngine;
using System.Collections;

public class FireballAbility : AbilityBase {
	
	string FireballPrefabString = "FireballPrefab";
	const int SPEED = 20;
	const int MAX_LIFE = 90;

    GameObject wizardHolder;

	int delayTimer = 180;
	int delayTimerInit = 180;
	int cooldownResolution = 60;		// assumping 60fps here (could use Time.fixedTime)

	void Awake(){
        wizardHolder = GameObject.Find("_WizardHolder");
	}

	void FixedUpdate(){
		++delayTimer;
	}

	void OnGUI() {
		if(delayTimer < delayTimerInit){
			EZGUI.init();

			EZOpt e = new EZOpt(Color.red, new Color(0.1f, 0.1f, 0.1f));
			e.dropShadowX = e.dropShadowY = 1;
			e.font = GLOBAL.spookyMagic;
		
			EZGUI.placeTxt("cooldown " + (delayTimerInit/cooldownResolution - (delayTimer / cooldownResolution)) + "s", 25, EZGUI.FULLW - 170, 130, e);
		}
	}

	public override void fire(){
		print(delayTimer);
		if(delayTimer < delayTimerInit) {
			GameAudio.playMagicFail();
			return;
		}

		delayTimer = 0;
		GameAudio.playFlameShoot();

		GameObject projectile = PhotonNetwork.Instantiate(
			FireballPrefabString, 
			GLOBAL.myWizard.transform.position + GLOBAL.myWizard.transform.forward.normalized,  
			gameObject.transform.rotation, 0
		) as GameObject;

		projectile.GetComponent<ProjectileBase>().wizard = gameObject;
		//set initial velocity
		projectile.rigidbody.velocity = GLOBAL.MainCamera.transform.forward * SPEED;

        // keep Hierarchy clean
		projectile.transform.parent = wizardHolder.transform;
	}

	public override string getAbilityName(){
		return "Fireball";
	}

	public override string getAbilityDescription(){
		return "Burn, baby, Burn.";
	}
}
