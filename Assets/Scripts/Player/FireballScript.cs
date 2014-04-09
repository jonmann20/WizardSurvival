using UnityEngine;
using System.Collections;

public class FireballScript : MonoBehaviour {

	void Awake(){
		PhotonView pView = GetComponent<PhotonView>();

		if(pView != null && pView.isMine){
			this.enabled = false;
		}
	}

	void OnTriggerEnter(){
		PhotonNetwork.Instantiate("Fire", gameObject.transform.position, Quaternion.identity, 0);
		PhotonNetwork.Destroy(gameObject);
	}
}
