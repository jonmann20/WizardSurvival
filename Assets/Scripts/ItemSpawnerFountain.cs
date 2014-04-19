using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSpawnerFountain : MonoBehaviour {

	public List<GameObject> ItemPrefabs = new List<GameObject>();
	private int ItemQuantity;
	
	bool attemptedSpawn = false;

	void Update()
	{
		if(!attemptedSpawn && GLOBAL.objectiveMet == false)
		{
			attemptedSpawn = true;
			if(PhotonNetwork.isMasterClient) {
				int i = 0;
				foreach (GameObject ItemPrefab in ItemPrefabs) {
					spawn(i);
					++i;
				}
			}
			else
				GLOBAL.that.SuperDestroy(gameObject);
		}
	}
	
	void spawn(int index){
		Vector3 fountainPos = new Vector3 (174.8422f, -17.6110f, 84.3543f);
		Vector3 pos;
		do {
			pos = Random.insideUnitSphere * 12;
		} while ((pos.x < 8.5f && pos.x > -8.5f) || (pos.z < 8.5f && pos.z > -8.5f));
		
		pos.y = 0;
		pos += fountainPos;

		GameObject item = GLOBAL.that.SuperInstantiate(ItemPrefabs[index], pos, Quaternion.identity);
		
		item.transform.parent = transform;


		// If the spawned item is a collectable, give it a quantity
		if(item.GetComponent<CollectableBase>() != null) {
			ItemQuantity = Random.Range(1, 3);
			item.GetComponent<CollectableBase>().setQuantity(ItemQuantity);
		}
	}
}

