using UnityEngine;
using System.Collections;

public class FireballScript : MonoBehaviour {
	
	public int life = 0;

	void Update () {
		life --;
		float fraction = (float)life / 30.0f;
		
		if(fraction < 1.0f)
		{
			transform.localScale = new Vector3(fraction, fraction, fraction);
		}
		
		if(life <= 0)
			PhotonNetwork.Destroy(gameObject);
	}
}
