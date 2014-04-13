using UnityEngine;
using System.Collections;

public class MageOrbScript : MonoBehaviour {

	public int life = 180;

	void Awake(){
	}

	void Update()
	{
		life --;
		if(life < 30)
		{
			transform.localScale = new Vector3(1, 1, 1) * ((float)life / 30.0f);
		}
		if(life <= 0)
			GLOBAL.that.SuperDestroy(gameObject);
	}

	void OnTriggerEnter(){
		if(GetComponent<PhotonView>().isMine)
		{
			GLOBAL.that.SuperDestroy(gameObject);
		}
	}
}
