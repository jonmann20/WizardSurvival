using UnityEngine;
using System.Collections;

public class PlayerJump : MonoBehaviour {

    public GameObject wizard;
    PlayerController pController;

    void Awake(){
        pController = wizard.GetComponent<PlayerController>();
    }

    void OnTriggerEnter(Collider coll){
		if(coll.gameObject.tag == "Ground")
		{
			pController.plusY = 0;
        	pController.isInAir = false;
		}
    }

    void OnTriggerStay(Collider coll){
		if(coll.gameObject.tag == "Ground")
		{
			pController.plusY = 0;
        	pController.isInAir = false;
		}
    }

    void OnTriggerExit(Collider coll){
		if(coll.gameObject.tag == "Ground")
		{
			pController.plusY = Physics.gravity.y * 1.4f;
        	pController.isInAir = true;
		}
    }
}
