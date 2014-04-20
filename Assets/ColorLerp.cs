using UnityEngine;
using System.Collections;

public class ColorLerp : MonoBehaviour {

	public const float RATE_OF_CHANGE = 0.025f;
	float fraction = 0.0f;
	Color desiredColor;
	Color currentColor;

	// Use this for initialization
	void Start () {
		currentColor = new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f));
		desiredColor = new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f));
	}
	
	// Update is called once per frame
	void Update () {
		fraction += RATE_OF_CHANGE;
		renderer.material.color = Color.Lerp(currentColor, desiredColor, fraction);

		if(fraction >= 1.0)
			findNewColor();
	}

	void findNewColor()
	{
		currentColor = renderer.material.color;
		desiredColor = new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f));
		fraction = 0;
	}
}
