using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireArcAbility : AbilityBase {
	
	const int SPEED = 20;
	const int MAX_LIFE = 120;
	const int numberOfFireballs = 20;
	
	GameObject wizardHolder;

	GameObject otherWizard;
	List<GameObject> projectiles = new List<GameObject>();

	void Awake(){
		wizardHolder = GameObject.Find("_WizardHolder");
	}
	
	public override void fire()
	{
		print("FIRE ARC");
		GameAudio.playChain();

		if(PhotonNetwork.playerList.Length > 1)
		{
			otherWizard = getClosestWizard(new List<GameObject>(GameObject.FindGameObjectsWithTag("Player")));

			Vector3 first = transform.position;
			Vector3 last = otherWizard.transform.position;

			float fraction = 1.0f / (float)numberOfFireballs;
			float sum = 0;
			for(int i = 0; i < numberOfFireballs; i++)
			{
				sum += fraction;
			    Vector3 p = Vector3.Lerp(first, last, sum);
				print("first: " + first.ToString() + "last: " + last.ToString() + "sum: " + sum + "p: " + p.ToString());
				GameObject projectile = PhotonNetwork.Instantiate("FireballPrefab", p, Quaternion.identity, 0) as GameObject;
				projectiles.Add(projectile);

				//set life
				projectile.GetComponent<FireballScript>().life = MAX_LIFE;
				
				// keep Hierarchy clean
				projectile.transform.parent = wizardHolder.transform;
			}
		}
		else
			HudScript.addNewMessage("Arc needs more players!", 120, Color.red);
	}

	void Update()
	{
		if(otherWizard != null)
		{
			Vector3 first = transform.position;
			Vector3 last = otherWizard.transform.position;
		
			float fraction = 1.0f / (float)numberOfFireballs;
			float sum = 0;

			for(int i = 0; i < projectiles.Count; i++)
			{
				if(projectiles[i] == null)
				{
					projectiles.RemoveAt(i);
					continue;
				}
				sum += fraction;
				Vector3 p = Vector3.Lerp(first, last, sum);
				projectiles[i].transform.position = p;
			}
		}
	}

	GameObject getClosestWizard(List<GameObject> wizards)
	{
		float closestDistance = 999999;
		GameObject closestWizard = wizards[0];
		print("wizards length: " + wizards.Count);
		foreach(GameObject g in wizards)
		{
			if(g.GetComponent<PhotonView>() == null) continue;
			if(g.GetComponent<PhotonView>().isMine) continue;
			float distance = Vector3.Distance(transform.position, g.transform.position);
			if(distance < closestDistance)
			{
				closestWizard = g;
				closestDistance = distance;
			}
		}

		return closestWizard;
	}

	public override string getAbilityName()
	{
		return "Arc of Fire";
	}
	public override string getAbilityDescription()
	{
		return "it burns, burns, burns...";
	}
}
