using UnityEngine;
using System.Collections;

public class FireballAbility : AbilityBase {
	
	string FireballPrefabString = "FireballPrefab";
	const int SPEED = 20;
	const int MAX_LIFE = 90;

    GameObject wizardHolder;

	void Awake(){
        wizardHolder = GameObject.Find("_WizardHolder");
	}

	public override void fire()
	{
		GameAudio.playFlameShoot();

		GameObject projectile = PhotonNetwork.Instantiate(
			FireballPrefabString, 
			Wizard.myWizard.transform.position + (Wizard.myWizard.transform.forward.normalized * 1),  
			gameObject.transform.rotation, 0
		) as GameObject;

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
