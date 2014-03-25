using UnityEngine;
using System.Collections;

public class FireballAbility : AbilityBase {
	
	string FireballPrefabString = "FireballPrefab";
	const int SPEED = 20;
	const int MAX_LIFE = 90;

	public void Awake()
	{
		print ("NEW FireballAbility CREATED!");
	}

	public override void fire()
	{
		print("FireballAbility FIRED");

		GameObject projectile = PhotonNetwork.Instantiate(
			FireballPrefabString, 
			gameObject.transform.position + (new Vector3(0, 0 ,0)) + (gameObject.transform.forward.normalized * 1), 
			gameObject.transform.rotation, 0
			) as GameObject;

		//set initial velocity
		projectile.rigidbody.velocity = GLOBAL.MainCamera.transform.forward * SPEED;

		//set life
		projectile.GetComponent<FireballScript>().life = MAX_LIFE;
	}

	public override string getAbilityName()
	{
		return "Calore";
	}
	public override string getAbilityDescription()
	{
		return "Burn, baby, Burn.";
	}
}
