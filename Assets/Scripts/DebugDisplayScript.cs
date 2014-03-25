using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DebugDisplayScript : MonoBehaviour {

	delegate List<string> StringListReturningDelegate();
	List<StringListReturningDelegate> ThingsToDisplay = new List<StringListReturningDelegate>();

	//FPS STUFF
	int m_frameCounter = 0;
	float m_timeCounter = 0.0f;
	float m_lastFramerate = 0.0f;
	public float m_refreshTime = 0.5f;

	//To add additional debug information...
	//(1) Create a new string-returning function
	//(2) Add a StringReturningDelegate for that function to ThingsToDisplay in Awake()
	void Awake()
	{
		ThingsToDisplay.Add(new StringListReturningDelegate(fps));
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

	List<string> fps()
	{
		List<string> output = new List<string>();
		string str = "fps: " + m_lastFramerate;
		output.Add(str);
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

	void Update()
	{
		//FPS STUFF
		if( m_timeCounter < m_refreshTime )
		{
			m_timeCounter += Time.deltaTime;
			m_frameCounter++;
		}
		else
		{
			//This code will break if you set your m_refreshTime to 0, which makes no sense.
			m_lastFramerate = (float)m_frameCounter/m_timeCounter;
			m_frameCounter = 0;
			m_timeCounter = 0.0f;
		}
	}
}
