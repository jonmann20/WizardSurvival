using UnityEngine;
using System.Collections;

public class MarchingEnemyScript : MonoBehaviour {

	float sinCounter = 0.0f;
	float fraction = 0.0f;
	// Use this for initialization
	void Start () {
		sinCounter += Random.Range(0.0f, 3.14f);
		fraction = Random.Range(0.5f, 1.5f);
		transform.localScale = new Vector3(fraction, fraction, fraction);
	}
	
	// Update is called once per frame
	void Update () {
		sinCounter += 0.2f;
		float y = Mathf.Abs(Mathf.Sin(sinCounter)) + 1;
		transform.position = new Vector3(transform.position.x, y, transform.position.z + (3.0f - fraction) * 0.05f);
	}
}
