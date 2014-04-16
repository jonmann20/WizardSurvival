using UnityEngine;
using System.Collections;

public class DoorLeft : MonoBehaviour {

	private int door_LastIndex;
	public bool doorOpen = false;
	// Use this for initialization

	private void playDoorAnim () {
		animation.Play ("DoorLeftOpen");
		doorOpen = true;
	}

	void Start () {
		//animator = (Animator) gameObject.GetComponentInChildren (typeof(Animator));
		//animation = (Animation) gameObject.GetComponentInChildren (typeof(Animation));
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
			//playDoorAnim(); 
			//animation.Play ();
			//animator.SetBool("dooropen", doorOpen);
		}
	}
}
