using UnityEngine;
using System.Collections;

public class FireballScript : ProjectileBase {

	public GameObject FirePrefab;

	void Awake(){
		if(!GetComponent<PhotonView>().isMine){
			Destroy(this);
		}
	}

	void OnTriggerEnter(){
		if(GetComponent<PhotonView>().isMine)
		{
			GameObject fire = Instantiate(FirePrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
			fire.GetComponent<ProjectileBase>().wizard = this.wizard;
			GLOBAL.that.SuperDestroy(gameObject);
		}
	}
}
