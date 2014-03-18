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
	// Use this for initialization
	void Start () {
	
		playerSingleton = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
	
		//Movement
		float horizontal = Input.GetAxis("Horizontal") * strafeSpeed * Time.deltaTime;
		transform.Translate(horizontal, 0, 0);
		
		float vertical = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
		transform.Translate(0, 0, vertical);

		//Ability
		if(Input.GetMouseButton(0)){
			if(Time.time > fireRate+lastShot){
				GameObject clone = (GameObject) Instantiate(fireProj, gameObject.transform.position + (new Vector3(0,gameObject.transform.renderer.bounds.size.y/2 ,0)) + (gameObject.transform.forward.normalized * 1), gameObject.transform.rotation) as GameObject;
				//fireProj.tag = "Bullet";
				clone.rigidbody.velocity = thisCamera.transform.forward * fireSpeed * Time.deltaTime;
				
				lastShot = Time.time;
			}
		}
	}
}
