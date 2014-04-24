using UnityEngine;
using System.Collections;

public class IceCollider : MonoBehaviour {

	void Awake(){
		if(!GetComponent<PhotonView>().isMine){
			Destroy(this);
		}
	}
	
	void OnTriggerEnter(){
		if(GetComponent<PhotonView>().isMine)
		{
			GLOBAL.that.SuperDestroy(gameObject);
		}
	}
}


