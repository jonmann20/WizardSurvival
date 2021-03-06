﻿using UnityEngine;
using System.Collections;

using InControl;

public class MouseCamera : MonoBehaviour {

	public GameObject target;
	public GameObject playerPrefab;
	
	float distance = 2.63f;
	float horizontalDistance = -1.65f;
	float verticalDistance = 1.29f;

	public float maxDistance = 13.0f;
	public float minDistance = 8.0f;
	public float zoomSpeed = 1;

	float xSpeed = 135;
	float ySpeed = 120.0f;
	
	float yMinLimit = -23f;
	float yMaxLimit = 33.5f;

	public float rotationThreshold = 20;

	private Vector3 lastStationaryPosition;
	public float followUpdateSpeed = 100;
	public float distanceUpdateSpeed = 30;
	public float targetDistance = 6;

	public GUITexture reticle;

	float x = 0.0f;
	float y = 0.0f;

	void Start(){
		Screen.lockCursor = true;

        if(this.target != null) {
            Vector3 position = (this.target.transform.rotation * Vector3.forward * -distance) + target.transform.position + (Vector3.left * horizontalDistance) + (Vector3.up * verticalDistance);
            transform.position = position;
        }
		
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;
		
		// Make the rigid body not change rotation
        if(rigidbody){
            rigidbody.freezeRotation = true;
        }

		// reticle
		reticle.pixelInset = new Rect (0 - (Screen.width /80), 0 - (Screen.width /80), Screen.width / 40, Screen.width / 40);
		reticle.enabled = true;

		//target = (GameObject) Instantiate(playerPrefab, new Vector3(0,2,0), Quaternion.identity) as GameObject;
	}

	void Update(){
        InputDevice device = InputManager.ActiveDevice;
        
		distance = (float) Mathf.Clamp (
            distance + device.GetControl(InputControlType.ScrollWheel) * -zoomSpeed * Time.deltaTime,
			minDistance,
			maxDistance
		);
	}

	void LateUpdate () {
		if (target){
            bool inputRightStick = rightStickIn();

            if(!inputRightStick) {
				holdPos();
			}
		}
	}

	void holdPos(){
		if(GLOBAL.health > 0){
			transform.position = (target.transform.rotation * Vector3.forward * -distance) + target.transform.position + (target.transform.rotation * Vector3.left * horizontalDistance) + (target.transform.rotation * Vector3.up * verticalDistance);
		}
		else{
			transform.position = (transform.rotation * Vector3.forward * (-distance * 10)) + target.transform.position;
		}
	}

	bool rightStickIn(){
        InputDevice device = InputManager.ActiveDevice;
        InputControl ctrl_RightStickX = device.GetControl(InputControlType.RightStickX);
        InputControl ctrl_RightStickY = device.GetControl(InputControlType.RightStickY);

		if(ctrl_RightStickX.IsPressed || ctrl_RightStickY.IsPressed){
			float horizontal = ctrl_RightStickX.LastValue * xSpeed * Time.deltaTime;
			x += horizontal;
			y -= ctrl_RightStickY.LastValue * ySpeed * Time.deltaTime;
			
			target.transform.Rotate(0, horizontal, 0);
			
			y = ClampAngle(y, yMinLimit, yMaxLimit);
			
			Quaternion rotation = Quaternion.Euler(y, x, 0);
			//Vector3 position = (rotation * Vector3.forward * -distance) + target.transform.position + (rotation * Vector3.left * -0.5f) + ( rotation * Vector3.up * 0.2f);

			Vector3 position = new Vector3(0,0,0);

			if(GLOBAL.health > 0){
				position = (target.transform.rotation * Vector3.forward * -distance) + target.transform.position + (target.transform.rotation * Vector3.left * horizontalDistance) + (target.transform.rotation * Vector3.up * verticalDistance);
			}
			else{
				position = (rotation * Vector3.forward * (-distance * 10)) + target.transform.position;

			}
			
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
