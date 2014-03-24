using UnityEngine;
using System.Collections;

public class Wizard : MonoBehaviour {

	private Vector3 latestCorrectPos;
	private Vector3 onUpdatePos;
	private float fraction;

	void Awake()
	{
		print("WIZARD INITIALIZED");
		//Don't control wizards belonging to other  players
		if(!GetComponent<PhotonView>().isMine)
		{
			
			PlayerController p = GetComponent<PlayerController>();
			Rigidbody r = GetComponent<Rigidbody>();
			r.useGravity = false;
			p.enabled = false;
		}

		latestCorrectPos = transform.position;
		onUpdatePos = transform.position;
	}

	//WARNING: Start() does not get called upon PhotonNetwork.Instantiate(). Use awake instead.
	void Start () {
	}
}
