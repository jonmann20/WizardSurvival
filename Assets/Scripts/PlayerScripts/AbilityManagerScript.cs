using UnityEngine;
using System.Collections;

using InControl;

public class AbilityManagerScript : MonoBehaviour {

	public static AbilityBase currentAbility;

	void Start () {
		changeAbility("FireballAbility");
	}
	
	void Update(){
		if( currentAbility )
		{
			if( currentAbility.isCharged() )
			{
                InputDevice device = InputManager.ActiveDevice;
                InputControl ctrl_RightTrigger = device.GetControl(InputControlType.RightTrigger);

				if(ctrl_RightTrigger.IsPressed && ctrl_RightTrigger.LastValue == 0)
				{
					currentAbility.fire();
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
