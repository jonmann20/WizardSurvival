using UnityEngine;
using System.Collections;

public abstract class ObjectiveBase : MonoBehaviour {
	
	public void Awake () {
	
	}

	public void Update () {
		customUpdate();
	}
	
	public abstract void initObjective();
	public abstract bool isObjectiveFinished();
	public abstract void customUpdate();
	public abstract void finishObjective();
}