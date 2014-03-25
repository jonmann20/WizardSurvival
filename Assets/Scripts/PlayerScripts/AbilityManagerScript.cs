using UnityEngine;
using System.Collections;

public class AbilityManagerScript : MonoBehaviour {

	public AbilityBase currentAbility;

	// Use this for initialization
	void Start () {
		changeAbility("FireballAbility");
	}
	
	// Update is called once per frame
	void Update () {
		if( currentAbility )
		{
			if( currentAbility.isCharged() )
			{
				if( Input.GetButtonUp("Fire1") )
				{
					currentAbility.fire();
					print ("FIRE!");
				}
			}
		}
	}

	public void changeAbility(string abilityName)
	{
		if(currentAbility != null)
			Destroy(currentAbility);
		
		currentAbility = gameObject.AddComponent(abilityName) as AbilityBase;
	}
}
