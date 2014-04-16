using UnityEngine;
using System.Collections;

public class IceCollider : MonoBehaviour {

	void Update(){
		Invoke("destory", 2);
	}

	void OnTriggerEnter(){
		GLOBAL.that.SuperDestroy(gameObject);
	}

	void destroy(){
		GLOBAL.that.SuperDestroy(gameObject);
	}
}
