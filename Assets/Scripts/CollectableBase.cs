using UnityEngine;
using System.Collections;

public abstract class CollectableBase : MonoBehaviour {

	public abstract float getSelectedItemSizeInInventory();
	public abstract float getNonSelectedItemSizeInInventory();
	public abstract string getName();
	public abstract void customUpdate();
	public abstract void useItem();

	public GameObject InventoryVersion;
	GameObject getInventoryVersion() { return InventoryVersion; }

	int quantity = 1;
	public void setQuantity(int n) { quantity = n; }
	public int getQuantity() { return quantity; }
	
	float sinCounter = 0.0f;

	void Update()
	{
		if(GetComponent<PhotonView>() != null)
		if(GetComponent<PhotonView>().isMine)
		{
			sinCounter += 0.025f;
			transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(sinCounter) * 0.01f, transform.position.z);

			if(GetComponent<InventoryItemScript>() == null)
				transform.Rotate(Vector3.up * Time.deltaTime * 55);

			customUpdate();
		}
	}
	
	public void OnCollisionEnter(Collision collision) {

		if(collision.gameObject.tag == "Player" && collision.collider.gameObject.transform.parent.GetComponent<PhotonView>().isMine)
		{
			if(!GLOBAL.isInventoryFull())
			{
				string quantityString = "";
				if(quantity > 1)
					quantityString = "(" + quantity + ")";
				HudScript.addNewMessage(getName() + " " + quantityString, 120, Color.white);



				GameObject newItem = Instantiate(getInventoryVersion()) as GameObject;
				GLOBAL.addToInventory(newItem);

				newItem.GetComponent<CollectableBase>().setQuantity(quantity);

				GLOBAL.that.SuperDestroy(gameObject);
			}
			else
			{
				HudScript.addNewMessage("Inventory full!", 120, Color.red);
			}
		}
	}
}
