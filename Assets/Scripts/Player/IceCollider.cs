using UnityEngine;
using System.Collections;

public class IceCollider : MonoBehaviour {

	void Update(){
		Destroy(gameObject, 2);
	}



	void OnTriggerEnter(){
		Destroy(gameObject);
	}
}
