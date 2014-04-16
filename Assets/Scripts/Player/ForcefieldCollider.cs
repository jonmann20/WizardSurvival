using UnityEngine;
using System.Collections;

public class ForcefieldCollider : MonoBehaviour {

	void Start(){
		Invoke("destory", 10);
	}

	void Update(){
		transform.position = Wizard.myWizard.transform.position;
	}

	void destory(){
		ForcefieldAbility.that.startCooldown();
		Destroy(gameObject);
	}
}
