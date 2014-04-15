using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;
using RAIN.Representation;

[RAINAction]
public class AIShootProjectile : RAINAction
{
	public Expression enemy;
	private Health enemyHealth = null;
	private GameObject enemyObject = null;

    public AIShootProjectile()
    {
        actionName = "AIShootProjectile";
    }

    public override void Start(AI ai)
    {
		enemyObject = enemy.Evaluate(ai.DeltaTime, ai.WorkingMemory).GetValue<GameObject>();
		if( enemyObject != null )
		{
			//enemyHealth = enemyObject.GetComponent<Health>();
		}
        base.Start(ai);
    }

    public override ActionResult Execute(AI ai)
    {
		Vector3 spawnLoc = ai.Body.transform.position + (ai.Body.transform.forward.normalized * 1.5f);
		spawnLoc.y += 0.8f;

		GameObject projectile = PhotonNetwork.Instantiate(
			"MageOrb", 
			spawnLoc,  
			ai.Body.transform.rotation, 
			0
		) as GameObject;

		//set velocity
		Vector3 vel = ai.Body.transform.forward * 15;
		vel.y = 0;
		projectile.rigidbody.velocity = vel;

		projectile.GetComponent<MageOrbScript>().damageToApply = (int) ai.WorkingMemory.GetItem("damageToApply").GetValue<int>(); 

        return ActionResult.SUCCESS;
    }

    public override void Stop(AI ai)
    {
        base.Stop(ai);
    }
}