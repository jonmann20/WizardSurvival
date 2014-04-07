using UnityEngine;
using System.Collections;

public class HealthCollectableScript : CollectableBase {
	
	public override float getSelectedItemSizeInInventory() { return 0.5f; }
	public override float getNonSelectedItemSizeInInventory() { return 1; }
	public override void customUpdate() { }
	public override void useItem() 
	{ 
		GLOBAL.health += 60;
		if(GLOBAL.health > 100)
			GLOBAL.health = 100;
	}
	public override string getName() { return "Surgeon's Delight"; }
}