using UnityEngine;
using System.Collections;

public class CLIENTIZER_Enemy : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		if(GetComponent<PhotonView>().isMine)
		{
			GetComponent<SyncScript>().enabled = false;
		}
		else
		{
			Destroy(GetComponent<FollowClosestPlayer>());

			Transform[] allChildren = GetComponentsInChildren<Transform>();
			foreach (Transform child in allChildren) {
				if(child.gameObject.name == "AI")
				{
					Destroy(child);
					continue;
				}
				if(child.gameObject.name == "skeletonNormal")
				{
					Destroy(child.GetComponent<MGAISuperClass>());
					Destroy(child.GetComponent<SampleAIController>());
				}
			}
		}
	}
}
