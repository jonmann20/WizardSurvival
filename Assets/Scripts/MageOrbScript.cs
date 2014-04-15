using UnityEngine;
using System.Collections;

public class MageOrbScript : MonoBehaviour {

	void OnTriggerEnter(){
		if(GetComponent<PhotonView>().isMine)
		{
			GLOBAL.that.SuperDestroy(gameObject);
		}
	}
}
