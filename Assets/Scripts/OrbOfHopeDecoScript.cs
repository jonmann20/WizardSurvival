using UnityEngine;
using System.Collections;

public class OrbOfHopeDecoScript : MonoBehaviour {

	int life = 2000;
	void Update () {
		transform.Translate(0, 0.06f, 0);
		life --;
		if(life <= 0)
			Destroy(gameObject);
	}
}