using UnityEngine;
using System.Collections;

public class FireballScript : MonoBehaviour {

	public GameObject FirePrefab;

	void Awake(){
		PhotonView pView = GetComponent<PhotonView>();

		if(pView != null && pView.isMine){
			this.enabled = false;
		}
	}

	void OnTriggerEnter(){
		if(GetComponent<PhotonView>().isMine)
		{
			Instantiate(FirePrefab, gameObject.transform.position, Quaternion.identity);
			GLOBAL.that.SuperDestroy(gameObject);
		}
	}
}
