using UnityEngine;
using System.Collections;

public class HealthCollectableScript : CollectableBase {
	
	public override float getSelectedItemSizeInInventory() { return 1.5f; }
	public override float getNonSelectedItemSizeInInventory() { return 1; }
	public override void customUpdate() { }
	public override void useItem() { }
}
