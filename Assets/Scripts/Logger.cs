using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Logger : MonoBehaviour {

	public float RayLength = 2;

	const string FILE_NAME = "PosLog";
	const int MOVEMENT_CAPTURE_INTERVAL = 60;
	int movementCaptureTimer = MOVEMENT_CAPTURE_INTERVAL;

	List<LogElement> CurrentDatums = new List<LogElement>();
	List<LogElement> ReceivedDatums = new List<LogElement>();
	List<GameObject> LoggerPoints = new List<GameObject>();
	public GameObject LoggerPointPrefab;

	//We can keep track of datums with the same session (indicates an individual player)
	int randomSessionIdentifier = -1;
	public delegate void StringDelegate(string message);
	bool hasWritten = false;

	public static Logger that;

	void Start () {
		that = this;
		randomSessionIdentifier = Random.Range(0, 2147483);
		init();
	}

	void init()
	{
		RetrieveDataFromServer();
	}

	void Update()
	{
		--movementCaptureTimer;

		if(movementCaptureTimer <= 0)
		{
			movementCaptureTimer = MOVEMENT_CAPTURE_INTERVAL;
			if(GLOBAL.MainCamera != null && Wizard.myWizard != null)
			{
				Vector3 pos = Wizard.myWizard.transform.position;
				Vector3 rot = GLOBAL.MainCamera.transform.forward;
				CurrentDatums.Add(new LogElement(pos, rot, randomSessionIdentifier));
			}
		}

		//DRAW LINES
		for(int i = 0; i < ReceivedDatums.Count - 1; i++)
		{
			if(ReceivedDatums[i].player_id == ReceivedDatums[i+1].player_id)
				Debug.DrawLine(ReceivedDatums[i].position, ReceivedDatums[i+1].position, Color.white);

			Vector3 nRot = ReceivedDatums[i].rotation * RayLength;
			Debug.DrawRay(ReceivedDatums[i].position, nRot, Color.blue);
		}
		if(ReceivedDatums.Count > 0)
		{
			Vector3 nRot = ReceivedDatums[ReceivedDatums.Count - 1].rotation * RayLength;
			Debug.DrawRay(ReceivedDatums[ReceivedDatums.Count - 1].position, nRot, Color.blue);
		}
		//WRITE AT GAME END
		if(GLOBAL.gameOver && !hasWritten)
		{
			write ();
			hasWritten = true;
		}

		if(!GLOBAL.gameOver)
		{
			hasWritten = false;
		}
	}

	void accessData(JSONObject obj){
		switch(obj.type){
			case JSONObject.Type.OBJECT:
				for(int i = 0; i < obj.list.Count; ++i){
					JSONObject j = (JSONObject)obj.list[i];
					accessData(j);
				}
				break;
			case JSONObject.Type.ARRAY:
				float x = float.Parse(obj.list[0].ToString());
				float y = float.Parse(obj.list[1].ToString());
				float z = float.Parse(obj.list[2].ToString());

				float rx = float.Parse (obj.list[3].ToString());
				float ry = float.Parse (obj.list[4].ToString());
				float rz = float.Parse (obj.list[5].ToString());

				int id = int.Parse (obj.list[6].ToString());

				Vector3 pos = new Vector3(x, y, z);
				Vector3 rot = new Vector3(rx, ry, rz);

				ReceivedDatums.Add (new LogElement(pos, rot, id));
				//LoggerPoints.Add(Instantiate(LoggerPointPrefab, new Vector3(x, y, z), Quaternion.identity) as GameObject);
				break;
			case JSONObject.Type.STRING:
				Debug.Log(obj.str);
				break;
			case JSONObject.Type.NUMBER:
				Debug.Log(obj.n);
				break;
			case JSONObject.Type.BOOL:
				Debug.Log(obj.b);
				break;
			case JSONObject.Type.NULL:
				Debug.Log("NULL");
				break;	
		}
	}

	public void write()
	{
		JSONObject j = new JSONObject(JSONObject.Type.OBJECT);
		foreach(LogElement d in CurrentDatums)
		{
			JSONObject arr = new JSONObject(JSONObject.Type.ARRAY);
			j.AddField("data", arr);

			arr.Add(d.position.x);
			arr.Add(d.position.y);
			arr.Add(d.position.z);

			arr.Add(d.rotation.x);
			arr.Add(d.rotation.y);
			arr.Add(d.rotation.z);

			arr.Add(randomSessionIdentifier);
		}

		string writeContent = j.Print(false);
		POSTToServer(writeContent);
	}

	public void buildPositionPaths(string s)
	{
		string fileContents = s;
		JSONObject j = new JSONObject(fileContents);
		accessData(j);
	}

	public void RetrieveDataFromServer()
	{
		string url = "http://www.ayarger.com/GAMES/WizardSurvival/logging/retrieve.php";
		
		WWWForm form = new WWWForm();
		form.AddField("data", "noop");
		WWW www = new WWW(url, form);
		StringDelegate callback = buildPositionPaths;

		StartCoroutine(WaitForRequest(www, callback));
	}

	public void noop(string message) { }
	public void POSTToServer(string s)
	{
		string url = "http://www.ayarger.com/GAMES/WizardSurvival/logging/store.php";
		
		WWWForm form = new WWWForm();
		form.AddField("data", s);
		WWW www = new WWW(url, form);
		StringDelegate callback = noop;

		StartCoroutine(WaitForRequest(www, callback));
	}

	IEnumerator WaitForRequest(WWW www, StringDelegate callback)
	{
		yield return www;
		
		// check for errors
		if (www.error == null)
		{
			callback(www.text);
		}
	}

	//codes:
	//0 = normal, 1 = death
	public static void alert(int code, string message)
	{

	}
}