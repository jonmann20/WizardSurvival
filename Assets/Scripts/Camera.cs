using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

    public GameObject wizard;
    Vector3 offset;

	void Start(){
        offset = transform.position - wizard.transform.position;
        print(offset);
	}
	
	void Update(){
        transform.position = wizard.transform.position +  offset;
	}
}
