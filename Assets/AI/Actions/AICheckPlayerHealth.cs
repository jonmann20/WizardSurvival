using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;
using RAIN.Representation;

[RAINAction]
public class AICheckPlayerHealth : RAINAction
{
	public Expression enemy;
	//private Health enemyHealth = null;
	private GameObject enemyObject = null;
	
	public AICheckPlayerHealth()
	{
		actionName = "AICheckPlayerHealth";
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
		if( ai.WorkingMemory.GetItem("Wizard").GetValue<RAIN.Entities.Aspects.VisualAspect>() == null )
		{
			enemyObject = null;
			return ActionResult.SUCCESS;
		}
		else
			enemyObject = ai.WorkingMemory.GetItem("Wizard").GetValue<RAIN.Entities.Aspects.VisualAspect>().Entity.Form;
		if( enemyObject != null )
		{
			if( enemyObject.GetPhotonView().isMine )
			{
				if ((int)enemyObject.GetPhotonView().owner.customProperties["Health"] <= 0 )
				{
					ai.WorkingMemory.SetItem("ShouldAttack", false );
				}
				else
				{
					ai.WorkingMemory.SetItem("ShouldAttack", true );
				}
			}
			
		}
		
		return ActionResult.SUCCESS;
	}
	
	public override void Stop(AI ai)
	{
		base.Stop(ai);
	}
}