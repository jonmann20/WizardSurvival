using UnityEngine;
using System.Collections;

public class NetworkLauncherScript : MonoBehaviour {

	
	public GameObject WizardPrefab;
	public GameObject DebugGroundPrefab;
	public int IDOfPreviousMasterClient = -1;
	
	void Start () {
		IDOfPreviousMasterClient = PhotonNetwork.masterClient.ID;
		GameObject wiz = PhotonNetwork.Instantiate("Wizard", new Vector3(-189, 4.9f, 87.6f), Quaternion.identity, 0) as GameObject;
		wiz.transform.position = new Vector3(-129, 8.71f, 87.3f);
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

		if(everyoneZeroHealth)
		{
			HudScript.addNewMessage("Game Over", 180, Color.red);

			GLOBAL.gameOver = true;
		}
	}

	bool hasMasterClientDisconnected()
	{
		if(PhotonNetwork.masterClient.ID != IDOfPreviousMasterClient) 
			return true;
		return false;
	}
}
