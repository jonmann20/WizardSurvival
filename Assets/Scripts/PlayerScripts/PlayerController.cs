using UnityEngine;
using System.Collections;

using InControl;

public class PlayerController : MonoBehaviour {

	public GameObject head, hat, body, legL, legR, armL, armR;
	Color initShaderColor;

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
	public float sinCounter = 0;

	private PlayerAbility playerAbility;

	private GameObject spawnPoint; 

	public ExitGames.Client.Photon.Hashtable networkedProperties;

	private int score = 0;
	private int Teamscore = 0;

	private GameObject hud;

	int hitTimer = 0;

	void Awake(){
		initShaderColor = body.renderer.materials[1].GetColor("_ReflectColor");
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
			//rigidbody.velocity = new Vector3(rigidbody.velocity.x + hor, rigidbody.velocity.y, rigidbody.velocity.z);
			//rigidbody.MovePosition(transform.position + new Vector3(hor, 0, 0));
		}

        if(ctrl_LeftStickY.IsPressed) {
            float vert = ctrl_LeftStickY.LastValue * movementSpeed * Time.deltaTime;
            transform.Translate(0, 0, vert);
			//rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, rigidbody.velocity.z + vert);
			//rigidbody.MovePosition(transform.position + new Vector3(0, 0, vert));
        }

        // jump
        if(ctrl_Jump.WasPressed){
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

		if( GLOBAL.health <= 0 )
		{
			GLOBAL.health = 0;
			TakeDamage(-100);

			score = 0;

			transform.position = spawnPoint.transform.position;
			/*GameObject wiz = PhotonNetwork.Instantiate("Wizard", new Vector3(0, 5, 0), Quaternion.identity, 0) as GameObject;
			GameObject mainCam = GameObject.FindWithTag("MainCamera") as GameObject;
			(mainCam.GetComponent<MouseCamera>() as MouseCamera).target = wiz;
			
			
			// keep Hierachy clean
			wiz.transform.parent = GameObject.Find("_WizardHolder").transform;
			PhotonNetwork.Destroy(gameObject);*/
		}
	}

	void swapShader(Color c){
		head.renderer.materials[1].SetColor("_ReflectColor", c);
		hat.renderer.materials[1].SetColor("_ReflectColor", c);
		body.renderer.materials[1].SetColor("_ReflectColor", c);
		legL.renderer.materials[1].SetColor("_ReflectColor", c);
		legR.renderer.materials[1].SetColor("_ReflectColor", c);
		armL.renderer.materials[1].SetColor("_ReflectColor", c);
		armR.renderer.materials[1].SetColor("_ReflectColor", c);
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
			// TODO: causing IN_AIR state
            //leg.Rotate(new Vector3(dtAngle * Time.deltaTime, 0));
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
			GameAudio.playJump();
			rigidbody.AddForce(0, 635, 0);
		}
	}
	
	void OnCollisionEnter(Collision col){
		if(col.gameObject.tag == "Ground"){
			// TODO: fix coming off ground bug
			//GameAudio.playJumpland();
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
	
	public void TakeDamage(int damage)
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
