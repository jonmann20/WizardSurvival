using UnityEngine;
using System.Collections;

public class OrbOfHopeDecoScript : MonoBehaviour {
	
	void Update () {
		transform.Translate(0, 0.06f, 0);
		if(transform.position.y > 10)
			Destroy(gameObject);
	}
}