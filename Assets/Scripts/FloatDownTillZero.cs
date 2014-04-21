using UnityEngine;
using System.Collections;

public class FloatDownTillZero : MonoBehaviour {
	
	void Update () {
		if(transform.position.y > 1)
		{
			transform.Translate(0, -0.1f, 0);
		}
	}
}
