using UnityEngine;
using System.Collections;

public class HolyLightScript : MonoBehaviour {


	public static HolyLightScript that;

	public GameObject OrbOfHopeDecoyPrefab;
	float lightTimer = 0;

	int sinCounter = 0;
	// Use this for initialization
	void Start () {
		that = this;
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		Color[] colors = new Color[vertices.Length];
		int i = 0;
		while (i < vertices.Length) {
			colors[i] = Color.Lerp(Color.red, Color.green, vertices[i].y);
			i++;
		}
		mesh.colors = colors;
	}
	
	// Update is called once per frame
	void Update () {
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		Vector3[] normals = mesh.normals;
		int i = 0;
		while (i < vertices.Length) {

			vertices[i] += normals[i] * Mathf.Sin(Time.time * 3 + vertices[i].y * 2f) * 0.002f;
			i++;
		}
	
		mesh.vertices = vertices;


		if(lightTimer > 0)
		{
			renderer.material.color = new Color(1, 1, 1, 0.2f);
			lightTimer -= 1;
		}
		else
			renderer.material.color = new Color(1, 1, 1, 0.05f);
	}

	public static void spark()
	{
		that.lightTimer = 240;
		float randVal = Random.Range(0, 2*Mathf.PI);
		float xval = Mathf.Sin(randVal);
		float zval = Mathf.Cos(randVal);
		
		Vector3 randVec = new Vector3(xval, 2, zval);
		
		Vector3 pos = randVec * 5 + new Vector3(that.transform.position.x, 0, that.transform.position.z);
		Instantiate(that.OrbOfHopeDecoyPrefab, pos, Quaternion.identity);
	}
}
