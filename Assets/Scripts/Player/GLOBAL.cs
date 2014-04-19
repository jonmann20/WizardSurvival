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
		// TODO: need to be networked
		// best individual score
		PlayerPrefs.SetString("BestIndividualName", "Tim");
		PlayerPrefs.SetInt("BestIndividualPoints", 1000);
		PlayerPrefs.SetInt("BestIndividualTeamPoints", 2000);
		PlayerPrefs.SetInt("BestIndividualNum", 10);

		// best team score
		PlayerPrefs.SetInt("BestTeam", 3000);
			// player 0
			PlayerPrefs.SetString("BestTeamName_0", "Tim");
			PlayerPrefs.SetInt("BestTeamScore_0", 100);
			// player 1
			PlayerPrefs.SetString("BestTeamName_1", "Bill");
			PlayerPrefs.SetInt("BestTeamScore_1", 200);
			// player 2
			PlayerPrefs.SetString("BestTeamName_2", "Smith");
			PlayerPrefs.SetInt("BestTeamScore_2", 200);
			// player 3
			PlayerPrefs.SetString("BestTeamName_3", "Ihavealongname");
			PlayerPrefs.SetInt("BestTeamScore_4", 1200);

		// longest wave
		PlayerPrefs.SetString("LongestWaveName", "Tim");
		PlayerPrefs.SetInt("LongestWaveNum", 12);
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
		print("trying to destroy via RPC view: " + view + " id: " + id);
		PhotonNetwork.Destroy(view.gameObject);
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) { }
}
