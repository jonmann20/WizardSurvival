using UnityEngine;
using System.Collections;

public class BouncyTitle : MonoBehaviour {

	float scale = 0;
	float target = 1;
	float stiffness = 1.0f;
	float velocity = 0.0f;
	float damping = 0.8f;

	public bool isActive = false;

	void FixedUpdate(){
		if(isActive){
			float force = (target - scale) * stiffness;
			velocity = damping * (velocity + force);
			scale += velocity;

			transform.localScale = new Vector3(scale, scale, scale);
		}
	}
}
