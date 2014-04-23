﻿using UnityEngine;
using System.Collections;
using RAIN.Core;
using RAIN.Action;
using System.Linq;

public class SampleAIController : MonoBehaviour {
	
	public float health = 90f;
	public float speed = 7.0f;

	private int damageToApply = 15;

	private float deathTimer = 3.0f;
	public float timeUntilRemove = 3.0f;

	const int MAX_INVINCIBILITY_TIMER = 5;
	int invincibilityTimer = 0;

	public Material initialMaterial;
	public Material redMaterial;

	private bool speedReduced = false;
	public float speedReduction = 0.6f;
	public float speedRecutionTimer = 4.0f;
	private float iceTimer = 0.0f;
	
	Transform skeleton;
	//private bool speedIsSet = false;
	public int scoreValue = 10;
	public Shader toonShader;

	void Start(){
		float ratio = Random.Range(1.0f, 2.5f);

		skeleton = transform.Find("skeleton");

		if(skeleton == null){
			skeleton = transform.Find("Ice Golem");
			damageToApply = (int)((float)damageToApply * (ratio + (1f)));
			health *= ratio;
			health *= 3.5f;
			speed *= speed * (ratio);
			speed *= .8f;

			speed = Mathf.Clamp(speed,4,7);
		}
		else{
			float sc = Random.Range(3.8f, 4.5f);

			transform.localScale = new Vector3(sc, sc, sc);
			damageToApply = (int)(damageToApply * ratio);
			health *= ratio;
			speed *= (ratio);
			speed = Mathf.Clamp(speed,5,9);
		}
		
		this.transform.rigidbody.freezeRotation = true;
		
		initialMaterial = skeleton.renderer.material;

		redMaterial = new Material(Shader.Find("Toon/Basic"));
		redMaterial.color = new Color(1, 0, 0, 0.7f);

		//for balance
		this.transform.parent.FindChild("AI").GetComponent<AIRig>().AI.Motor.DefaultSpeed = speed;
		this.transform.parent.FindChild("AI").GetComponent<AIRig>().AI.WorkingMemory.SetItem("damageToApply", damageToApply);

		//sync health and speed

		PhotonView view = PhotonView.Find(this.transform.parent.GetComponent<PhotonView>().viewID);
		//health
		float[] healthParam = new float[1];
		healthParam[0] = health;
		view.RPC("setHealthRPC",PhotonTargets.All, healthParam);
		//speed
		float[] speedParam = new float[1];
		speedParam[0] = speed;
		//view.RPC("setSpeedRPC",PhotonTargets.All, speedParam);


		//health = transform.parent.transform.GetComponent<Health>().health;
	}

	void Update(){
		if(health <= 0){
			gameObject.layer = LayerMask.NameToLayer("Dead Enemy");
			deathTimer -= Time.deltaTime;

			if(deathTimer <= 0){
				Remove();
			}
		}
		else {
			deathTimer = timeUntilRemove;
		}

		//for iceblast speed reduction
		if( iceTimer > 0 )
		{
			iceTimer -= Time.deltaTime;
			if( speedReduced )
			{
				this.transform.parent.FindChild("AI").GetComponent<AIRig>().AI.Motor.DefaultSpeed = speed * speedReduction;
				float[] speedParam = new float[1];
				speedParam[0] = speed * speedReduction;
				PhotonView view = PhotonView.Find(this.transform.parent.GetComponent<PhotonView>().viewID);
				//view.RPC("setSpeedRPC",PhotonTargets.All, speedParam);
				speedReduced = false;
			}

			//print ( this.transform.parent.FindChild("AI").GetComponent<AIRig>().AI.Motor.DefaultSpeed );
		}
		else
		{
			this.transform.parent.FindChild("AI").GetComponent<AIRig>().AI.Motor.DefaultSpeed = speed;
			float[] speedParam = new float[1];
			speedParam[0] = speed;
			PhotonView view = PhotonView.Find(this.transform.parent.GetComponent<PhotonView>().viewID);
			//view.RPC("setSpeedRPC",PhotonTargets.All, speedParam);
			speedReduced = false;
		}
	}

	void FixedUpdate(){
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
		if(health > 0 && invincibilityTimer <= 0 && col.gameObject.tag == "PlayerBullet"){
			if(col.gameObject.GetComponent<IceballScript>() != null){
				if(iceTimer <= 0){
					speedReduced = true;
				}
				iceTimer = speedRecutionTimer;

				TakeDamage(20);
				float[] healthParam = new float[1];
				healthParam[0] = health;
				PhotonView view = PhotonView.Find(this.transform.parent.GetComponent<PhotonView>().viewID);
				view.RPC("setHealthRPC",PhotonTargets.All, healthParam);

			}
			else{
				TakeDamage(33);
				float[] healthParam = new float[1];
				healthParam[0] = health;
				PhotonView view = PhotonView.Find(this.transform.parent.GetComponent<PhotonView>().viewID);
				view.RPC("setHealthRPC",PhotonTargets.All, healthParam);
			}
			invincibilityTimer = MAX_INVINCIBILITY_TIMER;

			if(health <= 0 && gameObject.layer == LayerMask.NameToLayer("Enemy")){
				//if(col.gameObject.GetComponent<ProjectileBase>().wizard.GetComponent<PhotonView>().isMine){
					GLOBAL.myWizard.gameObject.GetComponent<PlayerController>().IncrementPoints(this.scoreValue);
				//}
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
		if(col.collider.gameObject.tag == "Player"){
			if(col.collider.gameObject.transform.parent.GetComponent<PhotonView>().isMine){
				col.collider.gameObject.transform.parent.GetComponent<PlayerController>().TakeDamage(20, transform);
			}
		}
	}

	public void Remove(){
		this.gameObject.GetComponent<MGAISuperClass>().Remove();
	}

	public void SetHealth(float inHealth)
	{
		health = Mathf.Clamp(inHealth, 0, 5000);
	}
	
	public void TakeDamage(float damage)
	{
		health = Mathf.Clamp(health-damage, 0, 5000);
	}
}
