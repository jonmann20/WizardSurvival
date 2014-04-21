using UnityEngine;
using System.Collections;

public  class OrbOfHopeScript : MonoBehaviour {
	
	float sinCounter = 0.0f;
	
	void Update()
	{
		if(PhotonNetwork.isMasterClient){
			sinCounter += 0.025f;
			transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(sinCounter) * 0.01f, transform.position.z);

			transform.Rotate(Vector3.up * Time.deltaTime * 55);
		}
	}
	
	public void OnCollisionEnter(Collision collision){
		
		if(collision.gameObject.tag == "Player" && collision.collider.gameObject.transform.parent.GetComponent<PhotonView>().isMine)
		{
			GameObject[] existingOrbs = GameObject.FindGameObjectsWithTag("OrbOfHope");
			int numberOfOrbsLeft = ((int)existingOrbs.Length) - 1;
			if(numberOfOrbsLeft > 0)
				HudScript.addNewMessage("Orb of Hope Collected! " + numberOfOrbsLeft + " left!", 120, new Color(255, 215, 0));
			GLOBAL.announceOrbCollected();
			GameAudio.playOrbCollect();
			GLOBAL.myWizard.GetComponent<PlayerController>().IncrementPoints(10);
			GLOBAL.that.SuperDestroy(gameObject);
		}
	}
}
