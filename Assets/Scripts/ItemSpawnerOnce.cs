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
		print("is master client: " + PhotonNetwork.isMasterClient);

		if(!spawned)
		{
			if(PhotonNetwork.connected)
			{
				//Only the master client can spawn scene objects
				if(PhotonNetwork.isMasterClient)
				{
					spawn ();
					spawned = true;
				}
			}
		}
	}

	void spawn()
	{
		print("NAME:" + ItemPrefab.name + "photon view:" + ItemPrefab.GetComponent<PhotonView>());
		GameObject item;
		if(ItemPrefab.name != "health_potion")
			item = PhotonNetwork.InstantiateSceneObject(ItemPrefab.name, new Vector3(0, 0, 0), Quaternion.identity, 0, null) as GameObject;
		item = Instantiate(ItemPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

		item.transform.parent = transform;
		item.transform.localPosition = new Vector3(0, 0, 0);
		
		item.GetComponent<CollectableBase>().setQuantity(ItemQuantity);
	}
}
