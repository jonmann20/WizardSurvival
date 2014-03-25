using UnityEngine;
using System.Collections;
using RAIN.Core;
using RAIN.Action;

public class FollowClosestPlayer : MonoBehaviour {
	
	public float howOftenToCheckForClosestPlayer = 2.0f;

	void Start()
	{
		StartCoroutine("FindClosestPlayer");
	}

	IEnumerator FindClosestPlayer()
	{

		GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");
		
		if( playerList.Length > 1 )
		{
			//Find closest player
			float tempX= Mathf.Abs(playerList[0].transform.position.x - this.transform.position.x);
			float tempY= Mathf.Abs(playerList[0].transform.position.y - this.transform.position.y);
			float tempZ= Mathf.Abs(playerList[0].transform.position.z - this.transform.position.z);
			float closestDistance = tempX + tempY + tempZ;
			int closePlayerIdx = 0;
			
			for (int i = 0; i < playerList.Length; i++)
			{
				tempX= Mathf.Abs(playerList[i].transform.position.x - this.transform.position.x);
				tempY= Mathf.Abs(playerList[i].transform.position.y - this.transform.position.y);
				tempZ= Mathf.Abs(playerList[i].transform.position.z - this.transform.position.z);
				float tempDistance = tempX + tempY + tempZ;
				if( tempDistance < closestDistance )
				{
					closestDistance = tempDistance;
					closePlayerIdx = i;
				}
			}
			this.transform.FindChild("AI").GetComponent<AIRig>().AI.WorkingMemory.SetItem("detectObject2", playerList[closePlayerIdx]);
		}
		else if ( playerList.Length == 1 )
		{
			this.transform.FindChild("AI").GetComponent<AIRig>().AI.WorkingMemory.SetItem("detectObject2", playerList[0]);
		}

		yield return new WaitForSeconds(howOftenToCheckForClosestPlayer);
		StartCoroutine("FindClosestPlayer");

	}
}
