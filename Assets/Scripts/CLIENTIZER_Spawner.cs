using UnityEngine;
using System.Collections;

public class CLIENTIZER_Spawner : MonoBehaviour {

	void Start(){
		PhotonView pv = GetComponent<PhotonView>();

		if(pv && pv.owner.isMasterClient){
			Destroy(gameObject);
		}
	}
}