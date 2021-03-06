﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class HudScript : MonoBehaviour {

	bool playedLowHeath = false;

	const float VOXEL_WIDTH = 0.2f;
    float HEALTHBAR_X_OFFSET = -3.75f;
	const float HEALTHBAR_Y_OFFSET = 2.0f;
	const float HEALTHBAR_Z_OFFSET = 4;
	const float HEALTHBAR_FLOAT_RATE = 0.005f;
	const float HEALTHBAR_FLOAT_ALTITUDE = 0.05f;
	const int NUMBER_OF_VOXELS = 5;
	float healthPerVoxel = 100;

	float sinCounter = 0.0f;

	List<GameObject> healthVoxels = new List<GameObject>();
	public static GameObject hudCamera;

	//TEXT
    public GameObject AbilityNameText;
    //GameObject AbilityDescriptionText;
    //public Font font;
    //public GameObject AbilityNameText;
    public GameObject ScoreText;
	TextMesh scoreTxtMesh;
	public GameObject RoundTimer;

	public GameObject WaveText;
	public GameObject OrbText;
	//private float timer = 300f;
	//private float seconds = 0;
	//private int minutes = 0;
	private int wave;

	//HealthBar
	public Texture healthBarTexture;

	//PERIL FLASH
	Light hudLight;

	public Shader toonShader;
	public Shader toonShaderLight;

	//INVENTORY
	const float INVENTORY_OFFSET_X = -3.65f;
	const float INVENTORY_OFFSET_Y = -2.1f;
	const float INVENTORY_OFFSET_Z = 5;
	const float INVENTORY_PANELS_X_SCALE = 0.3f;
	const float INVENTORY_PANELS_Y_SCALE = 0.08f;
	const float INVENTORY_PANELS_Z_SCALE = 0.08f;
	const float INVENTORY_PANELS_X_SEPARATION = 0.3f;
	public float target = 1.0f;
	int inventorySelectedIndex = 0;

	//INVENTORY TEXTS
	GameObject FirstItemQuantityText;
	GameObject SecondItemQuantityText;
	GameObject ThirdItemQuantityText;

	//MESSAGE TEXT
	GameObject MessageText;
	public static Queue<Message> messageQueue = new Queue<Message>();

	//Leaderboard Button
	const float LEADERBOARD_X = 0.75f;
	const float LEADERBOARD_Y = 0.85f;
	const float LEADERBOARD_WIDTH = 0.18f;
	const float LEADERBOARD_HEIGHT = 0.07f;

	void Awake(){
		healthPerVoxel = 100.0f / (float)NUMBER_OF_VOXELS;
		hudCamera = GameObject.FindWithTag("HudCamera") as GameObject;
		hudLight = (GameObject.FindWithTag("HudLight") as GameObject).GetComponent<Light>();

		FirstItemQuantityText = GameObject.Find("FirstItemQuantityText") as GameObject;
		SecondItemQuantityText = GameObject.Find("SecondItemQuantityText") as GameObject;
		ThirdItemQuantityText = GameObject.Find("ThirdItemQuantityText") as GameObject;

		scoreTxtMesh = ScoreText.GetComponent<TextMesh>();
		MessageText = GameObject.Find("MessageText") as GameObject;

		//HEALTH
		Color greenColor = Color.green;
		Color redColor = Color.red;
		float fraction = 0.0f;
		
		for(int i = 0; i < NUMBER_OF_VOXELS; ++i)
		{
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

			cube.layer = 9;
			cube.transform.parent = hudCamera.transform;
			cube.transform.position = new Vector3(i * VOXEL_WIDTH + 0.1f + HEALTHBAR_X_OFFSET, HEALTHBAR_Y_OFFSET, HEALTHBAR_Z_OFFSET);
			Bouncy b = cube.AddComponent<Bouncy>() as Bouncy;
			b.target = 0.15f;

			Material mat = new Material(toonShaderLight);
			mat.color = Color.Lerp(redColor, greenColor, fraction);
			fraction += 1.0f / (float)NUMBER_OF_VOXELS;
			cube.GetComponent<MeshRenderer>().material = mat;

			healthVoxels.Add(cube);
		}

		//TEXT
        //AbilityNameText = new GameObject();
        //AbilityNameText.layer = 9;
        //AbilityNameText.transform.parent = hudCamera.transform;
        //AbilityNameText.transform.localPosition = new Vector3(2.5f, 2.39f, 4.76f);

        //TextMesh textMesh = AbilityNameText.AddComponent("TextMesh") as TextMesh;
        //MeshRenderer meshRenderer = AbilityNameText.GetComponent("MeshRenderer") as MeshRenderer;

        //Font ArialFont = (Font)Resources.GetBuiltinResource (typeof(Font), "Arial.ttf");
        ////print(ArialFont);
        //textMesh.font = ArialFont;
        //textMesh.renderer.material = ArialFont.material;
        //textMesh.characterSize = 0.1f;
        //textMesh.tabSize = 4;
        //textMesh.fontSize = 31;
	}

	void FixedUpdate(){
		sinCounter += HEALTHBAR_FLOAT_RATE;

		if(sinCounter > Mathf.PI * healthVoxels.Count * 2){
			sinCounter = 0;
		}
		

		for(int i=0; i < healthVoxels.Count; ++i)
		{
			float bonusHeight = 0;

			bonusHeight = Mathf.Sin(sinCounter * i) * HEALTHBAR_FLOAT_ALTITUDE;
			healthVoxels[i].transform.localPosition = new Vector3(i * (VOXEL_WIDTH + 0.05f) + HEALTHBAR_X_OFFSET, HEALTHBAR_Y_OFFSET + bonusHeight, HEALTHBAR_Z_OFFSET);

			//ADJUST FOR CURRENT HEALTH
			Bouncy b = healthVoxels[i].GetComponent<Bouncy>() as Bouncy;

			if(GLOBAL.health < (i + 1) * healthPerVoxel)
				b.target = 0.05f;
			else
				b.target = 0.15f;
		}

        // TEXT
        if(AbilityManagerScript.currentAbility != null)
        {
            AbilityNameText.GetComponent<TextMesh>().text = AbilityManagerScript.currentAbility.getAbilityName();
        }

		//WARNING FLASH
		if(GLOBAL.health <= 40)
		{	
			if(!playedLowHeath){
				playedLowHeath = true;
				GameAudio.playLowHP();
			}

			hudLight.intensity = Mathf.Abs(Mathf.Sin(sinCounter * 10)) * 5;
		}
		else
			hudLight.intensity = 8;

		//INVENTORY
		int numInventoryItems = GLOBAL.getInventoryCount();

		for(int i=0; i < numInventoryItems; ++i)
		{
			GameObject g = GLOBAL.getInventoryItemAt(i);
			g.layer = 9;
			g.transform.parent = hudCamera.transform;

			if(i == inventorySelectedIndex)
			{
				g.GetComponent<InventoryItemScript>().target = g.GetComponent<CollectableBase>().getSelectedItemSizeInInventory() + 1;
				g.transform.Rotate(Vector3.up * Time.deltaTime * 55);
			}
			else
			{
				g.GetComponent<InventoryItemScript>().target = g.GetComponent<CollectableBase>().getNonSelectedItemSizeInInventory();
				g.transform.rotation = Quaternion.identity;
			}

			if(i == 0 || i == 2)
			{
				g.transform.localPosition = new Vector3(INVENTORY_OFFSET_X + i * (INVENTORY_PANELS_X_SCALE + INVENTORY_PANELS_X_SEPARATION),
			                                        INVENTORY_OFFSET_Y + 0.1f,
			                                        4);
			}
			else
			{
				g.transform.localPosition = new Vector3(INVENTORY_OFFSET_X + i * (INVENTORY_PANELS_X_SCALE + INVENTORY_PANELS_X_SEPARATION),
				                                         INVENTORY_OFFSET_Y + 0.4f,
				                                         4);
			}

			//Quantity text
			//Workaround for the generally poor process of creating 3D texts dynamically.
			if(i == 0)
				setQuantityText(FirstItemQuantityText, g);
			else if(i == 1)
				setQuantityText(SecondItemQuantityText, g);
			else if(i == 2)
				setQuantityText(ThirdItemQuantityText, g);
		}

		if(numInventoryItems < 3)
			ThirdItemQuantityText.GetComponent<TextMesh>().text = "";
		if(numInventoryItems < 2)
			SecondItemQuantityText.GetComponent<TextMesh>().text = "";
		if(numInventoryItems < 1)
			FirstItemQuantityText.GetComponent<TextMesh>().text = "";

		// MESSAGE TEXT
		if(messageQueue.Count > 0)
		{
			MessageText.GetComponent<TextMesh>().text = messageQueue.Peek().messageString;
			MessageText.renderer.material.color = messageQueue.Peek().color;
			if(messageQueue.Peek().life > 0){
				--messageQueue.Peek().life;
				MessageText.transform.localPosition += (new Vector3(0.1f, -1.50f, 3.13f) - MessageText.transform.localPosition) * 0.1f;
			}
			else{
				MessageText.transform.localPosition += (new Vector3(0.1f, -2.05f, 3.13f) - MessageText.transform.localPosition) * 0.1f;
				
				//Swap to new message
				if(MessageText.transform.localPosition.y < -2.0f)
				{
					messageQueue.Dequeue();
				}
			}
		}
	}

	void setQuantityText(GameObject textGameObject, GameObject inventoryObject)
	{
		textGameObject.transform.position = inventoryObject.transform.position + new Vector3(0, 1, 0);

		int quantity = inventoryObject.GetComponent<CollectableBase>().getQuantity();
		if(quantity == 0)
			textGameObject.GetComponent<TextMesh>().text = "";
		else
			textGameObject.GetComponent<TextMesh>().text = "" + quantity;
	}

	void Update()
	{
		InputDevice device = InputManager.ActiveDevice;
		InputControl ctrl_invL = device.GetControl(InputControlType.LeftTrigger);
		InputControl ctrl_invR = device.GetControl(InputControlType.RightTrigger);
		InputControl ctrl_special = device.GetControl(InputControlType.LeftBumper);

		InputControl ctrl_DpadLeft = device.GetControl(InputControlType.DPadLeft);
		InputControl ctrl_DpadUp = device.GetControl(InputControlType.DPadUp);
		InputControl ctrl_DpadRight = device.GetControl(InputControlType.DPadRight);
		InputControl ctrl_Triangle = device.GetControl(InputControlType.Action4);

		int numInventoryItems = GLOBAL.getInventoryCount();

		if(ctrl_invL.WasPressed){
			if(inventorySelectedIndex > 0){
				GameAudio.playInvMove();
				--inventorySelectedIndex;
			}
			else {
				GameAudio.playInvNoMove();
			}
		}

		if(ctrl_invR.WasPressed){
			if(inventorySelectedIndex < numInventoryItems - 1){
				GameAudio.playInvMove();
				++inventorySelectedIndex;
			}
			else {
				GameAudio.playInvNoMove();
			}
		}

		if(ctrl_DpadLeft.WasPressed)
		{
			if(inventorySelectedIndex != 0){
				GameAudio.playInvMove();
				inventorySelectedIndex = 0;
			}
			else {
				GameAudio.playInvNoMove();
			}
		}

		if(ctrl_DpadUp.WasPressed)
		{
			if(inventorySelectedIndex != 1  && numInventoryItems > 1){
				GameAudio.playInvMove();
				inventorySelectedIndex = 1;
			}
			else {
				GameAudio.playInvNoMove();
			}
		}

		if(ctrl_DpadRight.WasPressed)
		{
			if(inventorySelectedIndex != 2 && numInventoryItems > 2) {
				GameAudio.playInvMove();
				inventorySelectedIndex = 2;
			}
			else {
				GameAudio.playInvNoMove();
			}
		}

		if(ctrl_special.WasPressed || ctrl_Triangle.WasPressed){

			bool isPlayerDown = (GLOBAL.health <= 0);
			bool isItemHealthPotion = (GLOBAL.getInventoryItemAt(inventorySelectedIndex).GetComponent<CollectableBase>().getName() == "Surgeon's Delight");
			if(inventorySelectedIndex > -1 && inventorySelectedIndex < numInventoryItems){

				if(!isPlayerDown)
				{
					GameAudio.playInvSelect();
					GLOBAL.useInventoryItemAt(inventorySelectedIndex);
				}
				else
				{
					if(!isItemHealthPotion)
					{
						GameAudio.playInvSelect();
						GLOBAL.useInventoryItemAt(inventorySelectedIndex);
					}
				}
			}
			else {
				HudScript.addNewMessage("No Item Selected!", 60, Color.red);
				GameAudio.playMagicFail();
			}
			inventorySelectedIndex = moveInventoryIndex(inventorySelectedIndex);
		}

		/*// timer
		if( PhotonNetwork.player.isMasterClient )
		{
			timer -= Time.deltaTime;
			if( Wizard.myWizard != null )
			{
				Wizard.myWizard.GetComponent<PlayerController>().networkedProperties["Time"] = timer;
				PhotonNetwork.player.SetCustomProperties(Wizard.myWizard.GetComponent<PlayerController>().GetNetworkedProperties());
			}

		}
		else
		{
			if( PhotonNetwork.masterClient != null )
			{
				if( PhotonNetwork.masterClient.customProperties.ContainsKey("Time") )
				{
					timer = (float) PhotonNetwork.masterClient.customProperties["Time"];
				}
				else
				{
					timer -= Time.deltaTime;
				}
			}
			else
			{
				timer -= Time.deltaTime;
			}
		}
		minutes = (int)timer/60;
		seconds = timer - (minutes * 60);

		if( seconds > 9 )
			RoundTimer.GetComponent<TextMesh>().text = minutes + ":" + seconds.ToString("0");
		else
		{
			RoundTimer.GetComponent<TextMesh>().text = minutes + ":" + "0" + seconds.ToString("0");
		}*/

		//WAVE TEXT
		if( PhotonNetwork.masterClient != null )
		{
			if( PhotonNetwork.masterClient.customProperties.ContainsKey("Wave") )
			{
				int tempWave = (int) PhotonNetwork.masterClient.customProperties["Wave"];
				wave = tempWave;
			}
			else
			{
				wave = -1;
			}

			int orbs = (int) PhotonNetwork.masterClient.customProperties["Orbs"];

			WaveText.GetComponent<TextMesh>().text = "Wave: " + wave;
			OrbText.GetComponent<TextMesh>().text = "Orbs: " + (5 - orbs) + " / 5";
		}
		
		// update score
		for(int i=0; i < PhotonNetwork.playerList.Length; ++i){
			PhotonPlayer p = PhotonNetwork.playerList[i];

			if(GLOBAL.myWizard != null && p == GLOBAL.myWizard.GetComponent<PhotonView>().owner) {
				if(p.customProperties.ContainsKey("Score")){
					int s = (int)p.customProperties["Score"];

					// don't show until first score
					if(s != 0) {
						scoreTxtMesh.GetComponent<MeshRenderer>().enabled = true;
						scoreTxtMesh.text = "Score: " + s.ToString();
					}
				}

				break;
			}
		}
	}

	public static void addNewMessage(string strmessage, int duration, Color c){

		//Don't enque message if it's already in the queue!
		foreach(Message m in messageQueue)
		{
			if(m.messageString == strmessage)
				return;
		}

		messageQueue.Enqueue(new Message(strmessage, duration, c));
	}

	//searches for next item in inventory and sets index to it.
	//This prevents a player from not having an item selected in their inventory.
	int moveInventoryIndex(int i)
	{
		if(GLOBAL.getInventoryCount() > i)
			return i;
		for(; i > -1; i--)
		{
			if(GLOBAL.getInventoryCount() > i)
				return i;
		}

		return 0;
	}
}
