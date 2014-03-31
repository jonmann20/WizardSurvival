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
			if(currentAbility.isCharged())
			{
                InputDevice device = InputManager.ActiveDevice;
                InputControl ctrl_RightBumper = device.GetControl(InputControlType.RightBumper);

				if(ctrl_RightBumper.IsPressed && ctrl_RightBumper.LastValue == 0)
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

		if( Wizard.myWizard.GetComponent<PlayerController>().networkedProperties.ContainsKey("Ability") )
		{
			Wizard.myWizard.GetComponent<PlayerController>().networkedProperties["Ability"] = AbilityManagerScript.currentAbility.getAbilityName();
			PhotonNetwork.player.SetCustomProperties(Wizard.myWizard.GetComponent<PlayerController>().networkedProperties);
		}
	}
}
