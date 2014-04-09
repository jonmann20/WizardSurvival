using UnityEngine;
using System.Collections;

public class Wizard : MonoBehaviour {

	public static GameObject myWizard;

	void Awake(){

        PhotonView pView = GetComponent<PhotonView>();

		if(pView != null && !pView.isMine)
		{
			PlayerController p = GetComponent<PlayerController>();
			Rigidbody r = GetComponent<Rigidbody>();
			AbilityManagerScript ams = GetComponent<AbilityManagerScript>();

			r.useGravity = false;
			p.enabled = false;
			ams.enabled = false;
		}
		else
		{
			myWizard = gameObject;
		}
	}
}
