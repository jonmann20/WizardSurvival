using UnityEngine;
using System.Collections;

public class IntroScript : MonoBehaviour {

	const float RATE_OF_MARCH = 1;
	public GameObject MarchingObjectPrefab;
	void Start () {
		for(int i = 0; i < 40; i++)
		{
			for(int j = 0; j < 50; j++)
			{
				Vector3 pos = new Vector3(i * 3 - 80, 1, j * 3 - 130);
				GameObject newMarcher = Instantiate(MarchingObjectPrefab, pos, Quaternion.identity) as GameObject;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = new Vector3(Random.Range(-100.0f, 50.0f), 1, -12 - Random.Range(0.0f, 5.0f));
		//GameObject newMarcher = Instantiate(MarchingObjectPrefab, pos, Quaternion.identity) as GameObject;

		if(transform.position.z > 10)
		{
			float diff = (-10 - transform.position.z) * 0.005f;
			transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + diff);
		}

		if(Input.GetKeyDown("space"))
		{
			Application.LoadLevel("NetworkSample");
		}
	}
}
