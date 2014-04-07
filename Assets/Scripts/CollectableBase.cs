using UnityEngine;
using System.Collections;

public abstract class CollectableBase : MonoBehaviour {

	public abstract float getSelectedItemSizeInInventory();
	public abstract float getNonSelectedItemSizeInInventory();
	public abstract string getName();
	public abstract void customUpdate();
	public abstract void useItem();

	int quantity = 1;
	public void setQuantity(int n) { quantity = n; }
	public int getQuantity() { return quantity; }
	
	float sinCounter = 0.0f;
	Vector3 initialPosition;
	void Awake () {
		initialPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
	}
	
	void Update()
	{
		sinCounter += 0.025f;
		transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(sinCounter) * 0.01f, transform.position.z);

		if(GetComponent<InventoryItemScript>() == null)
			transform.Rotate(Vector3.up * Time.deltaTime * 55);

		customUpdate();
	}
	
	public void OnCollisionEnter(Collision collision) {

		if(collision.gameObject.tag == "Player")
		{

			if(collision.gameObject.GetComponent<PhotonView>().isMine)
			{
				string quantityString = "";
				if(quantity > 1)
					quantityString = "(" + quantity + ")";
				


				GameObject NewInventoryItem = Instantiate(gameObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
				Destroy(NewInventoryItem.GetComponent<PhotonView>());
				NewInventoryItem.GetComponent<CollectableBase>().setQuantity(quantity);
				if(GLOBAL.addToInventory(NewInventoryItem))
				{
					Destroy(gameObject);
					HudScript.setNewMessage(getName() + " " + quantityString, 120, Color.white);
				}
				else
				{
					Destroy(NewInventoryItem);
					HudScript.setNewMessage("Inventory full!", 120, Color.red);
				}
			}
		}
	}
}
