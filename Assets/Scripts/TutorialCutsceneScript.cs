using UnityEngine;
using System.Collections;
using InControl;

public class TutorialCutsceneScript : MonoBehaviour {

	public Font SpookyMagic;

	int state = 0;
	int timer = 400;
	string message = "Behold your kingdom...";

	Vector3 initState0 = new Vector3(-58, 7.2f, 2.19f);
	Vector3 endState0 = new Vector3(71, 94, -80);
	Vector3 rotationState0 = new Vector3(19.6f, 300.97f, 0);

	Vector3 initState1 = new Vector3(-86.27f, 8.7235f, 399.6f);
	Vector3 endState1 = new Vector3(-86.2f, 10.3678f, 378.199f);
	Vector3 rotationState1 = new Vector3(7.4f, 357.4f, 0);

	Vector3 initState2 = new Vector3(-51.85f, 1.67f, 12.8f);
	Vector3 endState2 = new Vector3(-51.85f, 1.67f, 5.768f);
	Vector3 rotationState2 = new Vector3(7.4f, 88.9f, 0);

	Vector3 initState3 = new Vector3(-58.96f, 1.676f, -10.4f);
	Vector3 endState3 = new Vector3(-60.9f, 1.676f, -7.09f);
	Vector3 rotationState3 = new Vector3(-0.97f, -34.9f, 0);

	InputDevice device;

	void Start () {
		InputDevice device = InputManager.ActiveDevice;
	}

	void Update()
	{
		InputDevice device = InputManager.ActiveDevice;
		InputControl ctrl_Start = device.GetControl(InputControlType.Start);
		
		if(ctrl_Start.WasPressed) {	// NOTE: doesn't seem to work when active in TextField
			Application.LoadLevel("MattScene");
		}
	}

	void FixedUpdate()
	{
		timer --;

		if(timer <= 0)
		{
			state ++;
			if(state == 1)
				timer = 400;
			else if(state == 2)
				timer = 400;
			else if(state == 3)
				timer = 300;
			else if(state == 4)
				Application.LoadLevel("MattScene");
		}

		//STATES
		if(state == 0)
		{
			message = "Your kingdom is in danger...";
			transform.position = Vector3.Lerp(initState0, endState0, 1 - (float)timer / (float)400);
			transform.eulerAngles = rotationState0;
			return;
		}

		if(state == 1)
		{
			message = "Hold out as long as you can...";
			transform.position = Vector3.Lerp(initState1, endState1, 1 - (float)timer / (float)400);
			transform.eulerAngles = rotationState1;
			return;
		}

		if(state == 2)
		{
			message = "ITEMS from the heavens will aid you...";
			transform.position = Vector3.Lerp(initState2, endState2, 1 - (float)timer / (float)400);
			transform.eulerAngles = rotationState2;
			return;
		}

		if(state == 3)
		{
			message = "COLLECT the ORBS for reinforcements...";
			transform.position = Vector3.Lerp(initState3, endState3, 1 - (float)timer / (float)300);
			transform.eulerAngles = rotationState3;
			return;
		}
	}

	void OnGUI(){

		EZGUI.init();
		
		float startX = 222 + 30;
		float startY = 265;

		EZGUI.drawBox(-10, -10, 2000, 200, new Color(0, 0, 0, 1));
		EZGUI.drawBox(-10, -180 + EZGUI.FULLH, 2000, 200, new Color(0, 0, 0, 1));

		EZOpt e = new EZOpt();
		e.dropShadow = new Color(0.1f, 0.1f, 0.1f);
		e.leftJustify = false;
		
		e.font = SpookyMagic;

		EZGUI.placeTxt("Press \"Start\" to skip", 23, EZGUI.FULLW - 230, 100, e);
		EZGUI.placeTxt(message, 50, EZGUI.HALFW, EZGUI.FULLH - 60, e);
	}
}
