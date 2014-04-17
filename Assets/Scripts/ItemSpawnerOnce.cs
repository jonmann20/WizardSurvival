﻿using UnityEngine;
using System.Collections;

public class ItemSpawnerOnce : MonoBehaviour {
	
	public GameObject ItemPrefab;
	public bool addItemPosition = false;
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
			else
				GLOBAL.that.SuperDestroy(gameObject);
		}
	}
	
	void spawn(){

		Vector3 pos = transform.position;
		if(addItemPosition){
			pos += ItemPrefab.transform.position;
		}

		GameObject item = GLOBAL.that.SuperInstantiate(ItemPrefab, pos, Quaternion.identity);

        item.transform.parent = transform;

            // If the spawned item is a collectable, give it a quantity
            if(item.GetComponent<CollectableBase>() != null) {
                item.GetComponent<CollectableBase>().setQuantity(ItemQuantity);
			}
	}
}

