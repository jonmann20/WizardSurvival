using UnityEngine;
using System.Collections;

public class CLIENTIZER_IcePrefab : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		if(GetComponent<PhotonView>().isMine)
		{
			GetComponent<SyncScript>().enabled = false;
		}
		else
		{
			Destroy(GetComponent<IceCollider>());
			Destroy(GetComponent<IceballScript>());
			Destroy(GetComponent<Rigidbody>());
			Destroy(GetComponent<SphereCollider>());
		}
	}
}
