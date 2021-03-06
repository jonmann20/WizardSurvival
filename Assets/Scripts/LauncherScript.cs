﻿using UnityEngine;
using System.Collections;

public class LauncherScript : MonoBehaviour {

	public GameObject WizardPrefab;
	public GameObject DebugGroundPrefab;

	void Start(){

		//if(PhotonNetwork.countOfPlayers == 1)
		//	Instantiate(DebugGroundPrefab, new Vector3(0, 0, 0), Quaternion.identity);

		GameObject wiz = Instantiate(WizardPrefab, new Vector3(1.577f, 9.592f, -61.181f), Quaternion.identity) as GameObject;
		GameObject mainCam = GameObject.FindWithTag("MainCamera") as GameObject;
		(mainCam.GetComponent<MouseCamera>() as MouseCamera).target = wiz;
	}
}
