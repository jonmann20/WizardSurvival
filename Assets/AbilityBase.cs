using UnityEngine;
using System.Collections;

public abstract class AbilityBase : MonoBehaviour {

	int charge = 0;
	int maxCharge = 0;

	public abstract void fire();

	public int getCharge() { return charge; }
	public bool isCharged() { return (charge >= maxCharge); }
	public abstract string getAbilityName();
	public abstract string getAbilityDescription();
}
