using UnityEngine;
using System.Collections;

public class Wizard : MonoBehaviour {

	void Awake()
	{
		if(!GetComponent<PhotonView>().isMine)
		{
			
			PlayerController p = GetComponent<PlayerController>();
			Rigidbody r = GetComponent<Rigidbody>();
			r.useGravity = false;
			p.enabled = false;
		}
	}
}
