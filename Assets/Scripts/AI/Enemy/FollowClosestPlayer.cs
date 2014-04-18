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
		int closePlayerIdx = 0;

		
		for (int i = 0; i < playerList.Length - 1; i++)
		{
			if( playerList[i].gameObject.transform.parent == null )
			{
				if( playerList[i].gameObject.GetComponent<PlayerController>().getHealth() > 0 )
				{
					float tempDistance = Mathf.Abs(Vector3.Distance( playerList[i].gameObject.transform.position, this.gameObject.transform.position));
					if( tempDistance < closestDistance)
					{
						closestDistance = tempDistance;
						closePlayerIdx = i;
					}
				}
			}
		}

		if( playerList[closePlayerIdx] == null )
		{
			Debug.LogError("No players for AI to find");
		}
		this.transform.FindChild("AI").GetComponent<AIRig>().AI.WorkingMemory.SetItem("detectObject2", playerList[closePlayerIdx]);
		/*if( closestDistance < meleeDistance )
		{
			print ("close enough to melee");
			this.transform.FindChild("AI").GetComponent<AIRig>().AI.WorkingMemory.SetItem("attackPlayer", playerList[closePlayerIdx]);
		}
		else
		{
			this.transform.FindChild("AI").GetComponent<AIRig>().AI.WorkingMemory.SetItem("attackPlayer", null);
		}*/

		//}
		/*else if ( playerList.Length == 1 )
		{
			this.transform.FindChild("AI").GetComponent<AIRig>().AI.WorkingMemory.SetItem("detectObject2", playerList[0]);
		}*/

		yield return new WaitForSeconds(howOftenToCheckForClosestPlayer);
		StartCoroutine("FindClosestPlayer");

	}
}
