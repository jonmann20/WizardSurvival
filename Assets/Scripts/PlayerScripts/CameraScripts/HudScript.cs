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

	const int NUMBER_OF_VOXELS = 10;

	float sinCounter = 0.0f;

	List<GameObject> healthVoxels = new List<GameObject>();
	GameObject hudCamera;

	//TEXT
    public GameObject AbilityNameText;
    //GameObject AbilityDescriptionText;
    //public Font font;
    //public GameObject AbilityNameText;
    public GameObject ScoreText;

	//HealthBar
	public Texture healthBarTexture;

	//PERIL FLASH
	Light hudLight;

	public Shader toonShader;
	public Shader toonShaderLight;

	//INVENTORY
	const int INVENTORY_MAX = 4;
	const float INVENTORY_OFFSET_X = -3.75f;
	const float INVENTORY_OFFSET_Y = -2.0f;
	const float INVENTORY_OFFSET_Z = 5;
	const float INVENTORY_PANELS_X_SCALE = 0.3f;
	const float INVENTORY_PANELS_Y_SCALE = 0.1f;
	const float INVENTORY_PANELS_Z_SCALE = 0.3f;
	const float INVENTORY_PANELS_X_SEPARATION = 0.3f;
	public float target = 1.0f;
	int inventorySelectedIndex = -1;





	//Leaderboard Button
	const float LEADERBOARD_X = 0.75f;
	const float LEADERBOARD_Y = 0.85f;
	const float LEADERBOARD_WIDTH = 0.18f;
	const float LEADERBOARD_HEIGHT = 0.07f;
	
	// Use this for initialization
	void Awake () {

		hudCamera = GameObject.FindWithTag("HudCamera") as GameObject;
		hudLight = (GameObject.FindWithTag("HudLight") as GameObject).GetComponent<Light>();
		//print(hudLight);
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

			Material mat;
			mat = new Material(toonShaderLight);
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
		for(int i = 0; i < INVENTORY_MAX; i++)
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

		float currentHealthIndex = (float)GLOBAL.health / (float)NUMBER_OF_VOXELS;

		for(int i = 0; i < healthVoxels.Count; i++)
		{
			float bonusHeight = 0;

			bonusHeight = Mathf.Sin(sinCounter * i) * HEALTHBAR_FLOAT_ALTITUDE;
			healthVoxels[i].transform.localPosition = new Vector3(i * (VOXEL_WIDTH + 0.05f) + HEALTHBAR_X_OFFSET, HEALTHBAR_Y_OFFSET + bonusHeight, HEALTHBAR_Z_OFFSET);

			//ADJUST FOR CURRENT HEALTH
			Bouncy b = healthVoxels[i].GetComponent<Bouncy>() as Bouncy;


			if(currentHealthIndex < i)
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
		if(GLOBAL.health < 40)
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
		}
	}

	void Update()
	{
		InputDevice device = InputManager.ActiveDevice;
		InputControl ctrl_DPadL = device.GetControl(InputControlType.DPadLeft);
		InputControl ctrl_DPadR = device.GetControl(InputControlType.DPadRight);
		InputControl ctrl_RightTrigger = device.GetControl(InputControlType.RightTrigger);

		int numInventoryItems = GLOBAL.getInventoryCount();
		if(((ctrl_DPadR.IsPressed && ctrl_DPadR.LastValue == 0) || Input.GetKeyDown("right")) && inventorySelectedIndex < numInventoryItems - 1){
			++inventorySelectedIndex;
		}

		if(((ctrl_DPadL.IsPressed && ctrl_DPadL.LastValue == 0) || Input.GetKeyDown("left")) && inventorySelectedIndex > -1){
			--inventorySelectedIndex;
		}

		if((ctrl_RightTrigger.IsPressed && ctrl_RightTrigger.LastValue == 0) || Input.GetKeyDown("return")){
			if(inventorySelectedIndex > -1 && inventorySelectedIndex < numInventoryItems){
				GLOBAL.useInventoryItemAt(inventorySelectedIndex);
				--inventorySelectedIndex;
			}
		}
	}

    void OnGUI(){

		//Temporary instructions
		GUI.Label(new Rect(10, Screen.height * 0.72f, 100, 100), "Arrow keys for inventory. Enter to use.");

		//Scores (TEMP)
		
		int offset = 20;
		//GUI.Label( new Rect( Screen.width/2 , Screen.height/2, 300,25 ), "Number of Players");
		int teamScore = 0;
		for( int i = 0; i < PhotonNetwork.playerList.Length; i++ )
		{
			//Object key = "String";
			if( PhotonNetwork.playerList[i].customProperties.ContainsKey("Score") )
			{
				int tempScore = (int) PhotonNetwork.playerList[i].customProperties["Score"];
				teamScore += tempScore;
				GUI.Label(new Rect( Screen.width * .8f, Screen.height * .72f + (offset * (i)), (Screen.width * .16f), 25f), "Player " + PhotonNetwork.playerList[i].ID + ": " + teamScore.ToString() );
			}
			else
			{
				GUI.Label(new Rect( Screen.width * .8f, Screen.height * .72f + (offset * (i) ), (Screen.width * .16f), Screen.height * .03f), "Player " + PhotonNetwork.playerList[i].ID + ": does not have score property" );
			}
			if( PhotonNetwork.playerList[i].customProperties.ContainsKey("Health") )
			{
				float tempHealth = (int) PhotonNetwork.playerList[i].customProperties["Health"];
				print (tempHealth);
				tempHealth = tempHealth/100;
				GUI.DrawTexture(new Rect( Screen.width * .8f, Screen.height * .72f + (offset * i) + (Screen.height * .022f), (Screen.width * .16f) * tempHealth, Screen.height * .01f), healthBarTexture);
			}


			//GUI.Label(new Rect( Screen.width * .8f, Screen.height * .8f + (offset * i), 300f, 25f), "Player " + PhotonNetwork.otherPlayers[i].ID + ": " + tempScore );
		}
		this.gameObject.GetComponent<HudScript>().ScoreText.GetComponent<TextMesh>().text = "Score: " + teamScore.ToString();

    }
}
