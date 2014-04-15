using UnityEngine;
using System.Collections;

public class Jumpy : MonoBehaviour {

	float initialHeight;
	void Start () {
		initialHeight = transform.position.y;
	}

	void Update () {
		if(transform.position.y <= initialHeight)
		{
			rigidbody.velocity = new Vector3(0, 5, 0);
		}
	}
}
