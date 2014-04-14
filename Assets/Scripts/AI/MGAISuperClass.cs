using UnityEngine;
using System.Collections;
using MGSpawn.Spawn;

public class MGAISuperClass : MonoBehaviour {

	//the gameobject that Spawned this
	private MGSpawner mOwner;

	public virtual void SetOwner(MGSpawner owner)
	{
		mOwner = owner;
	}
	
	public virtual void Remove()
	{
		if(mOwner != null)
		{
			mOwner.RemoveUnit();
		}

		GLOBAL.that.SuperDestroy(this.transform.parent.transform.gameObject);
	}
}
