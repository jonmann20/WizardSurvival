using UnityEngine;
using System.Collections;

public class FireballAbility : AbilityBase {
	
	string FireballPrefabString = "FireballPrefab";
	const int SPEED = 20;
	const int MAX_LIFE = 90;

    GameObject wizardHolder;

	void Awake(){
		//print ("NEW FireballAbility CREATED!");
        wizardHolder = GameObject.Find("_WizardHolder");
	}

	public override void fire()
	{
		GameAudio.playFlame();

		GameObject projectile = PhotonNetwork.Instantiate(
			FireballPrefabString, 
			gameObject.transform.position + (new Vector3(0, 0 ,0)) + (gameObject.transform.forward.normalized * 1), 
			gameObject.transform.rotation, 0
		) as GameObject;

		//set initial velocity
		projectile.rigidbody.velocity = GLOBAL.MainCamera.transform.forward * SPEED;

		//set life
		projectile.GetComponent<FireballScript>().life = MAX_LIFE;

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
