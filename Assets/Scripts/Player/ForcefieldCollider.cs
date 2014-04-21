using UnityEngine;
using System.Collections;

public class ForcefieldCollider : MonoBehaviour {

	void Start(){
		Invoke("destory", 15);  // keep number in sync with ForcefieldAbility
	}

	void Update(){

		if(GetComponent<PhotonView>().isMine)
			transform.position = GLOBAL.myWizard.transform.position;
		
		// TODO: network this
		renderer.material.color = new Color(0, Mathf.PingPong(Time.time/2, 0.14f), 0, 0.18f);
	}

	void destory(){
		ForcefieldAbility.that.startCooldown();
		GLOBAL.that.SuperDestroy(gameObject);
	}
}
