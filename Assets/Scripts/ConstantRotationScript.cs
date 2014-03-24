using UnityEngine;
using System.Collections;

public class ConstantRotationScript : MonoBehaviour {

	public float drotationX = 0;
	public float drotationY = 0;
	public float drotationZ = 0;

	void Update () {
		transform.Rotate(drotationX, drotationY, drotationZ);
	}
}
