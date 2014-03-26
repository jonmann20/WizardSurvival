using UnityEngine;
using System.Collections;

public class SampleAIController : MonoBehaviour {
	

	public float speed = 1.0f;

	public int health = 3;

	void Start()
	{
		this.transform.rigidbody.freezeRotation = true;
	}

	void Update () {

		//this.transform.LookAt(player);

		if( health <= 0 )
		{
			Remove();
		}
	}

	void OnCollisionEnter(Collision coll)
	{
		if( coll.gameObject.tag == "PlayerBullet" )
		{
			health--;
		}
	}

	void OnCollisionStay(Collision coll)
	{
		if( coll.gameObject.tag == "Player" )
		{
			if(coll.gameObject.GetComponent<PhotonView>().isMine)
			{
				GLOBAL.health --;
//				print("health: " + GLOBAL.health);
			}
		}
	}

	void OnTriggerEnter(Collider coll)
	{
	
	}

	public void Remove()
	{
		this.gameObject.GetComponent<MGAISuperClass>().Remove();
	}
}
