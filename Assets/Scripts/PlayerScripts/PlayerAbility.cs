using UnityEngine;
using System.Collections;

public class PlayerAbility : MonoBehaviour {

	public float chargeTimeNeeded = 2.0f;
	public float baseDamage = 3.0f;

	public int level = 0; 

	private bool charging;
	private float currentCharge = 0.0f;
	private float startChargeTime = 0;
	private bool canActivate = false;

	public float rechargeTime = 3.0f;

	public bool needsFullChargeToFire = true;

	public float speed = 5;

	public GameObject ProjectilePrefab;

	public Transform player; 

	void Start()
	{
		player = PlayerController.playerSingleton;
	}
	

	// Update is called once per frame
	void Update () {

		if( charging )
		{
			currentCharge = Time.time / ( startChargeTime + chargeTimeNeeded );

			if( currentCharge >= 1.0f )
			{
				canActivate = true;
			}
			else
			{
				canActivate = false;
			}
		}
		else
		{
			currentCharge = 0.0f;
			canActivate = false;
		}
	
	}

	public void stopCharge()
	{
		charging = false;
		canActivate = false;
	}
	public void startCharge()
	{
		charging = true;
		startChargeTime = Time.time;
	}

	public bool isCharging()
	{
		return charging;
	}

	public bool Charged()
	{
		return canActivate;
	}
	
}
