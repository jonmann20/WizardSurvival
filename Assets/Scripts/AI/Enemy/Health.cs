using UnityEngine;
using System.Collections;
using RAIN.Core;

public class Health : MonoBehaviour {
    SampleAIController aiController;
	AIRig aiRig = null;

	void Start(){
		aiRig = GetComponentInChildren<AIRig>();
        aiController = transform.GetComponentInChildren<SampleAIController>();
	}
	
	void Update(){
        aiRig.AI.WorkingMemory.SetItem("health", aiController.health);
	}

	public void Damage(float damage){
        this.transform.GetComponentInChildren<SampleAIController>().health = Mathf.Clamp(aiController.health - damage, 0, aiController.health);
	}	
}
