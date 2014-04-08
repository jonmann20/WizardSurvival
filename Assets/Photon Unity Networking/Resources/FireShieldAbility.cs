using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireShieldAbility : AbilityBase {
	
	string FireballPrefabString = "FireballPrefab";
	const int MAX_LIFE = 90;
	float sinCounter = 0.0f;
	const float CIRCLE_RATE = 0.2f;
	const float CIRCLE_RADIUS = 3.0f;
	const int NUMBER_OF_FIREBALLS = 5;
	List<GameObject> shieldFlames = new List<GameObject>();
	
	public override void fire()
	{
		GameAudio.playWind();

		for(int i = 0; i < NUMBER_OF_FIREBALLS; i++)
		{
			Vector3 pos = new Vector3(0, 0, 0);
			GameObject projectile = PhotonNetwork.Instantiate(
				FireballPrefabString, 
				pos, 
				gameObject.transform.rotation, 0
			) as GameObject;

			//set life.
			projectile.GetComponent<FireballScript>().life = MAX_LIFE;
			//Add to projectile list for movement.
			shieldFlames.Add(projectile);
		}
	}

	void Update()
	{

		sinCounter += CIRCLE_RATE;
		for(int i = 0; i < shieldFlames.Count; i++)
		{
			if(shieldFlames[i] == null)
			{
				shieldFlames.RemoveAt(i);
				continue;
			}
			float diff = (2.0f * Mathf.PI) / ((float)NUMBER_OF_FIREBALLS);
			float sinValue = (diff * i) + sinCounter;
			Vector3 pos = new Vector3(Mathf.Sin(sinValue), 0, Mathf.Cos(sinValue));
			pos.Normalize();
			pos *= CIRCLE_RADIUS;

			shieldFlames[i].transform.position = gameObject.transform.position + pos;
		}
	}

	public override string getAbilityName()
	{
		return "Wind";
	}
	public override string getAbilityDescription()
	{
		return "On nights like tonight, legend has it the air carries a vaunted flame to aid the innocent.";
	}
}
