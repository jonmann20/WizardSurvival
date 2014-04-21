using UnityEngine;
using System.Collections;

public class BETWEEN_SCENES : MonoBehaviour {

	public static string player_name = "Tim the great";
	public static bool jeremyEasterEgg = false;
	public static bool isaiahEasterEgg = false;

	void Awake(){
		DontDestroyOnLoad(transform.gameObject);
	}
}
