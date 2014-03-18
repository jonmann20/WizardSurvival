using UnityEngine;
using System.Collections;

public class SampleAIController : MonoBehaviour {

	//player to move towards
	public Transform player;

	public float speed = 1.0f;

	public int health = 3;

	void Start()
	{
		if( player == null )
		{
			print ("WTF");
		}
		player = PlayerController.playerSingleton;
	}

	void Update () {

		this.transform.LookAt(player);

		transform.position = Vector3.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);

		//check health

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
