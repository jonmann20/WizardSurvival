using UnityEngine;
using System.Collections;

public class Nametag : MonoBehaviour {
	void Start () {
		PhotonNetwork.player.name = BETWEEN_SCENES.player_name;
		GetComponent<TextMesh>().text = PhotonNetwork.player.name;
		print(GetComponent<TextMesh>() + "set to " + PhotonNetwork.player.name);
	}
}