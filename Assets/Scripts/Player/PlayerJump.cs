using UnityEngine;
using System.Collections;

public class PlayerJump : MonoBehaviour {

    public GameObject wizard;
    PlayerController pController;

    void Awake(){
        pController = wizard.GetComponent<PlayerController>();
    }

    void OnTriggerEnter(Collider col){
		ground(col.gameObject.tag);
    }

    void OnTriggerStay(Collider col){
		ground(col.gameObject.tag);
    }

    void OnTriggerExit(Collider coll){
		if(coll.gameObject.tag == "Ground")
		{
			pController.plusY = Physics.gravity.y * 1.4f;
        	pController.isInAir = true;
		}
    }


	void ground(string t){
		if(t == "Ground"){
			if(pController.velMovement.y < 0){		// fixes sliding without input bug
				pController.velMovement.y = 0;
			}
			pController.plusY = 0;
			pController.isInAir = false;
		}
	}
}
