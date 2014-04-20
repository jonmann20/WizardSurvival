using UnityEngine;
using System.Collections;

public class CLIENTIZER_Spawner : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		if(!GetComponent<PhotonView>().owner.isMasterClient)
		{
			Destroy(gameObject);
		}
	}
}