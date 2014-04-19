using UnityEngine;
using System.Collections;
using RAIN.Core;
using RAIN.Action;

public class FollowClosestPlayer : MonoBehaviour {
	
	public float howOftenToCheckForClosestPlayer = 2.0f;

	public float meleeDistance = 100f;

	void Start()
	{
		StartCoroutine("FindClosestPlayer");
	}

	IEnumerator FindClosestPlayer()
	{

		GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");

		float closestDistance = float.MaxValue;
		int closePlayerIdx = -1;

		for (int i = 0; i < playerList.Length ; i++)
		{
			if( playerList[i].GetComponent<PhotonView>() == null )
			{
				continue;
			}

			PhotonPlayer p = playerList[i].GetComponent<PhotonView>().owner;

			if( p == null ) 
			{
				continue;
			}

			if(p.customProperties.ContainsKey("Health") && ((int)p.customProperties["Health"]) > 0)
			{
				float tempDistance = Mathf.Abs(Vector3.Distance( playerList[i].gameObject.transform.position, this.gameObject.transform.position));
				if( tempDistance < closestDistance)
				{
					closestDistance = tempDistance;
					closePlayerIdx = i;
				}
			}

		}

		if( closePlayerIdx == -1 )
		{
			this.transform.FindChild("AI").GetComponent<AIRig>().AI.WorkingMemory.SetItem("detectObject2", null);

		}
		else
		{
			this.transform.FindChild("AI").GetComponent<AIRig>().AI.WorkingMemory.SetItem("detectObject2", playerList[closePlayerIdx]);
		}

		yield return new WaitForSeconds(howOftenToCheckForClosestPlayer);
		StartCoroutine("FindClosestPlayer");

	}
}
