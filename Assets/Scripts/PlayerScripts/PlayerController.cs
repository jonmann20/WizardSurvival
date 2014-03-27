using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float movementSpeed = 10;
	public float strafeSpeed = 8;

	public enum JumpState { IN_AIR, NOT_IN_AIR };
	public JumpState currentJumpState = JumpState.IN_AIR;

	public float fireRate = .11f;
	private float lastShot = -10;

	public float fireSpeed = 4;
	public GameObject fireProj;

	public Transform thisCamera;

	public static Transform playerSingleton;

    public GameObject legL, legR;
	public float sinCounter = 0;

	private PlayerAbility playerAbility;
	
	void Start () {

		thisCamera = (GameObject.FindWithTag("MainCamera") as GameObject).transform;

		playerSingleton = this.transform;
		playerAbility = this.GetComponent<PlayerAbility>();
	}
	
	void Update () {
	
		// movement
		float horizontal = Input.GetAxis("Horizontal") * strafeSpeed * Time.deltaTime;
		transform.Translate(horizontal, 0, 0);
		
		float vertical = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
		transform.Translate(0, 0, vertical);

        // jump
        if(Input.GetButtonDown("Jump")){
			attemptJump();
        }

        animate();

		// ability
		if(Input.GetButton("Fire1")){

			//print("FIRE!");


			//Old code
			/*
			if(Time.time > fireRate+lastShot){
				GameObject clone = Instantiate(
                    fireProj, 
                    gameObject.transform.position + (new Vector3(0, 0 ,0)) + (gameObject.transform.forward.normalized * 1), 
                    gameObject.transform.rotation
                ) as GameObject;
				
                //fireProj.tag = "Bullet";
				clone.rigidbody.velocity = thisCamera.transform.forward * fireSpeed * Time.deltaTime;
				
				lastShot = Time.time;
			}*/
		}
	}

    bool isStepL = true;
    bool isStepR = false;
    void animate() {
		if(rigidbody.velocity.magnitude > 0.001f){
        	animateLeg(legL.transform, ref isStepL);
        	animateLeg(legR.transform, ref isStepR);
		}
    }

    void animateLeg(Transform leg, ref bool isStep) {
        float dtAngle = 0;
        float normalizedAngle = leg.localEulerAngles.x;

        if(normalizedAngle > 300) {
            normalizedAngle -= 360;
        }

       // print(legL.transform.localEulerAngles.x + ", " + normalizedAngle);

        if(isStep){
            if(normalizedAngle < -20) {
                isStep = false;
            }
            else {
                dtAngle = -42f;
            }
        }
        else {
            if(normalizedAngle > 20) {
                isStep = true;
            }
            else {
                dtAngle = 42f;
            }
        }
        //print(dtAngle);

        if(dtAngle != 0) {
            leg.Rotate(new Vector3(dtAngle * Time.deltaTime, 0));
        }
    }

//	void animateLeg2(Transform legTran, bool isRightLeg)
//	{
//		Vector3 vel2d = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
//		float velocityMagnitude = vel2d.magnitude;
//		if(velocityMagnitude < 1)
//			velocityMagnitude = 0;
//		//print(velocityMagnitude);
//		sinCounter += velocityMagnitude * 10 * Time.deltaTime;
//		//print(sinCounter);
//		if(isRightLeg)
//			legTran.localRotation = Quaternion.Euler(Mathf.Sin(sinCounter) * 45, 0, 0);
//		//else
//			//legTran.localRotation = Quaternion.Euler(Mathf.Cos(sinCounter) * 45, 0, 0);		                                  
//	}

	void attemptJump(){
		if(currentJumpState == JumpState.NOT_IN_AIR){
			rigidbody.AddForce(0, 405, 0);
		}
	}
	
	void OnCollisionEnter(Collision col){
		if(col.gameObject.tag == "Ground"){
			currentJumpState = JumpState.NOT_IN_AIR;
		}
	}
	void OnCollisionStay(Collision col){
		if(col.gameObject.tag == "Ground"){
			currentJumpState = JumpState.NOT_IN_AIR;
		}
	}
	void OnCollisionExit(Collision col){
		currentJumpState = JumpState.IN_AIR;
	}
}
