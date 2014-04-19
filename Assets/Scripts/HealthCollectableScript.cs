using UnityEngine;
using System.Collections;

public class HealthCollectableScript : CollectableBase {
	
	public override float getSelectedItemSizeInInventory() { return 0.1f; }
	public override float getNonSelectedItemSizeInInventory() { return 0.9f; }
	public override void customUpdate() { }
	public override void useItem() 
	{ 
		GLOBAL.myWizard.GetComponent<PlayerController>().TakeDamage(-60, transform);
	}
	public override string getName() { return "Surgeon's Delight"; }
}