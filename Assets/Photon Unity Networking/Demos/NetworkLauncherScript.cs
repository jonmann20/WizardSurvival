using UnityEngine;
using System.Collections;

public class NetworkLauncherScript : MonoBehaviour {

	
	public GameObject WizardPrefab;
	public GameObject DebugGroundPrefab;
	
	void Start () {
		
		if(PhotonNetwork.countOfPlayers == 1)
			PhotonNetwork.InstantiateSceneObject("DebugGround", new Vector3(0, 0, 0), Quaternion.identity, 0, null);
		
		GameObject wiz = PhotonNetwork.Instantiate("Wizard", new Vector3(0, 5, 0), Quaternion.identity, 0) as GameObject;
		GameObject mainCam = GameObject.FindWithTag("MainCamera") as GameObject;
		(mainCam.GetComponent<MouseCamera>() as MouseCamera).target = wiz;
	}
	
	void Update () {
		
	}
}
