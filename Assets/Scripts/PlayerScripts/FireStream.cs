using UnityEngine;
using System.Collections;

public class FireStream : MonoBehaviour {

	public float speed = 10;

	public float range = 10;
	public GameObject explosionPrefab;

	public int life = 90;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per framesa
	void Update () {
		life --;
		float fraction = (float)life / 30.0f;

		if(fraction < 1.0f)
		{
			transform.localScale = new Vector3(fraction, fraction, fraction);
		}

		if(life <= 0)
			PhotonNetwork.Destroy(gameObject);
	}
	void OnCollisionEnter(Collision coll) {

		/*if( coll.gameObject.tag == "Projectile" )
			return;
		// Rotate the object so that the y-axis faces along the normal of the surface
		ContactPoint contact = coll.contacts[0];
		Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
		Vector3 pos = contact.point;
		GameObject clone = (GameObject) Instantiate(explosionPrefab, pos, rot) as GameObject;
		// Destroy the projectile
		clone.rigidbody.velocity = new Vector3(Random.Range(0,10),Random.Range(0,10), Random.Range(0,10)).normalized * speed;
		clone = (GameObject) Instantiate(explosionPrefab, pos, rot) as GameObject;
		// Destroy the projectile
		clone.rigidbody.velocity = new Vector3(Random.Range(0,10),Random.Range(0,10), Random.Range(0,10)).normalized * speed;
		clone = (GameObject) Instantiate(explosionPrefab, pos, rot) as GameObject;
		// Destroy the projectile
		clone.rigidbody.velocity = new Vector3(Random.Range(0,10),Random.Range(0,10), Random.Range(0,10)).normalized * speed;
		Destroy (gameObject);*/
	}
}
