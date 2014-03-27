using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HudScript : MonoBehaviour {

	const float VOXEL_WIDTH = 0.2f;
	const float HEALTHBAR_X_OFFSET = -4.0f;
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

	//PERIL FLASH
	Light hudLight;

	public Shader toonShader;

	//INVENTORY
	const int INVENTORY_MAX = 4;
	const float INVENTORY_OFFSET_X = -4.0f;
	const float INVENTORY_OFFSET_Y = -2.0f;
	const float INVENTORY_OFFSET_Z = 5;
	const float INVENTORY_PANELS_X_SCALE = 0.3f;
	const float INVENTORY_PANELS_Y_SCALE = 0.1f;
	const float INVENTORY_PANELS_Z_SCALE = 0.3f;
	const float INVENTORY_PANELS_X_SEPARATION = 0.3f;

	// Use this for initialization
	void Awake () {
		hudCamera = GameObject.FindWithTag("HudCamera") as GameObject;
		hudLight = (GameObject.FindWithTag("HudLight") as GameObject).GetComponent<Light>();
		print(hudLight);
		Color greenColor = Color.green;
		Color redColor = Color.red;
		float fraction = 0.0f;

		for(int i = 0; i < NUMBER_OF_VOXELS; i++)
		{
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.layer = 9;
			cube.transform.parent = hudCamera.transform;
			cube.transform.localScale = new Vector3(VOXEL_WIDTH, VOXEL_WIDTH, VOXEL_WIDTH);
			cube.transform.position = new Vector3(i * VOXEL_WIDTH + 0.1f + HEALTHBAR_X_OFFSET, HEALTHBAR_Y_OFFSET, HEALTHBAR_Z_OFFSET);

			Material mat;
			mat = new Material(toonShader);
			mat.color = Color.Lerp(redColor, greenColor, fraction);
			fraction += 1.0f / (float)NUMBER_OF_VOXELS;
			cube.GetComponent<MeshRenderer>().material = mat;

			healthVoxels.Add(cube);
		}

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

			if(currentHealthIndex < i)
				healthVoxels[i].GetComponent<MeshRenderer>().enabled = false;
			else
				healthVoxels[i].GetComponent<MeshRenderer>().enabled = true;
		}

		//TEXT
		if(AbilityManagerScript.currentAbility != null)
		{
			AbilityNameText.GetComponent<TextMesh>().text = AbilityManagerScript.currentAbility.getAbilityName();
		}

		//WARNING FLASH
		if(GLOBAL.health < 40)
		{
			hudLight.intensity = Mathf.Abs(Mathf.Sin(sinCounter * 10)) * 5;
		}

		//INVENTORY
		for(int i = 0; i < GLOBAL.Inventory.Count; i++)
		{
			GameObject g = GLOBAL.Inventory[i];
			g.layer = 9;
			g.transform.parent = hudCamera.transform;
			g.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
			g.transform.localPosition = new Vector3(INVENTORY_OFFSET_X + i * (INVENTORY_PANELS_X_SCALE + INVENTORY_PANELS_X_SEPARATION),
			                                        INVENTORY_OFFSET_Y + 0.1f,
			                                        4);
		}
	}
}
