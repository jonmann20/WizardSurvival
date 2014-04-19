using UnityEngine;
using System.Collections;

public class CLIENTIZER_Wizard : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(GetComponent<PhotonView>().isMine)
		{
			GetComponent<SyncScript>().enabled = false;
			GLOBAL.myWizard = gameObject;
			//GLOBAL.myWizard.transform.FindChild("NameText").gameObject.SetActive(false);	// trying to turn off name for just yourself
			transform.FindChild("NameText").gameObject.SetActive(false);	// trying to turn off name for just yourself
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
