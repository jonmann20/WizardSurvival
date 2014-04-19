using UnityEngine;
using System.Collections;

public class Wizard : MonoBehaviour {

	public GameObject[] parts;
	PhotonPlayer ownerPhotonPlayer;
	public Material[] materials;
	bool isMaterialSet = false;

	void Start () {
		ownerPhotonPlayer = GetComponent<PhotonView>().owner;
	}

	void Update()
	{
		if(!isMaterialSet)
		{
			string abilityName = (string)ownerPhotonPlayer.customProperties["Ability"];
			switch(abilityName)
			{
			case "Fireball":
				swapMat(materials[0]);
				break;
			case "Forcefield":
				swapMat(materials[1]);
				break;
			case "Ice Blast":
				swapMat(materials[2]);
				break;
			}
		}

		int netHealth = (int)ownerPhotonPlayer.customProperties["Health"];
		print("network health: " + netHealth);
	}

	public void swapMat(Material m){
		foreach(GameObject p in parts){
			Material[] mats = p.renderer.materials;
			mats[1] = m;
			p.renderer.materials = mats;
		}

		isMaterialSet = true;
	}
}
