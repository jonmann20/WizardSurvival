using UnityEngine;
using System.Collections;

public class ItemSpawnerOnce : MonoBehaviour {
	
	public GameObject ItemPrefab;
	public bool addItemPosNRot = false;
	public int ItemQuantity;
	
	bool attemptedSpawn = false;

	//When the player connects, if the player is master spawn item.
	//ELSE, never spawn item + Destroy self.
	void Update()
	{
		if(!attemptedSpawn && GLOBAL.inRoom)
		{
			attemptedSpawn = true;
			if(PhotonNetwork.isMasterClient)
				spawn();
		}
	}
	
	void spawn(){

		Vector3 pos = transform.position;
		Quaternion quat = transform.rotation;
		if(addItemPosNRot) {
			pos += ItemPrefab.transform.position;
			quat *= Quaternion.Euler(ItemPrefab.transform.localEulerAngles);
		}

		GameObject item = GLOBAL.that.SuperInstantiate(ItemPrefab, pos, quat);

        item.transform.parent = transform;

            // If the spawned item is a collectable, give it a quantity
            if(item.GetComponent<CollectableBase>() != null) {
                item.GetComponent<CollectableBase>().setQuantity(ItemQuantity);
			}
	}
}

