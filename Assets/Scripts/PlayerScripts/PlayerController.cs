using UnityEngine;
using System.Collections;

using InControl;

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

    public GameObject legL, legR, armL, armR;
	public float sinCounter = 0;

	private PlayerAbility playerAbility;

	private int score = 0;

	private GameObject hud;
	
	void Start(){
		thisCamera = (GameObject.FindWithTag("MainCamera") as GameObject).transform;

		playerSingleton = this.transform;
		playerAbility = this.GetComponent<PlayerAbility>();

		hud = GameObject.Find("HudCamera");

		if( hud == null )
		{
			print ("HudCamera Object not found for leaderboard");
		}
	}
	
	void Update () {
        InputDevice device = InputManager.ActiveDevice;
        InputControl ctrl_Jump = device.GetControl(InputControlType.Action1);
        InputControl ctrl_LeftStickX = device.GetControl(InputControlType.LeftStickX);
        InputControl ctrl_LeftStickY = device.GetControl(InputControlType.LeftStickY);
        InputControl ctrl_Select = device.GetControl(InputControlType.Select);

        // movement
        if(ctrl_LeftStickX.IsPressed) {
            float hor = ctrl_LeftStickX.LastValue * strafeSpeed * Time.deltaTime;
            transform.Translate(hor, 0, 0);
        }

        if(ctrl_LeftStickY.IsPressed) {
            float vert = ctrl_LeftStickY.LastValue * movementSpeed * Time.deltaTime;
            transform.Translate(0, 0, vert);
        }

        // jump
        if(ctrl_Jump.IsPressed) {
            attemptJump();
        }

        // ability (see AbilityManagerScript.cs)
        //if(ctrl_RightTrigger.IsPressed) {
        //    print("shoot");
        //}

		// leaderboard
        if(ctrl_Select.IsPressed) {
			hud.gameObject.GetComponent<LeaderboardScript>().FlipGameState();
		}

        animate();
	}

    bool isStepL = true;
    bool isStepR = false;
    void animate() {
		//print (rigidbody.velocity.magnitude);

		if(rigidbody.velocity.magnitude > 0.001f){
        	animateLeg(legL.transform, ref isStepL);
        	animateLeg(legR.transform, ref isStepR);

            animateArm(armL.transform, isStepL);
            animateArm(armR.transform, isStepR);
		}
    }

    void animateArm(Transform arm, bool isStep) {
        float dtAngle = 0;
        float normalizedAngle = arm.localEulerAngles.x;

        if(normalizedAngle > 300) {
            normalizedAngle -= 360;
        }

        if(isStep) {
            dtAngle = -42f;
        }
        else {
            dtAngle = 42f;
        }

        arm.Rotate(new Vector3(dtAngle * Time.deltaTime, 0));
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
			rigidbody.AddForce(0, 205, 0); //405
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

	public void IncrementPoints( int numToAdd )
	{
		score += numToAdd;

        // TODO: bring back matt's text functionality
		hud.GetComponent<HudScript>().ScoreText.GetComponent<TextMesh>().text = "Score: " + score.ToString();

		if( PhotonNetwork.player.customProperties.ContainsKey("Score") )
		{
			PhotonNetwork.player.customProperties["Score"] = score;
		}
		else
		{
			PhotonNetwork.player.customProperties.Add("Score", score);
		}

		
	}
	
}
