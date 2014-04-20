using UnityEngine;
using System.Collections;

public class CLIENTIZER_InvisibleProjectile : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		if(GetComponent<PhotonView>().isMine)
		{
			GetComponent<SyncScript>().enabled = false;
		}
		else
		{
			Destroy(GetComponent<InvisibleProjectile>());
			Destroy(GetComponent<LimitedLife>());
			Destroy(GetComponent<SphereCollider>());
		}
	}
}