using UnityEngine;
using System.Collections;

public class titlehudscript : MonoBehaviour {
	public static GameObject hudCamera;
	Light hudLight;
	
	void Awake(){
		hudCamera = GameObject.FindWithTag("HudCamera") as GameObject;
		hudLight = (GameObject.FindWithTag("HudLight") as GameObject).GetComponent<Light>();
	}
}
