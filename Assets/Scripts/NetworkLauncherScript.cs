using UnityEngine;
using System.Collections;

public class NetworkLauncherScript : MonoBehaviour {

	
	public GameObject WizardPrefab;
	public GameObject DebugGroundPrefab;
	public int IDOfPreviousMasterClient = -1;
	
	void Start () {
		IDOfPreviousMasterClient = PhotonNetwork.masterClient.ID;
		GameObject wiz = PhotonNetwork.Instantiate("Wizard", new Vector3(1.577f, 9.592f, -61.181f), Quaternion.identity, 0) as GameObject;
		GameObject mainCam = GameObject.FindWithTag("MainCamera") as GameObject;
		(mainCam.GetComponent<MouseCamera>() as MouseCamera).target = wiz;
	
        // keep Hierachy clean
        wiz.transform.parent = GameObject.Find("_WizardHolder").transform;
	}
	
	void Update () {
		if(!GLOBAL.gameOver){
			if(hasMasterClientDisconnected()){
				GLOBAL.GameOver("The Host Disconnected...");
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

			if(everyoneZeroHealth){
				GLOBAL.GameOver("Game Over");
			}

			if(PhotonNetwork.connected == false) {
				GLOBAL.GameOver("Online Connection Lost...");
			}
		}

		//OrbsOfHope
		if(PhotonNetwork.isMasterClient)
		{
			if(AreAllOrbsCollectedAndReport())
			{
				GLOBAL.announceAllOrbsCollected();
				GLOBAL.spawnItemsAtFountain();
				GLOBAL.that.PlaceOrbsOfHope();
			}
		}
	}

	bool AreAllOrbsCollectedAndReport()
	{
		GameObject[] existingOrbs = GameObject.FindGameObjectsWithTag("OrbOfHope");
		GLOBAL.myWizard.GetComponent<PlayerController>().networkedProperties["Orbs"] = existingOrbs.Length;

		if(existingOrbs.Length <= 0)
			return true;
		else
			return false;
	}

	bool hasMasterClientDisconnected()
	{
		if(PhotonNetwork.masterClient == null)
			return false;
		if(PhotonNetwork.masterClient.ID != IDOfPreviousMasterClient) 
			return true;
		return false;
	}
}
