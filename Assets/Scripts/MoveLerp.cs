using UnityEngine;
using System.Collections;

public class MoveLerp : MonoBehaviour {
	
	public const float RATE_OF_CHANGE = 0.05f;
	public float radius = 0.25f;
	float fraction = 0.0f;
	Vector3 desiredPos;
	Vector3 currentPos;
	
	// Use this for initialization
	void Start () {
		currentPos = Random.onUnitSphere * radius;
		desiredPos = Random.onUnitSphere * radius;
	}
	
	// Update is called once per frame
	void Update () {
		fraction += RATE_OF_CHANGE;
		transform.localPosition = Vector3.Lerp(currentPos, desiredPos, fraction);
		
		if(fraction >= 1.0)
			findNewPos();
	}
	
	void findNewPos()
	{
		currentPos = transform.localPosition;
		desiredPos = Random.onUnitSphere * radius;
		fraction = 0;
	}
}
