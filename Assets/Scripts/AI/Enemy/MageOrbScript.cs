using UnityEngine;
using System.Collections;

public class MageOrbScript : MonoBehaviour {

	public float life = 3.5f;
	
	// Update is called once per frame
	void Update () {
		life -= Time.deltaTime;

		if( life <= 0 )
		{
			GLOBAL.that.SuperDestroy(this.gameObject);
		}
	}
	
}
