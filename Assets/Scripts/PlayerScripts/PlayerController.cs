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


	void Start () {
	
		playerSingleton = this.transform;
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
			if(Time.time > fireRate+lastShot){
				GameObject clone = Instantiate(
                    fireProj, 
                    gameObject.transform.position + (new Vector3(0, 0/*gameObject.transform.renderer.bounds.size.y/2*/ ,0)) + (gameObject.transform.forward.normalized * 1), 
                    gameObject.transform.rotation
                ) as GameObject;
				
                //fireProj.tag = "Bullet";
				clone.rigidbody.velocity = thisCamera.transform.forward * fireSpeed * Time.deltaTime;
				
				lastShot = Time.time;
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
