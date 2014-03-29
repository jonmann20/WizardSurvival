using UnityEngine;
using System.Collections;

public abstract class CollectableBase : MonoBehaviour {

	public abstract float getSelectedItemSizeInInventory();
	public abstract float getNonSelectedItemSizeInInventory();
	public abstract void customUpdate();
	public abstract void useItem();
	
	float sinCounter = 0.0f;
	Vector3 initialPosition;
	void Awake () {
		initialPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
	}
	
	void Update()
	{
		sinCounter += 0.025f;
		transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(sinCounter) * 0.01f, transform.position.z);
		
		transform.Rotate(Vector3.up * Time.deltaTime * 55);

		customUpdate();
	}
	
	public void OnCollisionEnter(Collision collision) {

		if(collision.gameObject.tag == "Player")
		{
			
			if(collision.gameObject.GetComponent<PhotonView>().isMine)
			{
				GameObject NewInventoryItem = Instantiate(gameObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
				if(GLOBAL.addToInventory(NewInventoryItem))
					Destroy(gameObject);
				else
					Destroy(NewInventoryItem);
			}
		}
	}
}
