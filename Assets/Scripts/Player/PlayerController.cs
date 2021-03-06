﻿using UnityEngine;
using System.Collections;

using InControl;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	//    0,   1,    2,   3,    4,    5,     6
	// head, hat, body, legL, legR, armL, armR;
	public GameObject[] parts;

	public bool isInAir = true;

	public float fireSpeed = 4;
	public GameObject fireProj;

	public static Transform playerSingleton;
	public float sinCounter = 0;

    public ExitGames.Client.Photon.Hashtable networkedProperties;

    float movementSpeed = 500;
    float strafeSpeed = 400;
    float jumpSpeed = 720;

    //float fireRate = .11f;
    //float lastShot = -10;

    public float plusY = 0; // for jumping
	public Vector3 velMovement;

    PunchAbility punch;
    AbilityManagerScript ams;
	//PlayerAbility playerAbility;
	//GameObject spawnPoint; 

	int score = 0;
	//int Teamscore = 0;

	Leaderboard leaderboard;

	bool invincible = false;
	//float hitAnimRate = 0;
	//float hitAnimTimer = 0.5f;

    bool isStepL = true;
    bool isStepR = false;

	// Controls
	InputDevice idevice = InputManager.ActiveDevice;
	InputControl ctrl_Jump, ctrl_LeftStickX, ctrl_LeftStickY, ctrl_Select, ctrl_Start, ctrl_RightBumper, ctrl_RightJoystickButton, ctrl_O;

    delegate void VoidDelegate();
    VoidDelegate getInput, updatePlayer;
    Color initShaderColor;


	//RESPAWN
	//private GameObject respawnArea;
	int previousWave;

	void Awake(){
		refreshControls();
		initShaderColor = parts[0].renderer.materials[1].GetColor("_ReflectColor");

        getInput = control_active;
        updatePlayer = update_active;

        ams = GetComponent<AbilityManagerScript>();
        punch = GetComponent<PunchAbility>();
	}

	void Start(){
		plusY = Physics.gravity.y * 1.4f;

		playerSingleton = this.transform;

		networkedProperties = PhotonNetwork.player.customProperties;

        leaderboard = GLOBAL.MainCamera.GetComponent<Leaderboard>();

		//respawnArea = GameObject.Find("Respawn Area");
	}

    void Update(){
        refreshControls();

        if(getInput != null){
            getInput();
        }

        if(updatePlayer != null){
            updatePlayer();
        }

        if(ctrl_Select.WasPressed || ctrl_Start.WasPressed){
            leaderboard.FlipGameState();
        }
    }

    void FixedUpdate(){
		velMovement.y += plusY;

		if(updatePlayer == update_active)
		{
			Vector3 newForward = new Vector3(GLOBAL.MainCamera.transform.forward.x, 0, GLOBAL.MainCamera.transform.forward.z);
			transform.forward = newForward;
        	rigidbody.velocity = transform.TransformDirection(velMovement * Time.fixedDeltaTime);
		}
		else
		{
			velMovement = Vector3.zero;
		}
    }

    void refreshControls(){
		idevice = InputManager.ActiveDevice;

		ctrl_Jump = idevice.GetControl(InputControlType.Action1);
		ctrl_LeftStickX = idevice.GetControl(InputControlType.LeftStickX);
		ctrl_LeftStickY = idevice.GetControl(InputControlType.LeftStickY);
		ctrl_Select = idevice.GetControl(InputControlType.Select);
		ctrl_Start = idevice.GetControl(InputControlType.Start);
		ctrl_RightBumper = idevice.GetControl(InputControlType.RightBumper);
		ctrl_RightJoystickButton = idevice.GetControl(InputControlType.RightStickButton);
		ctrl_O = idevice.GetControl(InputControlType.Action2);
	}

	// control while the player is alive and kicking
	void control_active(){

		// fire
		bool doFire = false;
		if(ams.isAbilityName("Ice Blast")){
			doFire = ctrl_RightBumper.IsPressed;

			if(!doFire) {
				//GameAudio.stopFreezeSpell();
			}

		}
		else {
			doFire = ctrl_RightBumper.WasPressed;
		}

		if(doFire){
			ams.attemptFire();
		}

		// punch
		if(ctrl_RightJoystickButton.WasPressed || ctrl_O.WasPressed){
			punch.fire();
		}

		// movement
		if(ctrl_LeftStickX.IsPressed){
            velMovement.x = ctrl_LeftStickX.LastValue * strafeSpeed;
		}
        else {
            velMovement.x = 0;
        }

        if(ctrl_Jump.WasPressed){
            if(!isInAir){
                GameAudio.playJump();
                velMovement.y = jumpSpeed;
            }
        }
        else {
            if(rigidbody.velocity.y > 0){
                velMovement.y /= 1.12f;
            }
        }

		if(ctrl_LeftStickY.IsPressed){
            velMovement.z = ctrl_LeftStickY.LastValue * movementSpeed;
		}
        else {
            velMovement.z = 0;
        }
	}

	// control while the player is down
    //void control_down()
    //{
    //    if(ctrl_Jump)
    //    {
    //        GLOBAL.health = 100;

    //        getInput = control_active;
    //        updatePlayer = update_active;

    //        Vector3 newForward = new Vector3(thisCamera.transform.forward.x, 0, thisCamera.transform.forward.z);
    //        transform.forward = newForward;
    //        rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    //    }
    //}

	void update_active(){
		animate();

		// health
		if(!GLOBAL.isDead && GLOBAL.health <= 0){
			GLOBAL.isDead = true;
			HudScript.addNewMessage("KO!", 120, Color.red);

            rigidbody.constraints = RigidbodyConstraints.None;
			rigidbody.mass = 1;
			rigidbody.velocity = new Vector3(0, 10, 0);
			rigidbody.angularVelocity = Random.onUnitSphere * 10;

			print("UNLOCKED RIGIDBODY");

			getInput = null;
			updatePlayer = update_down;
		}
	}

	void update_down()
	{

	}

	public void Respawn()
	{
		getInput = control_active;
		updatePlayer = update_active;

		TakeDamage(-100, transform);

		//Vector3 resLoc = respawnArea.transform.position;
		//transform.position = resLoc;

		Vector3 newForward = new Vector3(GLOBAL.MainCamera.transform.forward.x, 0, GLOBAL.MainCamera.transform.forward.z);
		transform.forward = newForward;
		rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
		rigidbody.angularVelocity = Vector3.zero;
		rigidbody.freezeRotation = true;
		velMovement = Vector3.zero;

		print("LOCKED RIGIDBODY");
		GLOBAL.isDead = false;
	}

	#region Animation

	void swapShader(Color c){
		foreach(GameObject p in parts) {
			p.renderer.materials[1].SetColor("_ReflectColor", c);
		}
	}

    void animate(){
        Vector2 v = new Vector2(rigidbody.velocity.x, rigidbody.velocity.z);

		if(v.magnitude > 0.3f){
        	animateLeg(parts[3].transform, ref isStepL);	// legL
        	animateLeg(parts[4].transform, ref isStepR);	// legR

            animateArm(parts[5].transform, isStepL);	// armL
            animateArm(parts[6].transform, isStepR);	// armR
		}
    }

    void animateLeg(Transform leg, ref bool isStep) {
        float dtAngle = 0;
        float normalizedAngle = leg.localEulerAngles.x;

        if(normalizedAngle > 300) {
            normalizedAngle -= 360;
        }

        if(isStep) {
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

	IEnumerator animDamage() {
		const int time = 2;
		float elapsedTime = 0;

		float d = 0.18f;

		while(elapsedTime < time) {
			bool b = false;

			for(float i=d; i < time; i += d*2) {
				//print (i + ", " + (i+d) + " ?? " + elapsedTime);

				if(elapsedTime > i && elapsedTime < (i+d)) {
					b = true;
					break;
				}
			}

			if(b) {
				swapShader(initShaderColor);	// blink color (init)
			}
			else {
				swapShader(Color.red);			// blink color (red)
			}

			elapsedTime += Time.deltaTime;
			yield return null;

			if(elapsedTime >= time) {
				invincible = false;
				swapShader(initShaderColor);	// reset color
			}
		}


	}

	#endregion Animation

	public int getHealth()
	{
		return (int) PhotonNetwork.player.customProperties["Health"];
	}

	public void IncrementPoints(int numToAdd){
		if( this.gameObject.GetComponent<PhotonView>().isMine )
		{
			score += numToAdd;

			if(networkedProperties.ContainsKey("Score")){
				networkedProperties["Score"] = score;
			}
			else {
				networkedProperties.Add("Score", score);
			}

			PhotonNetwork.player.SetCustomProperties(networkedProperties);
		}
	}

	public void TakeDamage(int damage, Transform t){
		if( this.gameObject.GetComponent<PhotonView>().isMine )
		{
			if((!invincible || damage < 0) && !GLOBAL.gameOver  ){
				invincible = true;
				StartCoroutine(animDamage());

				GameAudio.playPain();
				swapShader(Color.red);
				
				GLOBAL.health = Mathf.Clamp(GLOBAL.health - damage, 0, 100);
				if(GLOBAL.health > 100)
					GLOBAL.health = 100;

				if( networkedProperties.ContainsKey("Health") )
					networkedProperties["Health"] = GLOBAL.health;
				else
				{
					networkedProperties.Add("Health", GLOBAL.health);
				}
				
				PhotonNetwork.player.SetCustomProperties(networkedProperties);
			}
		}
	}

	public ExitGames.Client.Photon.Hashtable GetNetworkedProperties(){
		return networkedProperties;
	}

	void OnTriggerEnter(Collider coll){
		if(coll.gameObject.tag == "EnemyBullet"){
			
			if(this.gameObject.GetPhotonView().isMine){
				MageOrbScript m = coll.gameObject.GetComponent<MageOrbScript>();
				if(m) {
					TakeDamage(m.damageToApply, coll.collider.transform);
				}
				
				GLOBAL.that.SuperDestroy(coll.gameObject);
			}
		}
	}
}
