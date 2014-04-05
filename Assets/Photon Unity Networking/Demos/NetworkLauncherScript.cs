using UnityEngine;
using System.Collections;

public class NetworkLauncherScript : MonoBehaviour {

	
	public GameObject WizardPrefab;
	public GameObject DebugGroundPrefab;
	public bool gameOver = false;
	public int gameOverTimer = 180;
	
	void Start () {
		GameObject wiz = PhotonNetwork.Instantiate("Wizard", new Vector3(0, 5, 0), Quaternion.identity, 0) as GameObject;
		GameObject mainCam = GameObject.FindWithTag("MainCamera") as GameObject;
		(mainCam.GetComponent<MouseCamera>() as MouseCamera).target = wiz;


        // keep Hierachy clean
        wiz.transform.parent = GameObject.Find("_WizardHolder").transform;
	}
	
	void Update () {

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
			//gameOver = true;
			//HudScript.setNewMessage("Game Over", 180);

			LeaderboardScript.endGame();

		}

		//Game Over Timer -- when it reaches zero, boot players back to title screen.
//		if(gameOver) gameOverTimer --;
//		if(gameOverTimer <= 0)
//			Application.LoadLevel("Title");
	}
}
