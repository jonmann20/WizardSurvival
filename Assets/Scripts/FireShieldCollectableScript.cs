using UnityEngine;
using System.Collections;

public class FireShieldCollectableScript : CollectableBase {
	
	public override float getSelectedItemSizeInInventory() { return 5; }
	public override float getNonSelectedItemSizeInInventory() { return 4; }
	public override void customUpdate() { }
	public override void useItem()
	{ 
		Wizard.myWizard.GetComponent<AbilityManagerScript>().changeAbility("FireShieldAbility");
	}
}
