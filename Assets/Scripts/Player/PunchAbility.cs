using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PunchAbility : AbilityBase {

	const int MAX_LIFE = 10;
	int lifeCounter = 0;

	const float CIRCLE_RATE = 0.2f;
	const float CIRCLE_RADIUS = 3.0f;
	const float DISTANCE = 2.0f;
	const float FIST_SCALE = 0.8f;

	GameObject arm;
	GameObject fakeArm;
	Vector3 previousLocalPositionOnBody;

	Material[] punchMat;

	void Awake()
	{
		arm = transform.Find("ArmR").gameObject as GameObject;
		punchMat = arm.renderer.materials;
	}

	public override void fire()
	{
		if(fakeArm == null)
		{
			arm.renderer.enabled = false;
			fakeArm = PhotonNetwork.Instantiate("PunchCube", transform.position, Quaternion.identity, 0) as GameObject;
			fakeArm.GetComponent<ProjectileBase>().wizard = gameObject;
			fakeArm.GetComponent<MeshRenderer>().materials = punchMat;
			fakeArm.tag = "PlayerBullet";
		}

		GameAudio.playJump();
		lifeCounter = MAX_LIFE;
	}
	
	void Update()
	{
		if(lifeCounter > 0)
		{
			--lifeCounter;
		
			Vector3 pos;
			float val = 0.0f;
			if(lifeCounter > (float)(MAX_LIFE * 0.5f))
			{
				val = (((float)MAX_LIFE) - (float)lifeCounter) / ((float)MAX_LIFE * 0.5f);
				pos = transform.forward * val;

			}
			else
			{
				val = (float)lifeCounter / ((float)MAX_LIFE * 0.5f);
				pos = transform.forward * val;

			}
		
			pos *= DISTANCE;

			fakeArm.transform.position = transform.position + pos + transform.forward * 0.1f;
			fakeArm.transform.localScale = new Vector3(1, 1, 1) * val;
		}
		else if(lifeCounter <= 0 && fakeArm != null)
		{
			PhotonNetwork.Destroy(fakeArm);
			arm.renderer.enabled = true;
		}
	}
	
	public override string getAbilityName()
	{
		return "Melee";
	}

	public override string getAbilityDescription()
	{
		return "FALCAWNNNN! PAAAAWWWNCCHHHHH!";
	}
}
