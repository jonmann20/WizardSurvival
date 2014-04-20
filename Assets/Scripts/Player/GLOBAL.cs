using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	public GameObject _invH;
	static GameObject _InventoryHolder;
	public static Font spookyMagic;

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

		Leaderboard.gs = Leaderboard.gameState.waiting;

		HudScript.messageQueue.Clear();
		HudScript.hudCamera.SetActive(true);
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
				newBestIndividualName = "TODO: network names"; // (int)p.customProperties["Name"];
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
				PlayerPrefs.SetString("BestTeamName_" + i, "TODO: network names");
				PlayerPrefs.SetInt("BestTeamScore_" + i, (int)PhotonNetwork.playerList[i].customProperties["Score"]);
			}
		}

		// longest wave
		int longestWave = PlayerPrefs.GetInt("LogenstWaveNum");
		if(newBestWaveNum > longestWave){
			PlayerPrefs.SetString("LongestWaveName", "TODO: network names");
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

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) { }
}
