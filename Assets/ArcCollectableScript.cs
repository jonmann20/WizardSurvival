using UnityEngine;
using System.Collections;

public class ArcCollectableScript : CollectableBase {
	
	public override float getSelectedItemSizeInInventory() { return 5; }
	public override float getNonSelectedItemSizeInInventory() { return 4; }
	public override void customUpdate() { }
	public override void useItem() 
	{ 
		AbilityBase ability = Wizard.myWizard.AddComponent<FireArcAbility>() as AbilityBase;
		ability.fire();
	}
}