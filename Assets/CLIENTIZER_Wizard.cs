using UnityEngine;
using System.Collections;

public class CLIENTIZER_Wizard : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(GetComponent<PhotonView>().isMine)
		{
			Destroy(GetComponent<SyncScript>());
			GLOBAL.myWizard = gameObject;
		}
		else
		{
			Destroy(GetComponent<AbilityManagerScript>());
			Destroy(GetComponent<Rigidbody>());
			Destroy(GetComponent<PlayerController>());
			Destroy(GetComponent<PunchAbility>());
		}
	}
}
