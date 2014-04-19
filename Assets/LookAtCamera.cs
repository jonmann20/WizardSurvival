using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

	GameObject MainCamera;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(MainCamera == null)
			MainCamera = GameObject.FindWithTag("MainCamera");
		else
			transform.rotation = Quaternion.LookRotation(MainCamera.transform.forward);
	}
}
