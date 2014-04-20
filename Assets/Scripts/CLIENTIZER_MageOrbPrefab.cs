using UnityEngine;
using System.Collections;

public class CLIENTIZER_MageOrbPrefab : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		if(GetComponent<PhotonView>().isMine)
		{
			GetComponent<SyncScript>().enabled = false;
		}
		else
		{
			Destroy(GetComponent<MageOrbScript>());
			Destroy(GetComponent<LimitedLife>());
		}
	}
}