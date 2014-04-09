using UnityEngine;
using System.Collections;

public class InventoryItemScript : MonoBehaviour {

	string itemName = "...";

	float scale = 0.0f;

	public float target = 4.0f;
	float stiffness = 1.0f;
	float velocity = 0.0f;
	float damping = 0.8f;

	void Start () {
		gameObject.layer = 9;
		transform.localScale = new Vector3(scale, scale, scale);
	}

	void Update () {
		float force = (target - scale) * stiffness;
		velocity = damping * (velocity + force);
		scale += velocity;

		transform.localScale = new Vector3(scale, scale, scale);
	}

	void setName(string n)
	{
		itemName = n;
	}
}
