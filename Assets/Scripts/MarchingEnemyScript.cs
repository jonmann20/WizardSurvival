using UnityEngine;
using System.Collections;

public class MarchingEnemyScript : MonoBehaviour {

	void Update(){
		transform.Translate(0, 0, 0.6f * Time.deltaTime);
	}
}
