using UnityEngine;
using System.Collections;
using RAIN.Core;
using RAIN.Action;

public class AINetworkHealth : MonoBehaviour {

	[RPC]
	public void setHealthRPC(float[] health)
	{
		this.GetComponentInChildren<SampleAIController>().SetHealth(health[0]);
		
	}
	[RPC]
	public void setSpeedRPC(float[] speed)
	{
		this.transform.FindChild("AI").GetComponent<AIRig>().AI.Motor.DefaultSpeed = speed[0];	
	}
	[RPC]
	public void setScoreValueRPC(int[] score)
	{
		this.GetComponentInChildren<SampleAIController>().scoreValue = score[0];
	}
}
