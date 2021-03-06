﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GLOBAL : Photon.MonoBehaviour {

	public static int health = 100;
	public static int maxInventory = 3;
	public static List<GameObject> Inventory = new List<GameObject>();
	public static bool inRoom = false;
	public static bool objectiveMet = false;

	public static GameObject MainCamera;
	public static string WizardName;
	public static bool gameOver;

	public static GLOBAL that;

	static string gameOverTxt;

	public static GameObject myWizard;

	public GameObject OrbOfHopePrefab;
	public GameObject[] FountainItemPrefabs;

	public GameObject _invH;
	static GameObject _InventoryHolder;
	public static Font spookyMagic;

	public static bool isDead = false;

	void Awake(){
		MainCamera = GameObject.FindWithTag("MainCamera") as GameObject;
		that = this;
		_InventoryHolder = _invH;
		spookyMagic = Resources.Load<Font>("SpookyMagic");
	}

	//returns false if inventory full.
	public static void addToInventory(GameObject g)
	{
		// don't show till first item pickup
		_InventoryHolder.SetActive(true);

		g.AddComponent<InventoryItemScript>();
		Inventory.Add(g);
	}

	public static bool isInventoryFull(){
		return Inventory.Count >= maxInventory;
	}

	public static int getInventoryCount()
	{
		return Inventory.Count;
	}

	public static GameObject getInventoryItemAt(int i)
	{
		return Inventory[i];
	}

	public static void useInventoryItemAt(int i){
		// Use item
		GameObject inventoryItem = Inventory[i];
		CollectableBase cb = inventoryItem.GetComponent<CollectableBase>();
		cb.useItem();

		// Adjust remaining quantity of item.
		int quantity = cb.getQuantity();

		if(quantity - 1 <= 0){
			Inventory.RemoveAt(i);
			Destroy(inventoryItem);
		}
		else{
			cb.setQuantity(quantity - 1);
		}
	}

	public static void reset(){
		health = 100;
		gameOver = false;
		Inventory.Clear();
		inRoom = false;

		BETWEEN_SCENES.jeremyEasterEgg = false;
		BETWEEN_SCENES.isaiahEasterEgg = false;

		Leaderboard.gs = Leaderboard.gameState.waiting;

		HudScript.messageQueue.Clear();
		HudScript.hudCamera.SetActive(true);
		isDead = false;
	}

	public static void GameOver(string s){
		GameAudio.stopBattleMusic();
		GameAudio.playGameOver();
		GLOBAL.gameOver = true;
		GLOBAL.health = 0;

		HudScript.hudCamera.SetActive(false);
		gameOverTxt = s;
		Leaderboard.gs = Leaderboard.gameState.leaderboard;

		// save high scores
		// TODO: needs to be networked

		// best individual score
		string newBestIndividualName = "";
		int newBestIndividualPoints = 0;
		int newBestTeamPoints = 0;
		int newBestWaveNum = GameController.that.wave;

		for(int i=0; i < PhotonNetwork.playerList.Length; ++i){
			PhotonPlayer p = PhotonNetwork.playerList[i];
			int score = (int)p.customProperties["Score"];

			if(score > newBestIndividualPoints) {
				newBestIndividualPoints = score;
				newBestIndividualName = p.name;
			}

			newBestTeamPoints += score; 
		}

		int bestIndividualPoints = PlayerPrefs.GetInt("BestIndividualPoints");
		if(newBestIndividualPoints > bestIndividualPoints){
			PlayerPrefs.SetString("BestIndividualName", newBestIndividualName);
			PlayerPrefs.SetInt("BestIndividualPoints", newBestTeamPoints);
			PlayerPrefs.SetInt("BestIndividualTeamPoints", newBestIndividualPoints);
			PlayerPrefs.SetInt("BestIndividualNum", newBestWaveNum);
		}


		// best team score
		int bestTeam = PlayerPrefs.GetInt("BestTeam");
		if(newBestTeamPoints > bestTeam){
			PlayerPrefs.SetInt("BestTeam", newBestTeamPoints);

			for(int i=0; i < PhotonNetwork.playerList.Length; ++i){
				PlayerPrefs.SetString("BestTeamName_" + i, PhotonNetwork.playerList[i].name);
				PlayerPrefs.SetInt("BestTeamScore_" + i, (int)PhotonNetwork.playerList[i].customProperties["Score"]);
			}
		}

		// longest wave (picks player w/highest individual score)
		int longestWave = PlayerPrefs.GetInt("LogenstWaveNum");
		if(newBestWaveNum > longestWave){
			PlayerPrefs.SetString("LongestWaveName", newBestIndividualName);
			PlayerPrefs.SetInt("LongestWaveNum", newBestWaveNum);
		}
	}

	public GameObject SuperInstantiate(GameObject prefab, Vector3 pos, Quaternion rot)
	{
		if(prefab.GetComponent<PhotonView>() != null)
		{
			return (GameObject)PhotonNetwork.InstantiateSceneObject(prefab.name, pos, rot, 0, null);
		}
		else
			return (GameObject)Instantiate(prefab, pos, rot);
	}

	public void SuperDestroy(GameObject g)
	{
		print("superdestroy: " + g);
		if(g.GetComponent<PhotonView>() == null)
		{
			Destroy (g);
			return;
		}
		if(g.GetComponent<PhotonView>().isMine){
			PhotonNetwork.Destroy(g);
			return;
		}

		if(PhotonNetwork.isMasterClient){
			PhotonNetwork.Destroy(g);
		}
		else{
			photonView.RPC("networkDestroyOnMasterClient", PhotonTargets.MasterClient, g.GetComponent<PhotonView>().viewID);
		}
	}

	public void PlaceOrbsOfHope()
	{
		List<GameObject> OrbSpawners = GameObject.FindGameObjectsWithTag("OrbSpawner").OfType<GameObject>().ToList();
		GameObject OrbSpawnerAlways = GameObject.FindWithTag("OrbSpawnerAlways");

		Shuffle(ref OrbSpawners);
		int numberOfOrbsToSpawn = 4;

		for(int i = 0; i < numberOfOrbsToSpawn; i++)
		{
			Vector3 pos = OrbSpawners[i].transform.position;
			GLOBAL.that.SuperInstantiate(OrbOfHopePrefab, pos, Quaternion.identity);
		}

		GLOBAL.that.SuperInstantiate(OrbOfHopePrefab, OrbSpawnerAlways.transform.position, Quaternion.identity);
	}

	public static void Shuffle(ref List<GameObject> list)  
	{  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = Random.Range(0, n + 1);  
			GameObject value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}

	void OnGUI(){
		if(gameOver){
			EZGUI.init();
			EZOpt e = new EZOpt(Color.red, new Color(0.1f, 0.1f, 0.1f));
			e.leftJustify = true;
			e.dropShadowX = e.dropShadowY = 3;

			EZGUI.placeTxt(gameOverTxt, 35, 230, 73, e);
		}
	}

	[RPC]
	public void networkDestroyOnMasterClient(int id)
	{
		PhotonView view = PhotonView.Find(id);
		print("newworkDestroyOnMasterClient id = " + id + " view = " + view);
		if(view == null)
			return;
		PhotonNetwork.Destroy(view.gameObject);
	}

	public static void sendRevivalMessages()
	{
		that.photonView.RPC("revivePlayersRPC", PhotonTargets.All);
	}

	[RPC]
	public void revivePlayersRPC()
	{
		int waveNumver = (int)PhotonNetwork.masterClient.customProperties["Wave"];
		if(waveNumver > 0)
			HudScript.addNewMessage("Wave Complete!", 180, new Color(255, 215, 0));
		if(health <= 0)
			myWizard.GetComponent<PlayerController>().Respawn();
	}

	public static void announceAllOrbsCollected()
	{
		that.photonView.RPC("AllOrbsCollected", PhotonTargets.All);
	}

	public static void announceOrbCollected()
	{
		that.photonView.RPC("orbCollected", PhotonTargets.All);
	}

	[RPC]
	public void orbCollected()
	{
		HolyLightScript.spark();
	}

	[RPC]
	public void AllOrbsCollected()
	{
		int waveNumber = (int)PhotonNetwork.masterClient.customProperties["Wave"];
		if(waveNumber > 0)
		{
			HudScript.addNewMessage("All Orbs of Hope Collected!", 120, new Color(255, 215, 0));
			HudScript.addNewMessage("Items Spawned at fountain!", 120, new Color(255, 215, 0));
		}
	}

	public static void spawnItemsAtFountain()
	{
		int waveNumber = (int)PhotonNetwork.masterClient.customProperties["Wave"];
		if(waveNumber <= 0)
			return;

		GameObject fountain = GameObject.FindWithTag("Fountain");

		int NumberOfItemsToSpawn = Random.Range(10, 20);

		for(int i=0; i < NumberOfItemsToSpawn; ++i){
			int itemIndex = Random.Range(0, that.FountainItemPrefabs.Length);
			GameObject prefab = that.FountainItemPrefabs[itemIndex];

			float randVal = Random.Range(0, 2*Mathf.PI);
			float xval = Mathf.Sin(randVal);
			float zval = Mathf.Cos(randVal);

			Vector3 randVec = new Vector3(xval, 6, zval);

			Vector3 pos = randVec * 10 + fountain.transform.position;

			GameObject item = GLOBAL.that.SuperInstantiate(prefab, pos, Quaternion.identity);
			item.GetComponent<CollectableBase>().setQuantity(Random.Range(1, 5));
			item.AddComponent<FloatDownTillZero>();
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) { }
}
