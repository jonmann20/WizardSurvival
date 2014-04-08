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
		
		//if( playerList.Length > 1 )
		//{
			//Find closest player
		float tempX= Mathf.Abs(Mathf.Abs(playerList[0].gameObject.transform.position.x) - Mathf.Abs(this.gameObject.transform.position.x));
		float tempY= Mathf.Abs(Mathf.Abs(playerList[0].gameObject.transform.position.y) - Mathf.Abs(this.gameObject.transform.position.y));
		float tempZ= Mathf.Abs(Mathf.Abs(playerList[0].gameObject.transform.position.z) - Mathf.Abs(this.gameObject.transform.position.z));
		//Vector3 distanceToPlayer = playerList[0].gameObject.transform.position - this.gameObject.transform.position;
		float closestDistance = Mathf.Abs(Vector3.Distance( playerList[0].gameObject.transform.position, this.gameObject.transform.position));
		int closePlayerIdx = 0;
		
		for (int i = 1; i < playerList.Length - 1; i++)
		{
			tempX= Mathf.Abs(Mathf.Abs(playerList[i].gameObject.transform.position.x) - Mathf.Abs(this.gameObject.transform.position.x));
			tempY= Mathf.Abs(Mathf.Abs(playerList[i].gameObject.transform.position.y) - Mathf.Abs(this.gameObject.transform.position.y));
			tempZ= Mathf.Abs(Mathf.Abs(playerList[i].gameObject.transform.position.z) - Mathf.Abs(this.gameObject.transform.position.z));
			float tempDistance = Mathf.Abs(Vector3.Distance( playerList[i].gameObject.transform.position, this.gameObject.transform.position));
			if( tempDistance < closestDistance )
			{
				closestDistance = tempDistance;
				closePlayerIdx = i;
			}
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
