using UnityEngine;
using System.Collections;

public class LimitedLife : MonoBehaviour {
	
	public int life = 180;
	
	void Update()
	{
		life --;
		if(life < 30)
		{
			transform.localScale = new Vector3(1, 1, 1) * ((float)life / 30.0f);
		}
		if(life <= 0)
			GLOBAL.that.SuperDestroy(gameObject);
	}
}
