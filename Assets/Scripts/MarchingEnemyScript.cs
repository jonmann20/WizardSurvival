using UnityEngine;
using System.Collections;

public class MarchingEnemyScript : MonoBehaviour {

	void FixedUpdate(){
		transform.Translate(0, 0, 0.95f * Time.deltaTime);
	}
}
