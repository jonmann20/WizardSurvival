using UnityEngine;
using System.Collections;

public class SampleAIController : MonoBehaviour {
	
	public float health = 100f;
	public float speed = 5.0f;

	private float deathTimer = 3.0f;
	public float timeUntilRemove = 3.0f;

	const int MAX_INVINCIBILITY_TIMER = 5;
	public int invincibilityTimer = 0;

	public Material initialMaterial;
	public Material redMaterial;
	MeshRenderer renderer;
	
	Transform skeleton;

	public int scoreValue = 10;

	public Shader toonShader;

	void Start()
	{
		float ratio = Random.Range(0.8f, 2.0f);
		health *= ratio;
		transform.localScale *= ratio;
		speed *= 1 / ratio;

		skeleton = transform.Find("skeleton");
		this.transform.rigidbody.freezeRotation = true;

		initialMaterial = skeleton.renderer.material;
		redMaterial = new Material(Shader.Find("Toon/Basic Outline"));
		redMaterial.color = Color.red;
		//health = transform.parent.transform.GetComponent<Health>().health;
	}

	void Update () {

		if( health <= 0  )
		{
			//transform.GetComponent<BoxCollider>().enabled = false;
			gameObject.layer = LayerMask.NameToLayer("Dead Enemy");
			deathTimer -= Time.deltaTime;
			if( deathTimer <= 0 )
			{
				Remove();
			}
		}
		else
		{
			deathTimer = timeUntilRemove;
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
		if( invincibilityTimer <= 0 && coll.collider.gameObject.tag == "PlayerBullet")
		{
			health = Mathf.Clamp(health-25,0,health);
			invincibilityTimer = MAX_INVINCIBILITY_TIMER;

			if( health <= 0 && gameObject.layer == LayerMask.NameToLayer("Enemy") )
			{
				if( coll.gameObject.GetPhotonView().isMine )
				{
					Wizard.myWizard.gameObject.GetComponent<PlayerController>().IncrementPoints(this.scoreValue);
				}
			}
		}
	}

	void OnCollisionStay(Collision coll)
	{

		if(coll.collider.gameObject.tag == "Player")
		{
			if(coll.collider.gameObject.transform.parent.GetComponent<PhotonView>().isMine)
			{
				coll.collider.gameObject.transform.parent.GetComponent<PlayerController>().TakeDamage(20, transform);

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
