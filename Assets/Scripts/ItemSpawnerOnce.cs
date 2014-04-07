using UnityEngine;
using System.Collections;

public class ItemSpawnerOnce : MonoBehaviour {

	public GameObject ItemPrefab;
	public int ItemQuantity;

	bool spawned = false;

	// Use this for initialization
	void Start () {
		//PhotonNetwork.InstantiateSceneObject("EnemyWithAI 1", this.transform.position + Random.onUnitSphere, Quaternion.identity,0, null) as GameObject;

	}

	void Update()
	{
		if(!spawned)
		{
			if(PhotonNetwork.connected)
			{
				//Only the master client can spawn scene objects
				//if(PhotonNetwork.isMasterClient)
				//{
					spawn();
					spawned = true;
				//}
			}
		}
	}

	void spawn()
	{
		GameObject item = new GameObject();
		if(ItemPrefab.name != "health_potion" && ItemPrefab.name != "BookNTomeFireball")
		{
			//item = PhotonNetwork.InstantiateSceneObject(ItemPrefab.name, new Vector3(0, 0, 0), Quaternion.identity, 0, null) as GameObject;
			item = Instantiate(ItemPrefab, transform.position, Quaternion.identity) as GameObject;

			print("spawned a " + ItemPrefab.tag);
			if(ItemPrefab.tag == "Tome")
			{
				print("before");
				//ItemPrefab.transform.Rotate(-90, 0, 0);
				print("after");
			}
		}
		else if(ItemPrefab.name == "health_potion")
			item = Instantiate(ItemPrefab, transform.position, Quaternion.identity) as GameObject;
		else if(ItemPrefab.name == "BookNTomeFireball")
			item = Instantiate(ItemPrefab, transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject;
			//item = PhotonNetwork.InstantiateSceneObject(ItemPrefab.name, new Vector3(0, 0, 0), Quaternion.Euler(-90, 0, 0), 0, null) as GameObject;

		item.transform.parent = transform;
		item.transform.localPosition = new Vector3(0, 0, 0);

		if(item.GetComponent<CollectableBase>() != null)
			item.GetComponent<CollectableBase>().setQuantity(ItemQuantity);
	}
}
