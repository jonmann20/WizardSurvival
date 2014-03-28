using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HudScript : MonoBehaviour {

	const float VOXEL_WIDTH = 0.2f;
    float HEALTHBAR_X_OFFSET = -0.75f;
	const float HEALTHBAR_Y_OFFSET = 2.0f;
	const float HEALTHBAR_Z_OFFSET = 4;
	const float HEALTHBAR_FLOAT_RATE = 0.01f;
	const float HEALTHBAR_FLOAT_ALTITUDE = 0.05f;

	const int NUMBER_OF_VOXELS = 10;

	float sinCounter = 0.0f;

	List<GameObject> healthVoxels = new List<GameObject>();
	GameObject hudCamera;

	//TEXT
    //GameObject AbilityNameText;
    //GameObject AbilityDescriptionText;
    //public Font font;

	//PERIL FLASH
	Light hudLight;

	// Use this for initialization
	void Awake () {
		HEALTHBAR_X_OFFSET = -(NUMBER_OF_VOXELS * VOXEL_WIDTH) * 0.5f;

        HEALTHBAR_X_OFFSET += 2.8f;

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
			cube.transform.localScale = new Vector3(VOXEL_WIDTH, VOXEL_WIDTH, VOXEL_WIDTH);
			cube.transform.position = new Vector3(i * VOXEL_WIDTH + HEALTHBAR_X_OFFSET, HEALTHBAR_Y_OFFSET, HEALTHBAR_Z_OFFSET);

			Material mat;
			mat = new Material(Shader.Find("Diffuse"));
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
	
	// Update is called once per frame
	void Update () {
		sinCounter += HEALTHBAR_FLOAT_RATE;
		if(sinCounter > Mathf.PI * healthVoxels.Count * 2)
			sinCounter = 0;

		float currentHealthIndex = (float)GLOBAL.health / (float)NUMBER_OF_VOXELS;

		for(int i = 0; i < healthVoxels.Count; i++)
		{
			float bonusHeight = 0;

			bonusHeight = Mathf.Sin(sinCounter * i) * HEALTHBAR_FLOAT_ALTITUDE;
			healthVoxels[i].transform.localPosition = new Vector3(i * VOXEL_WIDTH + HEALTHBAR_X_OFFSET, HEALTHBAR_Y_OFFSET + bonusHeight, HEALTHBAR_Z_OFFSET);

			//ADJUST FOR CURRENT HEALTH

			if(currentHealthIndex < i)
				healthVoxels[i].GetComponent<MeshRenderer>().enabled = false;
			else
				healthVoxels[i].GetComponent<MeshRenderer>().enabled = true;
		}

        ////TEXT
        //if(AbilityManagerScript.currentAbility != null)
        //{
        //    AbilityNameText.GetComponent<TextMesh>().text = AbilityManagerScript.currentAbility.getAbilityName();
        //}

		//WARNING FLASH
		if(GLOBAL.health < 40)
		{
			hudLight.intensity = Mathf.Abs(Mathf.Sin(sinCounter * 10)) * 5;
		}
	}

    void OnGUI(){
        EZGUI.init();

        EZGUI.placeTxt("Calzone", 50, EZGUI.FULLW - 300, 190);
    }
}
