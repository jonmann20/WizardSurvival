using UnityEngine;
using System.Collections;

public class InventoryItemScript : MonoBehaviour {

	string itemName = "...";
	void Start () {
		//collision.gameObject.GetComponent<AbilityManagerScript>().changeAbility(BestowedAbilityName);
		GLOBAL.Inventory.Add(gameObject);
	}

	void Update () {
	
	}

	void setName(string n)
	{
		itemName = n;
	}
}
