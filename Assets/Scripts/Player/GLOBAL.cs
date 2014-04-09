using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GLOBAL : MonoBehaviour {

	public static int health = 100;
	public static int maxInventory = 3;
	public static List<GameObject> Inventory = new List<GameObject>();
	public static bool inRoom = false;

	void Awake()
	{
		MainCamera = GameObject.FindWithTag("MainCamera") as GameObject;
	}

	public static GameObject MainCamera;
	public static string WizardName;
	public static bool gameOver;

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

	public static void SuperDestroy(GameObject g)
	{
		if(g.GetComponent<PhotonView>() == null)
		{
			print("destroying because it isn't networked.");
			Destroy (g);
			return;
		}

		if(PhotonNetwork.isMasterClient)
		{
			print("Destroying because I'm the master.");
			PhotonNetwork.Destroy(g);
		}
		else
		{
			print("trying to destroy object with id: " + g.GetComponent<PhotonView>().viewID);
			g.GetComponent<PhotonView>().RPC("networkDestroyOnMasterClient", PhotonTargets.All, g.GetComponent<PhotonView>().viewID);
		}
	}

	[RPC]
	public static void networkDestroyOnMasterClient(PhotonView v)
	{
		print("DELETING OBJECT VIA RPC: " + v);
		PhotonNetwork.Destroy(v.gameObject);
	}
}
