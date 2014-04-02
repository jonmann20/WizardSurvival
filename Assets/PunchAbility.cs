using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PunchAbility : AbilityBase {

	const int MAX_LIFE = 10;
	int lifeCounter = 0;

	const float CIRCLE_RATE = 0.2f;
	const float CIRCLE_RADIUS = 3.0f;
	const float DISTANCE = 1.0f;
	const float FIST_SCALE = 0.8f;

	GameObject arm;
	Vector3 previousLocalPositionOnBody;
	Vector3 previousScaleOnBody;

	void Awake()
	{
		arm = transform.Find("ArmR").gameObject as GameObject;
		previousLocalPositionOnBody = arm.transform.localPosition;
		previousScaleOnBody = arm.transform.localScale;
	}

	public override void fire()
	{
		GameAudio.playWind();
		lifeCounter = MAX_LIFE;
	}
	
	void Update()
	{

		if(lifeCounter > 0)
		{
			arm.tag = "PlayerBullet";
			lifeCounter --;
		
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

			arm.transform.position = transform.position + pos + transform.forward * 0.1f;
			arm.transform.localScale = previousScaleOnBody + new Vector3(val * FIST_SCALE, val * FIST_SCALE, val * FIST_SCALE);
		}
		else
		{
			arm.tag = "Player";
			arm.transform.localPosition = previousLocalPositionOnBody;
			arm.transform.localScale = previousScaleOnBody;
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
