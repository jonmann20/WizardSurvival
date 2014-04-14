using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GLOBAL : Photon.MonoBehaviour {

	public static int health = 100;
	public static int maxInventory = 3;
	public static List<GameObject> Inventory = new List<GameObject>();
	public static bool inRoom = false;

	public static GameObject MainCamera;
	public static string WizardName;
	public static bool gameOver;

	public static GLOBAL that;

	void Awake()
	{
		MainCamera = GameObject.FindWithTag("MainCamera") as GameObject;
		that = this;
	}



	//returns false if inventory full.
	public static void addToInventory(GameObject g)
	{
		g.AddComponent<InventoryItemScript>();
		Inventory.Add(g);
	}

	public static bool isInventoryFull()
	{
		if(Inventory.Count < maxInventory)
			return false;
		return true;
	}

	public static int getInventoryCount()
	{
		return Inventory.Count;
	}
	public static GameObject getInventoryItemAt(int i)
	{
		return Inventory[i];
	}
	public static void useInventoryItemAt(int i)
	{
		//Use item
		GameObject inventoryItem = Inventory[i];

		inventoryItem.GetComponent<CollectableBase>().useItem();

		//Adjust remaining quantity of item.
		int quantity = inventoryItem.GetComponent<CollectableBase>().getQuantity();

		if(quantity - 1 <= 0)
		{
			Inventory.RemoveAt(i);
			Destroy(inventoryItem);
		}
		else
			inventoryItem.GetComponent<CollectableBase>().setQuantity(quantity - 1);
	}

	public static void reset()
	{
		health = 100;
		gameOver = false;
		Inventory.Clear();
		inRoom = false;
	}
	

	public void SuperDestroy(GameObject g)
	{
		if(g.GetComponent<PhotonView>() == null)
		{
			Destroy (g);
			return;
		}

		if(g.GetComponent<PhotonView>().isMine)
			PhotonNetwork.Destroy(g);

		if(PhotonNetwork.isMasterClient)
		{
			PhotonNetwork.Destroy(g);
		}
		else
		{
			photonView.RPC("networkDestroyOnMasterClient", PhotonTargets.MasterClient, g.GetComponent<PhotonView>().viewID);
		}
	}

	[RPC]
	public void networkDestroyOnMasterClient(int id)
	{
		PhotonView view = PhotonView.Find(id);
		PhotonNetwork.Destroy(view.gameObject);
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) { }

	public static IEnumerator ChangeSceneWithDelay(string sceneName, int delay)
	{
		yield return new WaitForSeconds(delay);
		Application.LoadLevel(sceneName);
	}

	public static IEnumerator QuitWithDelay(int delay)
	{
		yield return new WaitForSeconds(delay);
		Application.Quit();
	}
}
