using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GLOBAL : MonoBehaviour {

	public static int health = 100;
	public static int maxInventory = 3;
	public static List<GameObject> Inventory = new List<GameObject>();

	void Awake()
	{
		MainCamera = GameObject.FindWithTag("MainCamera") as GameObject;
	}

	public static GameObject MainCamera;
	public static string WizardName;

	//returns false if inventory full.
	public static bool addToInventory(GameObject g)
	{
		if(Inventory.Count < maxInventory)
		{
			g.AddComponent<InventoryItemScript>();
			Inventory.Add(g);
			return true;
		}
		return false;
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
}
