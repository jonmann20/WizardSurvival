using UnityEngine;
using System.Collections;

public class CLIENTIZER_FireballPrefab : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		if(GetComponent<PhotonView>().isMine)
		{
			GetComponent<SyncScript>().enabled = false;
		}
		else
		{
			Destroy(GetComponent<FireballAbility>());
			Destroy(GetComponent<FireballScript>());
			Destroy(GetComponent<LimitedLife>());
			Destroy(GetComponent<Rigidbody>());
			Destroy(GetComponent<SphereCollider>());
		}
	}
}