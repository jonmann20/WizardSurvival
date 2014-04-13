using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;
using RAIN.Representation;

[RAINAction]
public class AImeleeAttack : RAINAction
{
	public Expression enemy;
	private Health enemyHealth = null;
	private GameObject enemyObject = null;
	
	public AImeleeAttack()
	{
		actionName = "AImeleeAttack";
	}
	
	public override void Start(AI ai)
	{
		//enemyObject = enemy.Evaluate(ai.DeltaTime, ai.WorkingMemory).GetValue<GameObject>();
		if( enemyObject != null )
		{
			//enemyHealth = enemyObject.GetComponent<Health>();
		}
		base.Start(ai);
	}
	
	public override ActionResult Execute(AI ai)
	{
		//enemyObject = enemy.Evaluate(ai.DeltaTime, ai.WorkingMemory).GetValue<GameObject>();
		enemyObject = ai.WorkingMemory.GetItem("Wizard").GetValue<RAIN.Entities.Aspects.VisualAspect>().Entity.Form;
		if( enemyObject != null )
		{

			enemyObject.GetComponent<PlayerController>().TakeDamage(20,ai.Body.transform);

		}
		if( enemyObject == null )
		{
			Debug.Log("no player to hit");
		}
		
		return ActionResult.SUCCESS;
	}
	
	public override void Stop(AI ai)
	{
		base.Stop(ai);
	}
}