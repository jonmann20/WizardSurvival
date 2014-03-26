using UnityEngine;
using System.Collections;
using RAIN.Core;

public class Health : MonoBehaviour {
	public float health = 100f;
	public AIRig aiRig = null;

	// Use this for initialization
	void Start () {
		aiRig = gameObject.GetComponentInChildren<AIRig>();
	}
	
	// Update is called once per frame
	void Update () {
	
		aiRig.AI.WorkingMemory.SetItem("health", health);
	}

	public void Damage( float damage )
	{
		health = Mathf.Clamp(health-damage,0,health);
	}
}
