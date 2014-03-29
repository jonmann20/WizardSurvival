using UnityEngine;
using System.Collections;

public class CollectableScript : MonoBehaviour {
	
	public GameObject ItemPrefab;

	float sinCounter = 0.0f;
	Vector3 initialPosition;
	void Start () {
		initialPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

		GameObject item = Instantiate(ItemPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
		item.transform.parent = transform;
		item.transform.localPosition = new Vector3(0, 0, 0);
	}

	void Update()
	{
		sinCounter += 0.025f;
		transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(sinCounter) * 0.01f, transform.position.z);

        transform.Rotate(Vector3.up * Time.deltaTime * 55);
	}

	void OnCollisionEnter(Collision collision) {

		if(collision.gameObject.tag == "Player")
		{

			if(collision.gameObject.GetComponent<PhotonView>().isMine)
			{
				GameObject NewInventoryItem = Instantiate(ItemPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
				if(GLOBAL.addToInventory(NewInventoryItem))
					Destroy(gameObject);
				else
					Destroy(NewInventoryItem);
			}
		}
	}
}
