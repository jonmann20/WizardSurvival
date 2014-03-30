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
        PhotonNetwork.autoJoinLobby = false;    // we join randomly. always. no need to join a lobby to get the list of rooms.
    }

    public virtual void Update()
    {
        if (ConnectInUpdate && AutoConnect)
        {
            //Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");

            ConnectInUpdate = false;
            PhotonNetwork.ConnectUsingSettings("1");
        }
    }

    // to react to events "connected" and (expected) error "failed to join random room", we implement some methods. PhotonNetworkingMessage lists all available methods!

    public virtual void OnConnectedToMaster()
    {
		ExitGames.Client.Photon.Hashtable customPlayerProps = new ExitGames.Client.Photon.Hashtable() {{"Score", 0}};
		customPlayerProps.Add("Health", 100);
		PhotonNetwork.player.SetCustomProperties(customPlayerProps);
        //Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
        PhotonNetwork.JoinRandomRoom();
    }

    public virtual void OnPhotonRandomJoinFailed()
    {
        Debug.Log("OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. Calling: PhotonNetwork.CreateRoom(null, true, true, 4);");
        PhotonNetwork.CreateRoom(null, true, true, 4);
    }

    // the following methods are implemented to give you some context. re-implement them as needed.

    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.LogError("Cause: " + cause);
    }

	public GameObject NetworkLauncherPrefab;
    public void OnJoinedRoom()
    {
        //Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");
		Instantiate(NetworkLauncherPrefab, new Vector3(0, 0, 0), Quaternion.identity);
	}

    public virtual void OnJoinedLobby()
    {
        //Debug.Log("OnJoinedLobby(). Use a GUI to show existing rooms available in PhotonNetwork.GetRoomList().");
    }
}
