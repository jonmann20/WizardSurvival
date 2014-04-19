using UnityEngine;
using System.Collections;

public class BETWEEN_SCENES : MonoBehaviour {

	public static string player_name = "Nameless Wizard";

	void Awake()
	{
		DontDestroyOnLoad (transform.gameObject);
	}
}
