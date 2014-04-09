using UnityEngine;
using System.Collections;

public class Bouncy : MonoBehaviour {
	
	float scale = 0.0f;
	
	public float target = 3.5f;
	float stiffness = 1.0f;
	float velocity = 0.0f;
	float damping = 0.8f;
	
	void Start(){
		transform.localScale = new Vector3(scale, scale, scale);
	}
	
	void Update(){
		float force = (target - scale) * stiffness;
		velocity = damping * (velocity + force);
		scale += velocity;

		transform.localScale = new Vector3(scale, scale, scale);
	}

	public void reset(){
		scale = 0.0f;
		//target = 3.5f;
		stiffness = 1.0f;
		velocity = 0.0f;
		damping = 0.8f;
	}
}
