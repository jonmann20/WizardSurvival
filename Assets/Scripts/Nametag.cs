using UnityEngine;
using System.Collections;

public class Nametag : MonoBehaviour {

	GameObject MainCamera;

	void Update () {

		if(MainCamera == null)
			MainCamera = GameObject.Find("MainCamera");
		else
		{
			transform.rotation = Quaternion.LookRotation(MainCamera.transform.forward);
			PhotonPlayer p = transform.parent.gameObject.GetComponent<PhotonView>().owner;

			if(transform.parent.gameObject.GetComponent<PhotonView>().isMine)
			{
				PhotonNetwork.playerName = BETWEEN_SCENES.player_name;
				GetComponent<TextMesh>().text = "";
			}
			else
			{
				GetComponent<TextMesh>().text = p.name;
			}
		}
	}
}