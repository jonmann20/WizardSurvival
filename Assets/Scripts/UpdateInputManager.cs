using UnityEngine;
using System.Collections;

using InControl;

public class UpdateInputManager : MonoBehaviour {
	void Start(){
        InputManager.Setup();
        InputManager.AttachDevice(new UnityInputDevice(new DefaultProfile()));
	}
	
	void Update(){

        // TODO: use XInput (lower latency for windows)
        // TODO: use a fixed update, with lower latency (see README.pdf in InControl folder)

        InputManager.Update();
	}

}
