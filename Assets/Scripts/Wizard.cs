using UnityEngine;
using System.Collections;

public class Wizard : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKey(KeyCode.W)) {
            rigidbody.velocity += new Vector3(0, 0, 15 * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.S)) {
            rigidbody.velocity += new Vector3(0, 0, -15 * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.A)) {
            rigidbody.velocity += new Vector3(-15 * Time.deltaTime, 0, 0);
        }

        if(Input.GetKey(KeyCode.D)) {
            rigidbody.velocity += new Vector3(15 * Time.deltaTime, 0, 0);
        }
	}
}
