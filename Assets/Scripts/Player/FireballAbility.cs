using UnityEngine;
using System.Collections;

public class FireballAbility : AbilityBase {
	
	string FireballPrefabString = "FireballPrefab";
	const int SPEED = 20;
	const int MAX_LIFE = 90;
	Vector3 pos;

    GameObject wizardHolder;

	int delayTimer = 120;
	int delayTimerInit = 120;
	int cooldownResolution = 60;		// assumping 60fps here (could use Time.fixedTime)

	void Awake(){
        wizardHolder = GameObject.Find("_WizardHolder");
		pos.x = Screen.width / 2;
		pos.y = Screen.height / 2;
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
			EZGUI.drawBox(EZGUI.FULLW - 225, 130, 200 * ((float)(delayTimerInit - delayTimer) / (float)delayTimerInit), 20, new Color(1f, 0f, 0f, 1f));
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
		//Debug (GLOBAL.myWizard.transform.position);
		///Debug (GLOBAL.myWizard.transform.forward.normalized);
		Debug.Log (GLOBAL.myWizard.transform.position + GLOBAL.myWizard.transform.forward.normalized);
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

	private Vector3 projectileAdjustor() {
		return new Vector3 (0, 0, 0);
	}
}
