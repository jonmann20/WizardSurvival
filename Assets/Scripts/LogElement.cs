using UnityEngine;
using System.Collections;

public class LogElement {

	public Vector3 position;
	public Vector3 rotation;
	public int player_id;

	public LogElement(Vector3 pos, Vector3 rot, int pID)
	{
		position = pos;
		rotation = rot;
		player_id = pID;
	}
}
