using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {

	public GameObject target;
	//Vector3 offset;

	public float damping = 1;

	// The distance in the x-z plane to the target
	public float distance = 10.0f;
	// the height we want the camera to be above the target
	public float height = 5.0f;
	// How much we 
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;

	// Use this for initialization
	void Start () {
		//offset = target.transform.position - transform.position;
	}


	void LateUpdate() {

		if (!target)
			return;

		//applies damping to the rotation of camera
		float currentAngle = transform.eulerAngles.y;
		float desiredAngle = target.transform.eulerAngles.y;

		float wantedHeight = target.transform.position.y + height;
		float currentHeight = transform.position.y;

		float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * damping);
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);

		//Use this instead if you do not want damping
		/*float desiredAngle = target.transform.eulerAngles.y;
		Quaternion rotation = Quaternion.Euler(0, desiredAngle, 0);*/
		
		Quaternion rotation = Quaternion.Euler(0, angle, 0);

		transform.position = target.transform.position;
		transform.position -= rotation * Vector3.forward * distance;

		//FIXED Height
		/*Vector3 tempPos = transform.position;
		tempPos.y = currentHeight;
		transform.position = tempPos;*/

		if( transform.position.y < target.transform.position.y )
		{
			Vector3 tempPos = transform.position;
			tempPos.y = target.transform.position.y;
			transform.position = tempPos;
		}
		
		transform.LookAt(target.transform);
	}
}
