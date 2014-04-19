using UnityEngine;
using System.Collections;

public class BETWEEN_SCENES : MonoBehaviour {

	public static string player_name = "Pappy the solar seal";

	void Awake()
	{
		DontDestroyOnLoad (transform.gameObject);
	}
}
