using UnityEngine;
using System.Collections;

public class MouseCamera : MonoBehaviour {

	public GameObject target;
	public GameObject playerPrefab;
	public float distance = 10.0f;
	public float maxDistance = 13.0f;
	public float minDistance = 8.0f;
	public float zoomSpeed = 1;

	public float xSpeed = 250.0f;
	public float ySpeed = 120.0f;
	
	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;

	public float rotationThreshold = 20;

	private Vector3 lastStationaryPosition;
	public float followUpdateSpeed = 100;
	public float distanceUpdateSpeed = 30;
	public float targetDistance = 6;

	private float x = 0.0f;
	private float y = 0.0f;

	public GUITexture reticle;

	void Start(){
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
		
		// Make the rigid body not change rotation
		if (rigidbody)
			rigidbody.freezeRotation = true;


		//reticle
		reticle.pixelInset = new Rect (0 - (Screen.width /80), 0 - (Screen.width /80), Screen.width / 40, Screen.width / 40);
		reticle.enabled = true;

		//target = (GameObject) Instantiate(playerPrefab, new Vector3(0,2,0), Quaternion.identity) as GameObject;
	}

	void Update()
	{
		distance = (float) Mathf.Clamp (
			distance + Input.GetAxis ("Mouse ScrollWheel") * -zoomSpeed * Time.deltaTime,
			minDistance,
			maxDistance
		);
	}

    void OnCollisionEnter(Collision col){
        print("col");
    }


	void LateUpdate () {
		if (target){
			bool b0 = mouseIn();
			bool b1 = rightStickIn();

			if(!b0 && !b1){
				holdPos();
			}
		}
	}

	void holdPos(){
		Vector3 position = (transform.rotation * Vector3.forward * -distance) + target.transform.position;
		position.y += .5f;
		
		//transform.rotation = rotation;
		transform.position = position;
	}

	bool rightStickIn(){
		float rH = Input.GetAxisRaw("RightH");
		float rV = Input.GetAxisRaw("RightV");
		//print (rH);

		if(rH != 0 || rV != 0){
			float horizontal = Input.GetAxis("RightH") * xSpeed * Time.deltaTime;
			x += horizontal;
			y -= Input.GetAxis("RightV") * ySpeed * Time.deltaTime;
			
			target.transform.Rotate(0, horizontal, 0);
			
			y = ClampAngle(y, yMinLimit, yMaxLimit);
			
			Quaternion rotation = Quaternion.Euler(y, x, 0);
			Vector3 position = (rotation * Vector3.forward * -distance) + target.transform.position;
			position.y += .5f;
			
			transform.rotation = rotation;
			transform.position = position;

			return true;
		}

		return false;
	}

	bool mouseIn(){
		if(Input.GetMouseButton(1)){
			float horizontal = Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
			x += horizontal;
			y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
			
			target.transform.Rotate(0, horizontal, 0);
			
			y = ClampAngle(y, yMinLimit, yMaxLimit);
			
			Quaternion rotation = Quaternion.Euler(y, x, 0);
			Vector3 position = (rotation * Vector3.forward * -distance) + target.transform.position;
			position.y += .5f;
			
			transform.rotation = rotation;
			transform.position = position;

			return true;
		}

		return false;
	}

	static float ClampAngle (float angle, float min, float max) {
		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp (angle, min, max);
	}
}
