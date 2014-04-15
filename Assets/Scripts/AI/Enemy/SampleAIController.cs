using UnityEngine;
using System.Collections;
using RAIN.Core;
using RAIN.Action;

public class SampleAIController : MonoBehaviour {
	
	public float health = 100f;
	public float speed = 7.0f;

	private int damageToApply = 15;

	private float deathTimer = 3.0f;
	public float timeUntilRemove = 3.0f;

	const int MAX_INVINCIBILITY_TIMER = 5;
	public int invincibilityTimer = 0;

	public Material initialMaterial;
	public Material redMaterial;
	MeshRenderer renderer;
	
	Transform skeleton;
	private bool speedIsSet = false;
	public int scoreValue = 10;
	public Shader toonShader;

	void Start(){
		float ratio = Random.Range(0.8f, 2.0f);

		skeleton = transform.Find("skeleton");

		if(skeleton == null){
			skeleton = transform.Find("Ice Golem");
			damageToApply = (int)((float)damageToApply * (ratio + (1f)));
			health *= ratio;
			health *= 10;
			speed *= speed * (ratio);
			speed *= .8f;
		}
		else
		{
			transform.localScale *= (ratio + .5f);
			damageToApply = (int) (damageToApply * ratio) ;
			health *= ratio;
			speed *= speed * (ratio);
		}
		
		this.transform.rigidbody.freezeRotation = true;
		
		initialMaterial = skeleton.renderer.material;

		redMaterial = new Material(Shader.Find("Toon/Basic"));
		redMaterial.color = Color.red;
		
		this.transform.parent.FindChild("AI").GetComponent<AIRig>().AI.Motor.DefaultSpeed = speed;
		this.transform.parent.FindChild("AI").GetComponent<AIRig>().AI.WorkingMemory.SetItem("damageToApply", damageToApply);
		//health = transform.parent.transform.GetComponent<Health>().health;
	}

	void Update(){
	
		if(health <= 0){
			//transform.GetComponent<BoxCollider>().enabled = false;
			gameObject.layer = LayerMask.NameToLayer("Dead Enemy");
			deathTimer -= Time.deltaTime;

			if(deathTimer <= 0){
				Remove();
			}
		}
		else {
			deathTimer = timeUntilRemove;
		}

		// invincible
		if(invincibilityTimer > 0){
			--invincibilityTimer;
			skeleton.renderer.material = redMaterial;
            skeleton.renderer.materials[1].SetColor("_Color", Color.red);
		}
		else {
			skeleton.renderer.material = initialMaterial;
            skeleton.renderer.materials[1].SetColor("_Color", new Color(0.63529411764f, 0.63529411764f, 0.63529411764f));
		}
	}

	void doDamage(Collider col){
		if(invincibilityTimer <= 0 && col.gameObject.tag == "PlayerBullet"){
			health = Mathf.Clamp(health-25,0,health);
			invincibilityTimer = MAX_INVINCIBILITY_TIMER;
			
			if(health <= 0 && gameObject.layer == LayerMask.NameToLayer("Enemy")){
				if(col.gameObject.GetPhotonView().isMine){
					Wizard.myWizard.gameObject.GetComponent<PlayerController>().IncrementPoints(this.scoreValue);
				}
			}
		}
	}

	void OnTriggerEnter(Collider col){
		doDamage(col);
	}

	void OnTriggerStay(Collider col){
		doDamage(col);
	}

	void OnCollisionStay(Collision col){
		/*if(col.collider.gameObject.tag == "Player"){
			if(col.collider.gameObject.transform.parent.GetComponent<PhotonView>().isMine){
				col.collider.gameObject.transform.parent.GetComponent<PlayerController>().TakeDamage(20, transform);
			}
		}*/
	}

	public void Remove(){
		this.gameObject.GetComponent<MGAISuperClass>().Remove();
	}
}
