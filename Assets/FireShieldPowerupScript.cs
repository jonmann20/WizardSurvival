using UnityEngine;
using System.Collections;

public class FireShieldPowerupScript : MonoBehaviour {

	float sinCounter = 0.0f;
	Vector3 initialPosition;
	void Start () {
		initialPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
	}

	void Update()
	{
		sinCounter += 0.025f;
		transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(sinCounter) * 0.01f, transform.position.z);
	}

	void OnCollisionEnter(Collision collision) {

		if(collision.gameObject.tag == "Player")
		{
			collision.gameObject.GetComponent<AbilityManagerScript>().changeAbility("FireShieldAbility");
			Destroy(gameObject);
		}
	}
}
