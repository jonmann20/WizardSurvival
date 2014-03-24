using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HudScript : MonoBehaviour {

	const float VOXEL_WIDTH = 0.2f;
	float HEALTHBAR_X_OFFSET = -0.75f;
	const float HEALTHBAR_Y_OFFSET = 2.0f;
	const float HEALTHBAR_Z_OFFSET = 4;
	const float HEALTHBAR_FLOAT_RATE = 0.05f;
	const float HEALTHBAR_FLOAT_ALTITUDE = 0.1f;

	const int NUMBER_OF_VOXELS = 5;

	float sinCounter = 0.0f;

	List<GameObject> healthVoxels = new List<GameObject>();
	GameObject mainCamera;

	// Use this for initialization
	void Awake () {
		HEALTHBAR_X_OFFSET = -(NUMBER_OF_VOXELS * VOXEL_WIDTH) * 0.5f;
		mainCamera = GameObject.FindWithTag("MainCamera") as GameObject;

		Color greenColor = Color.green;
		Color redColor = Color.red;
		float fraction = 0.0f;

		for(int i = 0; i < NUMBER_OF_VOXELS; i++)
		{
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.transform.parent = mainCamera.transform;
			cube.transform.localScale = new Vector3(VOXEL_WIDTH, VOXEL_WIDTH, VOXEL_WIDTH);
			cube.transform.position = new Vector3(i * VOXEL_WIDTH + HEALTHBAR_X_OFFSET, HEALTHBAR_Y_OFFSET, HEALTHBAR_Z_OFFSET);

			Material mat;
			mat = new Material(Shader.Find("Diffuse"));
			mat.color = Color.Lerp(redColor, greenColor, fraction);
			fraction += 1.0f / (float)NUMBER_OF_VOXELS;
			cube.GetComponent<MeshRenderer>().material = mat;

			healthVoxels.Add(cube);
		}
	}
	
	// Update is called once per frame
	void Update () {
		sinCounter += HEALTHBAR_FLOAT_RATE;
		if(sinCounter > Mathf.PI * healthVoxels.Count * 2)
			sinCounter = 0;

		for(int i = 0; i < healthVoxels.Count; i++)
		{
			float bonusHeight = 0;

			//if(sinCounter >= Mathf.PI * i && sinCounter <= Mathf.PI * (i+1))
				bonusHeight = Mathf.Sin(sinCounter + i * 0.1f) * HEALTHBAR_FLOAT_ALTITUDE;

			healthVoxels[i].transform.localPosition = new Vector3(i * VOXEL_WIDTH + HEALTHBAR_X_OFFSET, HEALTHBAR_Y_OFFSET + bonusHeight, HEALTHBAR_Z_OFFSET);
		}
	}
}
