using UnityEngine;
using System.Collections;

public class Wizard : MonoBehaviour {

	void Awake()
	{
		print("WIZARD INITIALIZED");
		//Don't control wizards belonging to other  players
		print(GetComponent<PhotonView>().isMine);
		if(!GetComponent<PhotonView>().isMine)
		{
			
			PlayerController p = GetComponent<PlayerController>();
			p.enabled = false;
		}
	}

	//WARNING: Start() does not get called upon PhotonNetwork.Instantiate(). Use awake instead.
	void Start () {
	}

	void Update () {
        if(Input.GetKey(KeyCode.W)) {
            rigidbody.velocity += new Vector3(0, 0, 15 * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.S)) {
            rigidbody.velocity += new Vector3(0, 0, -15 * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.A)) {
            rigidbody.velocity += new Vector3(-15 * Time.deltaTime, 0, 0);
        }

        if(Input.GetKey(KeyCode.D)) {
            rigidbody.velocity += new Vector3(15 * Time.deltaTime, 0, 0);
        }
	}
}
