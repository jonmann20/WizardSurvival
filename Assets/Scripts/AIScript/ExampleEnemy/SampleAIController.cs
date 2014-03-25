using UnityEngine;
using System.Collections;

public class SampleAIController : MonoBehaviour {
	

	public float speed = 1.0f;

	public int health = 3;

	void Start()
	{
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
		if( coll.gameObject.tag == "Player" )
		{
			Remove();
		}
	}

	void OnTriggerEnter(Collider coll)
	{
		if( coll.gameObject.tag == "PlayerBullet" )
		{
			health--;
		}
	}

	public void Remove()
	{
		this.gameObject.GetComponent<MGAISuperClass>().Remove();
	}
}
