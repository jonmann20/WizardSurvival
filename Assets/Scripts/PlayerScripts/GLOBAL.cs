using UnityEngine;
using System.Collections;

public class GLOBAL : MonoBehaviour {

	public static int health = 100;

	void Awake()
	{
		MainCamera = GameObject.FindWithTag("MainCamera") as GameObject;
	}

	public static GameObject MainCamera;
	public static string WizardName;

}
