using UnityEngine;
using System.Collections;

public class IceCollider : MonoBehaviour {

	void OnTriggerEnter(){
		//GameAudio.playFreezeSpellCollision();
		GLOBAL.that.SuperDestroy(gameObject);
	}
}
