using UnityEngine;
using System.Collections;

public class Ventilation : MonoBehaviour
{
    #region Public Variables:
    public float rotateSpeed = 100.0f;
    #endregion

    #region Private Variables:
    private Transform myTransform;
    #endregion
	
    #region Unity Functions:
    // Use this for initialization
	void Start () 
    {
        Initialize();
	}
	
	// Update is called once per frame
	void Update () 
    {
        Rotate();
	}
    #endregion

    #region Ventilation Functions:
    //used to initialize variables
    private void Initialize()
    {
        myTransform = transform;
    }

    //this function is responsible of rotating the bladeRazor
    //based on RotationAxis and rotationSpeed
    private void Rotate()
    {
        myTransform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
    }
	#endregion
}
