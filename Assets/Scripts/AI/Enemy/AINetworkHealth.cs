using UnityEngine;
using System.Collections;
using RAIN.Core;
using RAIN.Action;

public class AINetworkHealth : MonoBehaviour {

	[RPC]
	public void setHealthRPC(float[] health)
	{
		this.GetComponentInChildren<SampleAIController>().SetHealth(health[0]);
		this.GetComponentInChildren<SampleAIController>().invincibilityTimer = 4;
		if( health.Length == 2 ) 
		{
			if( (int)health[1] == 0 )
				this.GetComponentInChildren<SampleAIController>().turnRed = true;
			else
			{
				this.GetComponentInChildren<SampleAIController>().turnRed = false;
			}
		}
		
	}
	[RPC]
	public void setSpeedRPC(float[] speed)
	{
		this.GetComponentInChildren<SampleAIController>().speedBeingSent = true;
		this.transform.FindChild("AI").GetComponent<AIRig>().AI.Motor.DefaultSpeed = speed[0];	
	}
	[RPC]
	public void setScoreValueRPC(int[] score)
	{
		this.GetComponentInChildren<SampleAIController>().scoreValue = score[0];
	}
	[RPC]
	public void setInitialSpeedRPC(float[] speed)
	{
		this.GetComponentInChildren<SampleAIController>().speed = speed[0];
	}

}
