using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float movementSpeed = 10;
	public float strafeSpeed = 8;

	public float fireRate = .11f;
	private float lastShot = -10;

	public float fireSpeed = 4;
	public GameObject fireProj;

	public Transform thisCamera;

	public static Transform playerSingleton;

    public GameObject legL, legR;

	private PlayerAbility playerAbility;


	void Start () {
	
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
            rigidbody.AddForce(0, 405, 0);
        }

        animate();

		// ability
		if(Input.GetButton("Fire1")){




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

		if( playerAbility )
		{
			if( playerAbility.Charged() )
			{
				if( Input.GetButtonUp("Fire1") )
				{
					GameObject clone = Instantiate(
						playerAbility.ProjectilePrefab, 
						gameObject.transform.position + (new Vector3(0, 0 ,0)) + (gameObject.transform.forward.normalized * 1), 
						gameObject.transform.rotation
						) as GameObject;
					clone.rigidbody.velocity = thisCamera.transform.forward * playerAbility.speed * Time.deltaTime;
					
					playerAbility.stopCharge();
					print ("fired");
				}
				print ("charged");
			}
			else if( playerAbility.isCharging() )
			{
				if( Input.GetButtonUp("Fire1") )
				{
					if (playerAbility.needsFullChargeToFire)
					{
						playerAbility.stopCharge();
					}
				}
				print("Charging");
			}
			else if( Input.GetButton("Fire1") )
			{
				if( !playerAbility.isCharging() )
				{
					playerAbility.startCharge();
				}
			}
		}
	}

    bool isStepL = true;
    bool isStepR = false;
    void animate() {
        animateLeg(legL.transform, ref isStepL);
        animateLeg(legR.transform, ref isStepR);
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
}
