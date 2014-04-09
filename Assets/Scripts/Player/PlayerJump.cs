using UnityEngine;
using System.Collections;

public class PlayerJump : MonoBehaviour {

    public GameObject wizard;
    PlayerController pController;

    void Awake(){
        pController = wizard.GetComponent<PlayerController>();
    }

    void OnTriggerEnter(Collider col){
        //if(col.gameObject.tag == "Ground"){
            pController.isInAir = false;
        //}
    }

    void OnTriggerExit(Collider col){
        //if(col.gameObject.tag == "Ground"){
            pController.isInAir = true;
        //}
    }
}
