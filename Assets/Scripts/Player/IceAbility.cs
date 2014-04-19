using UnityEngine;
using System.Collections;

public class IceAbility : AbilityBase {

	public override void fire(){
		GameAudio.playFreezeSpell();

		Transform w = GLOBAL.myWizard.transform;

		GameObject ice = PhotonNetwork.Instantiate(
			"IcePrefab",
			w.position + w.forward.normalized, //+ new Vector3(0, 1, 0),
			gameObject.transform.rotation,
			0
		) as GameObject;

		ice.GetComponent<ProjectileBase>().wizard = GLOBAL.myWizard;
		ice.rigidbody.velocity = GLOBAL.MainCamera.transform.forward * 20;
		ice.transform.parent = w.parent;
	}

	public override string getAbilityName() {
		return "Ice Blast";
	}

	public override string getAbilityDescription() {
		return "Burrrrr...";
	}
}
