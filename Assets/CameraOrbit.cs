using UnityEngine;
using System.Collections;

public class CameraOrbit : MonoBehaviour {
	
	float sinCounter1 = 0;
	public float radius = 100;
	public float height = 0;
	public GameObject target;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		sinCounter1 += 0.01f;
		Vector3 newPos = new Vector3(Mathf.Sin(sinCounter1) * radius, height, Mathf.Cos(sinCounter1 + Mathf.PI) * radius);
		transform.position = target.transform.position + newPos;
	}
}
