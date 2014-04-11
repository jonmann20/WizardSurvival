using UnityEngine;
using System.Collections;

public class Message {

	public string messageString;
	public int life;
	public Color color;

	public Message(string messString, int dur, Color c)
	{
		messageString = messString;
		life = dur;
		color = c;
	}
}
