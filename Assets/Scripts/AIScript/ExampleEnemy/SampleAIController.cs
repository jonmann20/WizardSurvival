using UnityEngine;
using System.Collections;

public class SampleAIController : MonoBehaviour {
	
	public float health = 100f;
	public float speed = 2.0f;
	const int MAX_INVINCIBILITY_TIMER = 5;
	public int invincibilityTimer = 0;

	public Material initialMaterial;
	public Material redMaterial;
	MeshRenderer renderer;
	
	Transform skeleton;

	void Start()
	{
		skeleton = transform.Find("skeleton");
		this.transform.rigidbody.freezeRotation = true;

		initialMaterial = skeleton.renderer.material;
		redMaterial = new Material(Shader.Find("Diffuse"));
		redMaterial.color = Color.red;
		//health = transform.parent.transform.GetComponent<Health>().health;
	}

	void Update () {

		if( health <= 0 )
		{
			Remove();
		}

		//INVINCIBLE
		if(invincibilityTimer > 0)
		{
			invincibilityTimer --;
			skeleton.renderer.material = redMaterial; 
		}
		else
			skeleton.renderer.material = initialMaterial;
	}

	void OnCollisionEnter(Collision coll)
	{
		if( invincibilityTimer <= 0 && coll.gameObject.tag == "PlayerBullet")
		{
			health = Mathf.Clamp(health-25,0,health);
			invincibilityTimer = MAX_INVINCIBILITY_TIMER;
		}
	}

	void OnCollisionStay(Collision coll)
	{
		if( coll.gameObject.tag == "Player" )
		{
			if(coll.gameObject.GetComponent<PhotonView>().isMine)
			{
				GLOBAL.health --;
			}
		}
	}

	void OnTriggerEnter(Collider coll)
	{
	
	}

	public void Remove()
	{
		this.gameObject.GetComponent<MGAISuperClass>().Remove();
	}
}
