using UnityEngine;
using System.Collections;

using InControl;

public class AbilityManagerScript : MonoBehaviour {

	public static AbilityBase currentAbility;


	public void attemptFire(){
		if(currentAbility != null &&
		   currentAbility.isCharged()
		){
			currentAbility.fire();
		}
	}

	public void changeAbility(string abilityName)
	{
		if(currentAbility != null){
			Destroy(currentAbility);
		}

		currentAbility = gameObject.AddComponent(abilityName) as AbilityBase;

		if(GLOBAL.myWizard != null &&
		   GLOBAL.myWizard.GetComponent<PlayerController>() != null &&
		   GLOBAL.myWizard.GetComponent<PlayerController>().networkedProperties.ContainsKey("Ability")
		){
			GLOBAL.myWizard.GetComponent<PlayerController>().networkedProperties["Ability"] = AbilityManagerScript.currentAbility.getAbilityName();
			PhotonNetwork.player.SetCustomProperties(GLOBAL.myWizard.GetComponent<PlayerController>().networkedProperties);
		}
	}

	// eww ugly hack
	public bool isAbilityName(string n){
		if(currentAbility != null){
			return currentAbility.getAbilityName() == n;
		}

		return false;
	}
}
