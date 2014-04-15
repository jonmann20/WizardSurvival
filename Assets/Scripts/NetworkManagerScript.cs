using UnityEngine;
using System.Collections;

public class NetworkManagerScript : MonoBehaviour {
	
	
	public GameObject WizardPrefab;
	public int IDOfPreviousMasterClient = -1;
	int previousMasterDelay = 60;

	void Start () {
		IDOfPreviousMasterClient = PhotonNetwork.masterClient.ID;
		GameObject wiz = PhotonNetwork.Instantiate("Wizard", new Vector3(1.577f, 9.592f, -61.181f), Quaternion.identity, 0) as GameObject;
		GameObject mainCam = GameObject.FindWithTag("MainCamera") as GameObject;
		(mainCam.GetComponent<MouseCamera>() as MouseCamera).target = wiz;
		
		// keep Hierachy clean
		wiz.transform.parent = GameObject.Find("_WizardHolder").transform;
	}
	
	void Update () {
		
		if(hasMasterClientDisconnected() && !GLOBAL.gameOver)
		{
			GLOBAL.gameOver = true;
			GLOBAL.health = 0;
			HudScript.addNewMessage("The Host Disconnected...", 180, Color.red);
		}
		
		bool everyoneZeroHealth = true;
		//CHECK IF EVERYONE IS DEAD
		for( int i = 0; i < PhotonNetwork.playerList.Length; i++ )
		{
			if( PhotonNetwork.playerList[i].customProperties.ContainsKey("Health"))
			{
				float tempHealth = (int) PhotonNetwork.playerList[i].customProperties["Health"];
				if(tempHealth > 0.0f)
				{
					everyoneZeroHealth = false;
				}
			}
		}
		
		if(everyoneZeroHealth && previousMasterDelay <= 0)
		{
			HudScript.addNewMessage("Game Over", 180, Color.red);
			
			GLOBAL.gameOver = true;
		}
	}
	
	bool hasMasterClientDisconnected()
	{
		if(previousMasterDelay > 0)
			previousMasterDelay --;

		if(previousMasterDelay <= 0 && PhotonNetwork.masterClient.ID != IDOfPreviousMasterClient) 
			return true;

		return false;
	}
}
