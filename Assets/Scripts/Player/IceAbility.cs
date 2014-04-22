using UnityEngine;
using System.Collections;

public class IceAbility : AbilityBase {

	const int SPEED = 20;
	const int MAX_LIFE = 90;

	int delayTimer = 20;
	int delayTimerInit = 20;
	//int cooldownResolution = 60;		// assumping 60fps here (could use Time.fixedTime)

	void FixedUpdate(){
		++delayTimer;
	}

//	void OnGUI() {
//		if(delayTimer < delayTimerInit){
//			EZGUI.init();
//			
//			EZOpt e = new EZOpt(Color.red, new Color(0.1f, 0.1f, 0.1f));
//			e.dropShadowX = e.dropShadowY = 1;
//			e.font = GLOBAL.spookyMagic;
//
//			// since cooldown will be so short, don't make it visible to the player
//			//EZGUI.placeTxt("cooldown " + (delayTimerInit/cooldownResolution - (delayTimer / cooldownResolution)) + "s", 25, EZGUI.FULLW - 170, 130, e);
//			//EZGUI.drawBox(EZGUI.FULLW - 225, 130, 200 * ((float)(delayTimerInit - delayTimer) / (float)delayTimerInit), 20, new Color(1f, 0f, 0f, 1f));
//		}
//	}

	public override void fire(){
		if(delayTimer < delayTimerInit){
			//GameAudio.playMagicFail();
			return;
		}

		GameAudio.playFreezeSpell();
		delayTimer = 0;

		Transform w = GLOBAL.myWizard.transform;

		GameObject ice = PhotonNetwork.Instantiate(
			"IcePrefab",
			w.position + GLOBAL.MainCamera.transform.forward.normalized * 1.1f,// + new Vector3(0, 0, 0.3f),
			gameObject.transform.rotation,
			0
		) as GameObject;

		ice.GetComponent<ProjectileBase>().wizard = GLOBAL.myWizard;
		ice.rigidbody.velocity = GLOBAL.MainCamera.transform.forward * SPEED;
		ice.transform.parent = w.parent;
	}

	public override string getAbilityName() {
		return "Ice Blast";
	}

	public override string getAbilityDescription() {
		return "Burrrrr...";
	}
}
