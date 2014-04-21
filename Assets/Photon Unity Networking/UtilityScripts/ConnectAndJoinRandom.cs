using System;
using UnityEngine;
using System.Collections;
//using ExitGames.Client.Photon.Hashtable;

/// <summary>
/// This script automatically connects to Photon (using the settings file), 
/// tries to join a random room and creates one if none was found (which is ok).
/// </summary>
public class ConnectAndJoinRandom : Photon.MonoBehaviour
{
    /// <summary>Connect automatically? If false you can set this to true later on or call ConnectUsingSettings in your own scripts.</summary>
    public bool AutoConnect = true;

    /// <summary>if we don't want to connect in Start(), we have to "remember" if we called ConnectUsingSettings()</summary>
    private bool ConnectInUpdate = true;

    public virtual void Start()
    {
		GLOBAL.reset();
		PhotonNetwork.Disconnect();
        PhotonNetwork.autoJoinLobby = false;
    }

    public virtual void Update()
    {
        if (ConnectInUpdate && AutoConnect)
        {
            ConnectInUpdate = false;
            PhotonNetwork.ConnectUsingSettings("1");
        }
    }

    public virtual void OnConnectedToMaster()
    {
		ExitGames.Client.Photon.Hashtable customPlayerProps = new ExitGames.Client.Photon.Hashtable() {{"Score", 0}};
		customPlayerProps.Add("Health", 100);
		customPlayerProps.Add("Ability", "none");
		customPlayerProps.Add("Time", 300.0);
		customPlayerProps.Add("Wave", 0);
		customPlayerProps.Add("Orbs", 5);
		PhotonNetwork.player.SetCustomProperties(customPlayerProps);
        PhotonNetwork.JoinRandomRoom();
		PhotonNetwork.JoinRoom("MainRoom");
    }

	public virtual void OnFailedToConnectToPhoton()
	{
		GLOBAL.GameOver("Could not connect to server. Please try again.");
	}

	public virtual void OnPhotonJoinRoomFailed()
	{
		PhotonNetwork.CreateRoom("MainRoom", true, true, 4);
	}

	public virtual void OnPhotonCreateRoomFailed()
	{
		PhotonNetwork.JoinRoom("MainRoom");
	}

	public GameObject NetworkLauncherPrefab;
    public void OnJoinedRoom()
    {
		GLOBAL.inRoom = true;
		Instantiate(NetworkLauncherPrefab, new Vector3(0, 0, 0), Quaternion.identity);
	}
}
