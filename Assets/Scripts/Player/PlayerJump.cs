using UnityEngine;
using System.Collections;

public class PlayerJump : MonoBehaviour {

    public GameObject wizard;
    PlayerController pController;

    void Awake(){
        pController = wizard.GetComponent<PlayerController>();
    }

    void OnTriggerEnter(){
        pController.isInAir = false;
    }

    void OnTriggerStay(){
        pController.isInAir = false;
    }

    void OnTriggerExit(){
        pController.isInAir = true;
    }
}
