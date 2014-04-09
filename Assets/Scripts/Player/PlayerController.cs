using UnityEngine;
using System.Collections;

using InControl;

public class PlayerController : MonoBehaviour {

	public GameObject head, hat, brim, body, legL, legR, armL, armR;
	Color initShaderColor;

	float movementSpeed = 500;
	float strafeSpeed = 400;
    float jumpSpeed = 700;

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

	bool invincible = false;
	float hitAnimRate = 0;
	float hitAnimTimer = 0.5f;

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

    Vector3 velMovement;

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
	}

    void Update(){
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
        rigidbody.velocity = transform.TransformDirection(velMovement * Time.fixedDeltaTime);
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
	void control_active(){
		// fire
		if(ctrl_RightBumper.WasPressed){
			GetComponent<AbilityManagerScript>().attemptFire();
		}

		// punch
		if(ctrl_RightJoystickButton.WasPressed || ctrl_O.WasPressed){
			GetComponent<PunchAbility>().fire();
		}

		// movement
		if(ctrl_LeftStickX.IsPressed){
            velMovement.x = ctrl_LeftStickX.LastValue * strafeSpeed;
		}
        else {
            velMovement.x = 0;
        }

        velMovement.y += Physics.gravity.y * 1.4f;

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

	void update_active(){
		animate();

		// health
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
	
	public void TakeDamage(int damage, Transform t){
		if(!invincible){
			invincible = true;
			StartCoroutine("animDamage");

			GameAudio.playPain();
			swapShader(Color.red);
			
			GLOBAL.health = Mathf.Clamp(GLOBAL.health - damage, 0, 100);
			networkedProperties["Health"] = GLOBAL.health;
			
			PhotonNetwork.player.SetCustomProperties(networkedProperties);
		}
	}

	IEnumerator animDamage(){
		const int time = 2;
		float elapsedTime = 0;

		float d = 0.18f;

		while(elapsedTime < time){
			bool b = false;

			for(float i=d; i < time; i += d*2){
				//print (i + ", " + (i+d) + " ?? " + elapsedTime);

				if(elapsedTime > i && elapsedTime < (i+d)){
					b = true;
					break;
				}
			}

			if(b){
				swapShader(initShaderColor);	// blink color (init)
			}
			else {
				swapShader(Color.red);			// blink color (red)
			}

			elapsedTime += Time.deltaTime;
			yield return null;
			
			if(elapsedTime >= time){
				invincible = false;
				swapShader(initShaderColor);	// reset color
			}
		}


	}

	public ExitGames.Client.Photon.Hashtable GetNetworkedProperties(){
		return networkedProperties;
	}


	void OnCollisionEnter(Collision coll)
	{
		if( coll.collider.gameObject.tag == "EnemyBullet")
		{
			TakeDamage(20, coll.collider.transform);
		}
	}
}
