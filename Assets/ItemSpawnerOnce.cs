using UnityEngine;
using System.Collections;

public class ItemSpawnerOnce : MonoBehaviour {

	public GameObject ItemPrefab; 
	public int ItemQuantity = 1;

	// Use this for initialization
	void Start () {
		GameObject item = Instantiate(ItemPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		item.transform.parent = transform;
		item.transform.localPosition = new Vector3(0, 0, 0);
		item.GetComponent<InventoryItemScript>().setQuantity(ItemQuantity);
	}
}
