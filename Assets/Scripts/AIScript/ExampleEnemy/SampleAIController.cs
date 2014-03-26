using UnityEngine;
using System.Collections;

public class SampleAIController : MonoBehaviour {
	

	public float speed = 1.0f;

	private float health;

	void Start()
	{
		this.transform.rigidbody.freezeRotation = true;
		health = transform.parent.transform.GetComponent<Health>().health;
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
			this.transform.parent.transform.GetComponent<Health>().Damage(25f);
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
