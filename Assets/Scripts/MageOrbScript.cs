using UnityEngine;
using System.Collections;

using RAIN.Core;

public class MageOrbScript : MonoBehaviour {

	public int damageToApply = 15;

	void OnTriggerEnter(){
		if(GetComponent<PhotonView>().isMine)
		{
			GLOBAL.that.SuperDestroy(gameObject);
		}
	}
}
