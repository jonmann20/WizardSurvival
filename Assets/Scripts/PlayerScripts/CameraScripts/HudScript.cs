using UnityEngine;
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
	GameObject hudCamera;

	//TEXT
    public GameObject AbilityNameText;
    //GameObject AbilityDescriptionText;
    //public Font font;
    //public GameObject AbilityNameText;
    public GameObject ScoreText;
	public GameObject RoundTimer;
	private float timer = 300f;
	private float seconds = 0;
	private int minutes = 0;

	//HealthBar
	public Texture healthBarTexture;

	//PERIL FLASH
	Light hudLight;

	public Shader toonShader;
	public Shader toonShaderLight;

	//INVENTORY
	const float INVENTORY_OFFSET_X = -3.75f;
	const float INVENTORY_OFFSET_Y = -2.0f;
	const float INVENTORY_OFFSET_Z = 5;
	const float INVENTORY_PANELS_X_SCALE = 0.3f;
	const float INVENTORY_PANELS_Y_SCALE = 0.1f;
	const float INVENTORY_PANELS_Z_SCALE = 0.3f;
	const float INVENTORY_PANELS_X_SEPARATION = 0.3f;
	public float target = 1.0f;
	int inventorySelectedIndex = -1;

	//INVENTORY TEXTS
	GameObject FirstItemQuantityText;
	GameObject SecondItemQuantityText;
	GameObject ThirdItemQuantityText;

	//MESSAGE TEXT
	static GameObject MessageText;
	static string messageString = "";
	static int messageLife = 0;

	//Leaderboard Button
	const float LEADERBOARD_X = 0.75f;
	const float LEADERBOARD_Y = 0.85f;
	const float LEADERBOARD_WIDTH = 0.18f;
	const float LEADERBOARD_HEIGHT = 0.07f;
	
	// Use this for initialization
	void Awake () {
		healthPerVoxel = 100.0f / (float)NUMBER_OF_VOXELS;
		hudCamera = GameObject.FindWithTag("HudCamera") as GameObject;
		hudLight = (GameObject.FindWithTag("HudLight") as GameObject).GetComponent<Light>();

		FirstItemQuantityText = GameObject.Find("FirstItemQuantityText") as GameObject;
		SecondItemQuantityText = GameObject.Find("SecondItemQuantityText") as GameObject;
		ThirdItemQuantityText = GameObject.Find("ThirdItemQuantityText") as GameObject;

		MessageText = GameObject.Find("MessageText") as GameObject;
		ScoreText.transform.localPosition = new Vector3(1.849f, -1.433f, 3.12f);

		//HEALTH
		Color greenColor = Color.green;
		Color redColor = Color.red;
		float fraction = 0.0f;
		
		for(int i = 0; i < NUMBER_OF_VOXELS; i++)
		{
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.layer = 9;
			cube.transform.parent = hudCamera.transform;
			cube.transform.position = new Vector3(i * VOXEL_WIDTH + 0.1f + HEALTHBAR_X_OFFSET, HEALTHBAR_Y_OFFSET, HEALTHBAR_Z_OFFSET);
			Bouncy b = cube.AddComponent<Bouncy>() as Bouncy;
			b.target = 0.2f;

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

		//INVENTORY
		for(int i = 0; i < GLOBAL.maxInventory; i++)
		{
			GameObject c = GameObject.CreatePrimitive(PrimitiveType.Cube);
			c.layer = 9;
			c.transform.parent = hudCamera.transform;
			c.transform.localScale = new Vector3(INVENTORY_PANELS_X_SCALE, INVENTORY_PANELS_Y_SCALE, INVENTORY_PANELS_Z_SCALE);
			c.transform.localPosition = new Vector3(INVENTORY_OFFSET_X + i * (INVENTORY_PANELS_X_SCALE + INVENTORY_PANELS_X_SEPARATION),
			                                   INVENTORY_OFFSET_Y,
			                                   4);

			Material mat;
			mat = new Material(toonShader);
			mat.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
			c.GetComponent<MeshRenderer>().material = mat;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		sinCounter += HEALTHBAR_FLOAT_RATE;
		if(sinCounter > Mathf.PI * healthVoxels.Count * 2)
			sinCounter = 0;



		for(int i = 0; i < healthVoxels.Count; i++)
		{
			float bonusHeight = 0;

			bonusHeight = Mathf.Sin(sinCounter * i) * HEALTHBAR_FLOAT_ALTITUDE;
			healthVoxels[i].transform.localPosition = new Vector3(i * (VOXEL_WIDTH + 0.05f) + HEALTHBAR_X_OFFSET, HEALTHBAR_Y_OFFSET + bonusHeight, HEALTHBAR_Z_OFFSET);

			//ADJUST FOR CURRENT HEALTH
			Bouncy b = healthVoxels[i].GetComponent<Bouncy>() as Bouncy;

			if(GLOBAL.health < (i + 1) * healthPerVoxel)
				b.target = 0.1f;
			else
				b.target = 0.2f;
		}

        ////TEXT
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

		//INVENTORY
		int numInventoryItems = GLOBAL.getInventoryCount();
		for(int i = 0; i < numInventoryItems; i++)
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
			g.transform.localPosition = new Vector3(INVENTORY_OFFSET_X + i * (INVENTORY_PANELS_X_SCALE + INVENTORY_PANELS_X_SEPARATION),
			                                        INVENTORY_OFFSET_Y + 0.1f,
			                                        4);

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
		int numInventoryItems = GLOBAL.getInventoryCount();

		if(ctrl_invL.WasPressed){
			if(inventorySelectedIndex > -1){
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

		if(ctrl_special.WasPressed){
			if(inventorySelectedIndex > -1 && inventorySelectedIndex < numInventoryItems){
				GameAudio.playInvSelect();

				GLOBAL.useInventoryItemAt(inventorySelectedIndex);

			}
			else {
				GameAudio.playMagicFail();
			}
		}

		// timer
		timer -= Time.deltaTime;
		minutes = (int)timer/60;
		seconds = timer - (minutes * 60);

		//RoundTimer.GetComponent<TextMesh>().text = minutes + ":" + seconds.ToString("00");
		//MESSAGE TEXT

		MessageText.GetComponent<TextMesh>().text = messageString;
		if(messageLife > 0)
		{
			messageLife --;
			MessageText.transform.localPosition += (new Vector3(0.1f, -1.50f, 3.13f) - MessageText.transform.localPosition) * 0.1f;
		}
		else
		{
			MessageText.transform.localPosition += (new Vector3(0.1f, -2.05f, 3.13f) - MessageText.transform.localPosition) * 0.1f;
		}
	}

    void OnGUI(){
		//Scores (TEMP)
		
		int offset = 20;
		//GUI.Label( new Rect( Screen.width/2 , Screen.height/2, 300,25 ), "Number of Players");
		int teamScore = 0;
		for( int i = 0; i < PhotonNetwork.playerList.Length; i++ )
		{
			//Don't list duplicate information about the current player.
			if(Wizard.myWizard != null)
			if(PhotonNetwork.playerList[i] == Wizard.myWizard.GetComponent<PhotonView>().owner)
			{
				teamScore += (int)PhotonNetwork.playerList[i].customProperties["Score"];
				continue;
			}

			//Object key = "String";
			if( PhotonNetwork.playerList[i].customProperties.ContainsKey("Score") )
			{
				int tempScore = (int) PhotonNetwork.playerList[i].customProperties["Score"];
				teamScore += tempScore;
				if( PhotonNetwork.playerList[i].customProperties.ContainsKey("Ability") )
				{
					string abilityName = (string) PhotonNetwork.playerList[i].customProperties["Ability"];
					GUI.Label(new Rect( Screen.width * .05f, Screen.height * .1f + (offset * (i)), (Screen.width * .16f), 25f), "Player " + PhotonNetwork.playerList[i].ID + ": " +  abilityName + " Score: " + tempScore.ToString() );
				}
				else
				{
					GUI.Label(new Rect( Screen.width * .05f, Screen.height * .1f + (offset * (i)), (Screen.width * .16f), 25f), "Player " + PhotonNetwork.playerList[i].ID + ": " + tempScore.ToString() );
				}
			}
			else
			{
				GUI.Label(new Rect( Screen.width * .05f, Screen.height * .1f + (offset * (i) ), (Screen.width * .16f), Screen.height * .03f), "Player " + PhotonNetwork.playerList[i].ID + ": does not have score property" );
			}
			if( PhotonNetwork.playerList[i].customProperties.ContainsKey("Health"))
			{
				float tempHealth = (int) PhotonNetwork.playerList[i].customProperties["Health"];
				//print (tempHealth);
				tempHealth = tempHealth/100;
				GUI.DrawTexture(new Rect( Screen.width * .05f, 5 + Screen.height * .1f + (offset * i) + (Screen.height * .022f), (Screen.width * .16f) * tempHealth, Screen.height * .01f), healthBarTexture);
			}



			//GUI.Label(new Rect( Screen.width * .8f, Screen.height * .8f + (offset * i), 300f, 25f), "Player " + PhotonNetwork.otherPlayers[i].ID + ": " + tempScore );
		}

		ScoreText.GetComponent<TextMesh>().text = "Score: " + teamScore.ToString();
    }

	public static void setNewMessage(string strmessage, int duration, Color c)
	{
		messageLife = duration;
		messageString = strmessage;
		MessageText.renderer.material.color = c;
	}
}
