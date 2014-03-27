using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GLOBAL : MonoBehaviour {

	public static int health = 100;
	public static List<GameObject> Inventory = new List<GameObject>();

	void Awake()
	{
		MainCamera = GameObject.FindWithTag("MainCamera") as GameObject;
	}

	public static GameObject MainCamera;
	public static string WizardName;
}
