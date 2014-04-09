﻿using UnityEngine;
using System.Collections;

using InControl;

public class PlayerController : MonoBehaviour {

	public GameObject head, hat, brim, body, legL, legR, armL, armR;
	Color initShaderColor;

	float movementSpeed = 13000; // 10
	float strafeSpeed = 11000; // 8
    float jumpSpeed = 1500;

    public bool isInAir = true;

	public float fireRate = .11f;
	private float lastShot = -10;

	public float fireSpeed = 4;
	public GameObject fireProj;

	public Transform thisCamera;

	public static Transform playerSingleton;
	public float sinCounter = 0;

	private PlayerAbility playerAbility;

	private GameObject spawnPoint; 

	public ExitGames.Client.Photon.Hashtable networkedProperties;

	private int score = 0;
	private int Teamscore = 0;

	private GameObject hud;

	//public int health = 100;
	int hitTimer = 0;

    bool isStepL = true;
    bool isStepR = false;

	// Controls
	InputDevice idevice = InputManager.ActiveDevice;
	InputControl ctrl_Jump;
	InputControl ctrl_LeftStickX;
	InputControl ctrl_LeftStickY;
	InputControl ctrl_Select;
	InputControl ctrl_RightBumper;
	InputControl ctrl_RightJoystickButton, ctrl_O;

    delegate void VoidDelegate();
    VoidDelegate getInput, updatePlayer;

    Vector3 forceMovement;

	void Awake(){
		refreshControls();
		initShaderColor = body.renderer.materials[1].GetColor("_ReflectColor");

        getInput = control_active;
        updatePlayer = update_active;
	}

	void Start(){
		thisCamera = (GameObject.FindWithTag("MainCamera") as GameObject).transform;
		spawnPoint = (GameObject.Find("SpawnPoint") as GameObject);

		playerSingleton = this.transform;
		playerAbility = this.GetComponent<PlayerAbility>();

		networkedProperties = PhotonNetwork.player.customProperties;

		/*if( networkedProperties.ContainsKey("Ability") )
		{
			networkedProperties["Ability"] = AbilityManagerScript.currentAbility.getAbilityName();
			PhotonNetwork.player.SetCustomProperties(networkedProperties);
		}*/

		hud = GameObject.Find("HudCamera");

		if(hud == null){
			print("HudCamera Object not found for leaderboard");
		}
	}

    void Update(){
        //health = GLOBAL.health;

        // Input Controls
        refreshControls();
        getInput();

        if(updatePlayer != null) {
            updatePlayer();
        }

        if(ctrl_Select.WasPressed) {
            hud.gameObject.GetComponent<LeaderboardScript>().FlipGameState();
        }
    }

    void FixedUpdate(){
        float vY = (rigidbody.velocity.y > 0) ? rigidbody.velocity.y / 1.2f  : rigidbody.velocity.y;
        rigidbody.velocity = new Vector3(0, vY, 0);
        rigidbody.AddRelativeForce(forceMovement);
    }

    void refreshControls(){
		idevice = InputManager.ActiveDevice;

		ctrl_Jump = idevice.GetControl(InputControlType.Action1);
		ctrl_LeftStickX = idevice.GetControl(InputControlType.LeftStickX);
		ctrl_LeftStickY = idevice.GetControl(InputControlType.LeftStickY);
		ctrl_Select = idevice.GetControl(InputControlType.Select);
		ctrl_RightBumper = idevice.GetControl(InputControlType.RightBumper);
		ctrl_RightJoystickButton = idevice.GetControl(InputControlType.RightStickButton);
		ctrl_O = idevice.GetControl(InputControlType.Action2);
	}

	// control while the player is alive and kicking
	void control_active()
	{
		// fire
		if(ctrl_RightBumper.WasPressed){
			GetComponent<AbilityManagerScript>().attemptFire();
		}

		// punch
		if(ctrl_RightJoystickButton.WasPressed || ctrl_O.WasPressed){
			GetComponent<PunchAbility>().fire();
		}


		// movement
        float fx = 0, fy = 0, fz = 0;
		if(ctrl_LeftStickX.IsPressed){
			//float hor = ctrl_LeftStickX.LastValue * strafeSpeed * Time.deltaTime;
			//transform.Translate(hor, 0, 0);

            fx = ctrl_LeftStickX.LastValue * Time.deltaTime * strafeSpeed;
		}

        if(ctrl_Jump.WasPressed) {
            fy = attemptJump();
        }

		if(ctrl_LeftStickY.IsPressed){
			//float vert = ctrl_LeftStickY.LastValue * movementSpeed * Time.deltaTime;
			//transform.Translate(0, 0, vert);

            fz = ctrl_LeftStickY.LastValue * Time.deltaTime * movementSpeed;
		}

        forceMovement = new Vector3(fx, fy, fz);
	}

	// control while the player is down
	void control_down()
	{
		/*
		if(ctrl_Jump)
		{
			GLOBAL.health = 100;

            getInput = control_active;
            updatePlayer = update_active;

            Vector3 newForward = new Vector3(thisCamera.transform.forward.x, 0, thisCamera.transform.forward.z);
			transform.forward = newForward;
			rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
		}*/
	}

	// control while the player is dead
    //void control_dead()
    //{

    //}

	void update_active()
	{
		animate();

		// health
		if(--hitTimer < 0){									// reset color
			swapShader(initShaderColor);
		}
		else if((hitTimer <= 18 && hitTimer > 15) ||		// blink color (init)
		        (hitTimer <= 12 && hitTimer > 9) ||
		        (hitTimer <= 6 && hitTimer > 3)
		){		
			swapShader(initShaderColor);
		}
		else {
			swapShader(Color.red);							// blink color (red)
		}
		
		if(GLOBAL.health <= 0)
		{
			HudScript.addNewMessage("KO!", 120, Color.red);
			
            getInput = control_down;
            updatePlayer = null; // update_down
            
            rigidbody.constraints = RigidbodyConstraints.None;
			GLOBAL.health = 0;
			TakeDamage(-100, transform);
			
			score = 0;
		}
	}

    //void update_down()
    //{

    //}

    //void update_dead()
    //{

    //}


	void swapShader(Color c){
		head.renderer.materials[1].SetColor("_ReflectColor", c);
		hat.renderer.materials[1].SetColor("_ReflectColor", c);
		brim.renderer.materials[1].SetColor("_ReflectColor", c);
		body.renderer.materials[1].SetColor("_ReflectColor", c);
		legL.renderer.materials[1].SetColor("_ReflectColor", c);
		legR.renderer.materials[1].SetColor("_ReflectColor", c);
		armL.renderer.materials[1].SetColor("_ReflectColor", c);
		armR.renderer.materials[1].SetColor("_ReflectColor", c);
	}

    void animate() {
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

        if(dtAngle != 0) {
            leg.Rotate(new Vector3(dtAngle * Time.deltaTime, 0));


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
        }
    }

	float attemptJump(){
		if(!isInAir){
			GameAudio.playJump();
            return jumpSpeed;
		}

        return 0;
	}

	public void IncrementPoints( int numToAdd )
	{
		score += numToAdd;

		if( networkedProperties.ContainsKey("Score") )
		{
			networkedProperties["Score"] = score;
		}
		else
		{
			networkedProperties.Add("Score", score);
		}

		PhotonNetwork.player.SetCustomProperties( networkedProperties );

		//hud.GetComponent<HudScript>().ScoreText.GetComponent<TextMesh>().text = "Score: " + score.ToString();
	}
	
	public void TakeDamage(int damage, Transform t)
	{
		if(hitTimer < 0){
			hitTimer = 21;

			GameAudio.playPain();
			swapShader(Color.red);

			GLOBAL.health = Mathf.Clamp(GLOBAL.health - damage, 0 , 100 );
			networkedProperties["Health"] = GLOBAL.health;
			
			PhotonNetwork.player.SetCustomProperties(networkedProperties);
		}
	}

	public ExitGames.Client.Photon.Hashtable GetNetworkedProperties()
	{
		return networkedProperties;
	}
}
