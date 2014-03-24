using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DebugDisplayScript : MonoBehaviour {

	delegate List<string> StringListReturningDelegate();
	List<StringListReturningDelegate> ThingsToDisplay = new List<StringListReturningDelegate>();

	//To add additional debug information...
	//(1) Create a new string-returning function
	//(2) Add a StringReturningDelegate for that function to ThingsToDisplay in Awake()
	void Awake()
	{
		ThingsToDisplay.Add(new StringListReturningDelegate(numberOfConnectedPlayers));
		ThingsToDisplay.Add(new StringListReturningDelegate(playerListInformation));
	}

	List<string> numberOfConnectedPlayers()
	{
		List<string> output = new List<string>();
		output.Add("Connected Players: " + PhotonNetwork.playerList.Length);
		return output;
	}

	List<string> playerListInformation()
	{
		List<string> output = new List<string>();
		output.Add("playerList:");
		foreach(PhotonPlayer p in PhotonNetwork.playerList)
		{
			output.Add("\t(ID: " + p.ID + " name: " + p.name + " isLocal: " + p.isLocal + " isMasterClient: " + p.isMasterClient + ")");

			/*
			output.Add("\tCustom Properties");

			Hashtable table = (Hashtable)p.customProperties;
			foreach(DictionaryEntry pair in table)
			{
				output.Add ("\t\t(" + pair.Key + ", " + pair.Value + ")");
			}*/
		}

		return output;
	}
	
	void OnGUI()
	{
		for(int i = 0; i < ThingsToDisplay.Count; i++)
		{
			List<string> currentThingList = ThingsToDisplay[i]();
			for(int j = 0; j < currentThingList.Count; j++)
			{
				string thingToDisplay = currentThingList[j];
				GUI.Label(new Rect(10, i * 20 + j * 20, 999, 20), thingToDisplay);
			}
		}
	}
}
