using UnityEngine;
using System.Collections;

public class FireballScript : MonoBehaviour {

	public GameObject FirePrefab;

	void Awake(){
		if(!GetComponent<PhotonView>().isMine){
			Destroy(this);
		}
	}

	void OnTriggerEnter(){
			Instantiate(FirePrefab, gameObject.transform.position, Quaternion.identity);
			GLOBAL.that.SuperDestroy(gameObject);
	}
}
