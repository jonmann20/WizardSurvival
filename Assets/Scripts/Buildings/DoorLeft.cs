using UnityEngine;
using System.Collections;

public class DoorLeft : MonoBehaviour {

	private int door_LastIndex;
	public bool doorOpen = false;
	private Animator animator;
	private int count = 0;
	// Use this for initialization

	private void playDoorAnim () {
		gameObject.transform.GetChild(0).animation.Play ();
		doorOpen = true;
	}

	void Start () {
		animator = (Animator) gameObject.GetComponentInChildren (typeof(Animator));
	}

	void Update () {
		/*for( int i = 0; i < PhotonNetwork.playerList.Length; i++ )
		{
			if( PhotonNetwork.playerList[i].customProperties.ContainsKey("Ability"))
			{
				++count;
			}
		}*/

		if (AbilityManagerScript.currentAbility != null && doorOpen == false) {
			doorOpen = true;
			playDoorAnim(); 
		}
	}
}
