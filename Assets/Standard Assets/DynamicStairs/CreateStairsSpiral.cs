using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class RailingSetup {

	public bool createRailing = true;

	public bool railingCollider = false;
	public bool railingRigidbody = false;
	public bool railingGravity = false;
	public bool railingKinematic = true;

	public bool handleCollider = true;
	public bool handleRigidbody = false;
	public bool handleGravity = false;
	public bool handleKinematic = true;

	public float railHeight = 2f;
	[Range (0f, 1f)]
	public float railingScaleSpeed = 0.2f;
	public bool railingScaleInstant = false;

	public Material railingMaterial = null;
	public Material railHandleMaterial = null;
}

[Serializable]
public class PanelSetup {

	public bool createPanel = true;
	public bool createPanelFrontMesh = true;

	public bool panelCollider = false;
	public bool panelRigidbody = false;
	public bool panelGravity = false;
	public bool panelKinematic = true;

	public Vector3 panelSize = new Vector3 (6f, 0.7f, 0.1f);

	public Material stairPanelMaterial = null;
	public Material panelFrontMeshMaterial = null;
}

[Serializable]
public class StairsSetup {

	public bool stairsCollider = false;
	public bool angledStairCollider = true;

	public bool stairsRigidbody = false;
	public bool stairsGravity = false;
	public bool stairsKinematic = true;

	public int amountOfStairs = 125;
	
	public bool stairsCCW = true;
	public bool stairsGoUp = true;

	public float startPositionAngle = 180f;
	public float currentAngle = 180f;
	
	public int totalAngle = 1300;

	public float moveStairInsidePillar = 0.3f;

	public Vector3 stairSize = new Vector3 (6f, 0.3f, 1.5f);
	public float VerticalRangeBetween = 0.4f;

	[Range (0f, 1f)] 
	public float xScaleSpeed = 0.2f;
	public bool xScaleInstant = false;
	[Range (0f, 0.5f)]
	public float yScaleSpeed = 0.005f;
	public bool yScaleInstant = false;
	[Range (0f, 1f)]
	public float zScaleSpeed = 0.2f;
	public bool zScaleInstant = false;

	public Material stairMaterial = null;
}


public class CreateStairsSpiral : MonoBehaviour {

	public bool createStaircase = false;

	public bool pauseStairCreation = false;
	private bool stairCreationPaused = false;

	public bool showCenterPillar = true;
	private Transform centerPillar;

	public StairsSetup StairsSetup;
	private Vector3 startPosition = new Vector3 (0f, 1f, 0f);
	private bool creatingStairs = false;
	private GameObject stairs;
	private GameObject stairMoveCollidersStart;
	private GameObject stairMoveCollidersEnd;
	private int amountOfColliders;
	private float nextStepAngle;
	private float zScaleSpeedBeforePause;
	private bool zScaleInstantBeforePause;

	public PanelSetup PanelSetup;
	private GameObject stairPanels;
	private GameObject panelFrontMeshObj;
	private Vector2 panelFrontMesh;

	public RailingSetup RailingSetup;
	private Vector3 railScale;
	private GameObject railing;
	private GameObject railingHandle;
	private Vector3 lastRailing;
	private float xRailingPosition;
	private float zRailingPosition;
	private float handleRotation;
	private float lastHandleRotation;
	private bool handleRotationSet = true;
	private float handleDistance;

	public bool showGizmos = true;
	public Color gizmoColor = Color.yellow;
	public float gizmoSize = 6;


	void Start (){ 

		if (!StairsSetup.stairsGoUp){

			StairsSetup.stairSize = new Vector3 (StairsSetup.stairSize.x, -StairsSetup.stairSize.y, StairsSetup.stairSize.z);
			StairsSetup.VerticalRangeBetween = -StairsSetup.VerticalRangeBetween;
		}

		StairsSetup.currentAngle = StairsSetup.startPositionAngle;

		railScale = new Vector3 (0.2f, RailingSetup.railHeight, 0.2f);

		zScaleSpeedBeforePause = StairsSetup.zScaleSpeed;
		
		centerPillar = this.gameObject.transform.Find ("CenterPillar");

		lastRailing = new Vector3(startPosition.x, startPosition.y, startPosition.z);

		if (!showCenterPillar) {
		
			Destroy(transform.GetComponent<BoxCollider> ());
			Destroy(centerPillar.GetComponent<CapsuleCollider> ());
			centerPillar.GetComponent<MeshRenderer>().enabled = false;
			Destroy(centerPillar.GetComponent<Rigidbody> ());
		}

		if (StairsSetup.VerticalRangeBetween == 0 && StairsSetup.stairSize.y < 0){

			StairsSetup.VerticalRangeBetween = -0.001f;

		} else if (StairsSetup.VerticalRangeBetween == 0 && StairsSetup.stairSize.y > 0){

			StairsSetup.VerticalRangeBetween = 0.001f;

		} else if (StairsSetup.VerticalRangeBetween < 0 && StairsSetup.stairSize.y == 0){
			
			StairsSetup.stairSize.y = -0.001f;
			
		} else if (StairsSetup.VerticalRangeBetween > 0 && StairsSetup.stairSize.y == 0){
			
			StairsSetup.stairSize.y = 0.001f;
			
		}  else if (StairsSetup.VerticalRangeBetween == 0 && StairsSetup.stairSize.y == 0){
			
			StairsSetup.stairSize.y = 0.001f;
			StairsSetup.VerticalRangeBetween = 0.001f;
			
		}
	}
	
	void Update () {

		// THIS WILL TAKE EITHER "Stairs" INPUT OR BOOLEAN "createStaircase" SET TO TRUE; TO CREATE THE STAIRS AND EVERYTHING ASSOSIATED
		/*
		if (Input.GetButtonDown("Stairs") || createStaircase){
			
			if (!creatingStairs){
				
				StartCoroutine(CreateStairsRoutine());
			}
		}
*/
		if (pauseStairCreation){

			if (!stairCreationPaused){

				zScaleSpeedBeforePause = StairsSetup.zScaleSpeed;
				zScaleInstantBeforePause = StairsSetup.zScaleInstant;
				
				StairsSetup.zScaleInstant = false;
				
				StairsSetup.zScaleSpeed = 0f;

				stairCreationPaused = true;
			}
			
		} else if (!pauseStairCreation && (StairsSetup.zScaleSpeed <= 0f)){
			
			StairsSetup.zScaleSpeed = zScaleSpeedBeforePause;
			StairsSetup.zScaleInstant = zScaleInstantBeforePause;

			stairCreationPaused = false;
		}

	}
	
	IEnumerator CreateStairsRoutine(){
		
		creatingStairs = true;
		
		int stairNr = 0;
		float stairsApartVertical = 0.0f;

		amountOfColliders = StairsSetup.amountOfStairs - 1;

		// MAIN LOOP WHERE YOU CREATE EVERYTHING YOU SET
		while (StairsSetup.amountOfStairs > stairNr){

			float stairAngle = StairsSetup.totalAngle / StairsSetup.amountOfStairs;
			
			if (StairsSetup.stairsCCW){
				
				StairsSetup.currentAngle = StairsSetup.currentAngle - stairAngle;
				
			} else {
				
				StairsSetup.currentAngle = StairsSetup.currentAngle + stairAngle;
			}
			
			Vector3 startScaleSmall = new Vector3 (0.001f, 0.001f, 0.001f);
			
			stairs = GameObject.CreatePrimitive(PrimitiveType.Cube);

			if (StairsSetup.stairMaterial != null){

				stairs.renderer.material = StairsSetup.stairMaterial;
			}
			
			if (StairsSetup.stairsRigidbody){
				
				stairs.AddComponent<Rigidbody>();
				stairs.rigidbody.angularDrag = 0f;
				
				if (StairsSetup.stairsGravity){
					
					stairs.rigidbody.useGravity = true;
					
				} else if (!StairsSetup.stairsGravity){
					
					stairs.rigidbody.useGravity = false;
				}
				
				if (StairsSetup.stairsKinematic){

					stairs.rigidbody.isKinematic = true;
					
				} else if (!StairsSetup.stairsKinematic){
					
					stairs.rigidbody.isKinematic = false;
				}
			}
			
			GameObject stairPivotObject = new GameObject ("stairPivot");
			
			Transform stair = stairs.GetComponent<Transform>();

			Transform stairPivot = stairPivotObject.GetComponent<Transform>();
			
			stairPivot.parent = centerPillar;

			float centerPillarRadius = centerPillar.localScale.x / 2 - StairsSetup.moveStairInsidePillar;

			float xStairPosition = centerPillar.parent.position.x - Mathf.Cos(StairsSetup.currentAngle * Mathf.Deg2Rad) * centerPillarRadius;

			float zStairPosition = centerPillar.parent.position.z + Mathf.Sin(StairsSetup.currentAngle * Mathf.Deg2Rad) * centerPillarRadius;


			// For vertex snap
			stairPivot.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
			stairs.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);

			stairs.transform.position = new Vector3 (xStairPosition, startPosition.y + stairsApartVertical + StairsSetup.stairSize.y * stairNr + centerPillar.parent.position.y, zStairPosition);

			// POSITION FOR THE STAIR
			if (StairsSetup.stairsCCW){
				
				stairPivot.transform.position = new Vector3 (stairs.transform.position.x + (stairs.transform.localScale.x / 2), stairs.transform.position.y + (stairs.transform.localScale.y / 2), stairs.transform.position.z + (stairs.transform.localScale.z / 2));
				
			} else {
				
				stairPivot.transform.position = new Vector3 (stairs.transform.position.x + (stairs.transform.localScale.x / 2), stairs.transform.position.y + (stairs.transform.localScale.y / 2), stairs.transform.position.z - (stairs.transform.localScale.z / 2));
			}

			stair.parent = stairPivot;

			stairPivot.transform.Rotate (0f, StairsSetup.currentAngle, 0f);
			
			float addXScale = 0f;
			float addYScale = 0f;
			float addZScale = 0f;

			if (StairsSetup.xScaleSpeed > StairsSetup.stairSize.x){

				StairsSetup.xScaleSpeed = StairsSetup.stairSize.x;
			}

			if (StairsSetup.yScaleSpeed > StairsSetup.stairSize.y){
				
				StairsSetup.yScaleSpeed = StairsSetup.stairSize.y;
			}

			if (StairsSetup.zScaleSpeed > StairsSetup.stairSize.z){
				
				StairsSetup.zScaleSpeed = StairsSetup.stairSize.z;
			}

			// CREATE STAIRS WITH ANIMATION OR WITHOUT IT
			if (StairsSetup.stairSize.y + StairsSetup.VerticalRangeBetween < 0){

				if (!StairsSetup.zScaleInstant){
					
					while (Mathf.Abs(stairPivot.transform.localScale.z) <= Mathf.Abs(StairsSetup.stairSize.z)){

						addZScale += StairsSetup.zScaleSpeed;
						
						stairPivot.transform.localScale = new Vector3 (startScaleSmall.x, startScaleSmall.y, startScaleSmall.z - addZScale);
						
						yield return 0;
					}
					
				} else if (StairsSetup.zScaleInstant) {
					
					stairPivot.transform.localScale = new Vector3 (startScaleSmall.x, startScaleSmall.y, -StairsSetup.stairSize.z);
					
					yield return 0;
				}
				
			} else if (StairsSetup.stairSize.y + StairsSetup.VerticalRangeBetween > 0){

				if (!StairsSetup.zScaleInstant){
					
					while (Mathf.Abs(stairPivot.transform.localScale.z) <= Mathf.Abs(StairsSetup.stairSize.z)){

						addZScale += StairsSetup.zScaleSpeed;
						
						stairPivot.transform.localScale = new Vector3 (startScaleSmall.x, startScaleSmall.y, startScaleSmall.z + addZScale);
						
						yield return 0;
					}
					
				} else if (StairsSetup.zScaleInstant) {
					
					stairPivot.transform.localScale = new Vector3 (startScaleSmall.x, startScaleSmall.y, StairsSetup.stairSize.z);
					
					yield return 0;
				}
				
			}
			
			if (!StairsSetup.yScaleInstant){

				if (StairsSetup.xScaleInstant || StairsSetup.xScaleSpeed == StairsSetup.stairSize.x){

					while (Mathf.Abs(stairPivot.transform.localScale.y) <= Mathf.Abs(StairsSetup.stairSize.y)){
						
						addYScale += StairsSetup.yScaleSpeed;
						
						stairPivot.transform.localScale = new Vector3 (StairsSetup.stairSize.x, startScaleSmall.y + addYScale, stairPivot.transform.localScale.z);
						
						yield return 0;
					}

				} else {

					while (Mathf.Abs(stairPivot.transform.localScale.y) <= Mathf.Abs(StairsSetup.stairSize.y)){
						
						addYScale += StairsSetup.yScaleSpeed;
						
						stairPivot.transform.localScale = new Vector3 (startScaleSmall.x, startScaleSmall.y + addYScale, stairPivot.transform.localScale.z);
						
						yield return 0;
					}
				}
				
			} else if (StairsSetup.yScaleInstant) {

				stairPivot.transform.localScale = new Vector3 (startScaleSmall.x, StairsSetup.stairSize.y, stairPivot.transform.localScale.z);
				
				yield return 0;
			}

			if (!StairsSetup.xScaleInstant){
				
				while (Mathf.Abs(stairPivot.transform.localScale.x) <= Mathf.Abs(StairsSetup.stairSize.x)){

					addXScale += StairsSetup.xScaleSpeed;

					stairPivot.transform.localScale = new Vector3 (startScaleSmall.x + addXScale, stairPivot.transform.localScale.y, stairPivot.transform.localScale.z);
					
					yield return 0;
				}
				
			} else if (StairsSetup.xScaleInstant) {

				if (StairsSetup.yScaleInstant && StairsSetup.xScaleInstant){
					
					stairPivot.transform.localScale = new Vector3 (StairsSetup.stairSize.x, stairPivot.transform.localScale.y, stairPivot.transform.localScale.z);
				}

				yield return 0;
			}

			// CREATE FRONTPANELS FOR STAIRS
			if (PanelSetup.createPanel){
					
				stairPanels = GameObject.CreatePrimitive(PrimitiveType.Cube);

				stairPanels.transform.localScale = new Vector3 (0.01f, 0.01f, 0.01f);
				
				if (!PanelSetup.panelCollider){
					
					Destroy(stairPanels.gameObject.GetComponent<BoxCollider>());
				}
				
				if (PanelSetup.panelRigidbody){
					
					stairPanels.AddComponent<Rigidbody>();
					stairPanels.rigidbody.angularDrag = 0f;

					if (PanelSetup.panelGravity){
						
						stairPanels.rigidbody.useGravity = true;
						
					} else if (!PanelSetup.panelGravity){
						
						stairPanels.rigidbody.useGravity = false;
					}
					
					if (PanelSetup.panelKinematic){
						
						stairPanels.rigidbody.isKinematic = true;
						
					} else if (!PanelSetup.panelKinematic){
						
						stairPanels.rigidbody.isKinematic = false;
					}
				}
				
				if (PanelSetup.createPanel && PanelSetup.stairPanelMaterial != null){
					
					stairPanels.renderer.material = PanelSetup.stairPanelMaterial;
				}
				
				// Always create collider for first and last stair, for the realism and because of angled colliders that are between stairs
				if (!StairsSetup.stairsCollider && stairNr != amountOfColliders && stairNr != 0) {
					
					Destroy(stairs.gameObject.GetComponent<BoxCollider>());
				}
				
				Transform stairPanel = stairPanels.GetComponent<Transform>();

				GameObject stairPanelPivotObject = new GameObject ("stairPanelPivot");
				
				Transform stairPanelPivot = stairPanelPivotObject.GetComponent<Transform>();
				
				stairPanelPivot.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
				stairPanel.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);

				// If stairs go up
				if (StairsSetup.stairSize.y > 0 && StairsSetup.VerticalRangeBetween > 0){

					stairPanelPivot.transform.position = stairPivot.transform.position;
					
					stairPanel.transform.position = new Vector3 (xStairPosition, startPosition.y + stairsApartVertical + StairsSetup.stairSize.y * stairNr + centerPillar.parent.position.y, zStairPosition);

				// If stairs go down
				} else {

					stairPanelPivot.transform.position = new Vector3 (stairPivot.transform.position.x, stairPivot.transform.position.y - StairsSetup.stairSize.y, stairPivot.transform.position.z);
					
					stairPanel.transform.position = new Vector3 (xStairPosition, startPosition.y + stairsApartVertical + StairsSetup.stairSize.y * stairNr + centerPillar.parent.position.y - StairsSetup.stairSize.y, zStairPosition);
				}

				stairPanel.parent = stairPanelPivot;
				
				stairPanelPivot.transform.Rotate (0f, StairsSetup.currentAngle, 0f);

				// Resize pivot scale to resize panel size
				if (StairsSetup.stairSize.y > 0 && StairsSetup.VerticalRangeBetween > 0){

					stairPanelPivot.transform.localScale = new Vector3 (PanelSetup.panelSize.x, PanelSetup.panelSize.y, -PanelSetup.panelSize.z);

				} else {

					stairPanelPivot.transform.localScale = new Vector3 (PanelSetup.panelSize.x, PanelSetup.panelSize.y, PanelSetup.panelSize.z);

				}

				stairPanelPivot.parent = centerPillar;
			}

			// CREATE PANEL FRONTMESH FOR STAIRS
			if (PanelSetup.createPanelFrontMesh ){
				
				panelFrontMeshObj = new GameObject ("panelFrontMesh");

				GameObject panelFrontMeshPivotObject = new GameObject ("panelFrontMeshPivot");

				Transform panelFrontMeshPivot = panelFrontMeshPivotObject.GetComponent<Transform>();
				
				panelFrontMeshPivot.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
				panelFrontMeshObj.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
				
				// If stairs go up
				if (StairsSetup.stairSize.y > 0 && StairsSetup.VerticalRangeBetween > 0){
					
					panelFrontMeshPivot.transform.position = stairPivot.transform.position;
					
					panelFrontMeshObj.transform.position = new Vector3 (xStairPosition, startPosition.y + stairsApartVertical + StairsSetup.stairSize.y * stairNr + centerPillar.parent.position.y, zStairPosition);
					
				// If stairs go down
				} else {
					
					panelFrontMeshPivot.transform.position = new Vector3 (stairPivot.transform.position.x, stairPivot.transform.position.y - StairsSetup.stairSize.y, stairPivot.transform.position.z);
					
					panelFrontMeshObj.transform.position = new Vector3 (xStairPosition, startPosition.y + stairsApartVertical + StairsSetup.stairSize.y * stairNr + centerPillar.parent.position.y - StairsSetup.stairSize.y, zStairPosition);
				}
				
				panelFrontMeshObj.transform.parent = panelFrontMeshPivot;

				panelFrontMeshPivot.transform.Rotate (0f, StairsSetup.currentAngle, 0f);
				
				// Resize pivot scale to resize panel size
				if (StairsSetup.stairSize.y > 0 && StairsSetup.VerticalRangeBetween > 0){

					if (PanelSetup.createPanel){

						panelFrontMeshPivot.transform.localScale = new Vector3 (PanelSetup.panelSize.x * 2, PanelSetup.panelSize.y, (-PanelSetup.panelSize.z * 2) + -0.02f);

					} else {

						panelFrontMeshPivot.transform.localScale = new Vector3 (StairsSetup.stairSize.x * 2, StairsSetup.stairSize.y, -0.02f);
					}

				} else {

					if (PanelSetup.createPanel){
						
						panelFrontMeshPivot.transform.localScale = new Vector3 (PanelSetup.panelSize.x * 2, PanelSetup.panelSize.y, (PanelSetup.panelSize.z * 2) + 0.02f);
						
					} else {
						
						panelFrontMeshPivot.transform.localScale = new Vector3 (StairsSetup.stairSize.x * 2, StairsSetup.stairSize.y, 0.02f);
					}
				}

				CreateFrontPanelMesh (panelFrontMeshObj);

				if (StairsSetup.stairsCCW){

					if (PanelSetup.createPanel){

						panelFrontMeshObj.transform.position = new Vector3(panelFrontMeshObj.transform.position.x, panelFrontMeshObj.transform.position.y - (PanelSetup.panelSize.y / 2), panelFrontMeshObj.transform.position.z);

					} else if (!PanelSetup.createPanel && StairsSetup.stairSize.y < 0 && StairsSetup.VerticalRangeBetween < 0){

						panelFrontMeshObj.transform.position = new Vector3(panelFrontMeshObj.transform.position.x, stairs.transform.position.y - (StairsSetup.stairSize.y / 2), panelFrontMeshObj.transform.position.z);

					} else {

						panelFrontMeshObj.transform.position = new Vector3(panelFrontMeshObj.transform.position.x, panelFrontMeshObj.transform.position.y - (StairsSetup.stairSize.y / 2), panelFrontMeshObj.transform.position.z);
					}

				} else {

					if (PanelSetup.createPanel){

						panelFrontMeshObj.transform.Rotate (180f, 0f, 0f);
						
						panelFrontMeshObj.transform.position = new Vector3(panelFrontMeshObj.transform.position.x, panelFrontMeshObj.transform.position.y + (PanelSetup.panelSize.y / 2), panelFrontMeshObj.transform.position.z);

					} else if (!PanelSetup.createPanel && StairsSetup.stairSize.y < 0 && StairsSetup.VerticalRangeBetween < 0){

						panelFrontMeshObj.transform.Rotate (180f, 0f, 0f);

						panelFrontMeshObj.transform.position = new Vector3(panelFrontMeshObj.transform.position.x, stairs.transform.position.y + (StairsSetup.stairSize.y / 2), panelFrontMeshObj.transform.position.z);

					} else {

						panelFrontMeshObj.transform.Rotate (180f, 0f, 0f);

						panelFrontMeshObj.transform.position = new Vector3(panelFrontMeshObj.transform.position.x, panelFrontMeshObj.transform.position.y + (StairsSetup.stairSize.y / 2), panelFrontMeshObj.transform.position.z);

					}
				}

				panelFrontMeshPivot.parent = centerPillar;
			}

			// CREATE RAILING AND HANDLE FOR RAILING
			if (RailingSetup.createRailing){

				railing = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

				if (RailingSetup.railingMaterial != null){
					
					railing.renderer.material = RailingSetup.railingMaterial;
				}

				if (!RailingSetup.railingCollider){

					Destroy(railing.GetComponent<CapsuleCollider>());
				}

				Transform rail = railing.GetComponent<Transform>();

				if (RailingSetup.railingRigidbody){

					railing.AddComponent<Rigidbody>();
					railing.rigidbody.angularDrag = 0f;

					if (RailingSetup.railingGravity){

						railing.rigidbody.useGravity = true;

					} else if (!RailingSetup.railingGravity){

						railing.rigidbody.useGravity = false;
					}

					if (RailingSetup.railingKinematic){

						railing.rigidbody.isKinematic = true;
					
					} else if (!RailingSetup.railingKinematic){

						railing.rigidbody.isKinematic = false;
					}


				}


				xRailingPosition = centerPillar.parent.position.x - Mathf.Cos(StairsSetup.currentAngle * Mathf.Deg2Rad) * (centerPillarRadius + StairsSetup.stairSize.x);
				
				zRailingPosition = centerPillar.parent.position.z + Mathf.Sin(StairsSetup.currentAngle * Mathf.Deg2Rad) * (centerPillarRadius + StairsSetup.stairSize.x);

				railing.transform.position = new Vector3 (xRailingPosition, (startPosition.y * 1.4f) + (StairsSetup.stairSize.y * stairNr)+ stairsApartVertical + railScale.y / 1.5f + centerPillar.parent.position.y, zRailingPosition);

				addYScale = 0.0f;

				if (RailingSetup.railingScaleSpeed > railScale.y){
					
					RailingSetup.railingScaleSpeed = railScale.y;
				}

				if (!RailingSetup.railingScaleInstant){

					while (railing.transform.localScale.y < railScale.y){

						addYScale += RailingSetup.railingScaleSpeed;
						
						railing.transform.localScale = new Vector3(railScale.x, addYScale, railScale.z);
						
						yield return 0;
					}

				} else {

					railing.transform.localScale = railScale;
				}


				// Rotation near the railing
				float midStairLenXHandle = centerPillarRadius + StairsSetup.stairSize.x;

				float midStairDistXHandle = midStairLenXHandle * Mathf.Tan(stairAngle * Mathf.Deg2Rad);

				float midStairHypotenuseHandle = Mathf.Sqrt(Mathf.Pow(midStairDistXHandle, 2) + Mathf.Pow(StairsSetup.VerticalRangeBetween + StairsSetup.stairSize.y, 2));

				float midStairRotXHandle = Mathf.Asin((StairsSetup.VerticalRangeBetween + StairsSetup.stairSize.y) / midStairHypotenuseHandle) * Mathf.Rad2Deg;

				if (stairNr == 0){

					railingHandle = GameObject.CreatePrimitive(PrimitiveType.Sphere);

					if (RailingSetup.railHandleMaterial != null){

						railingHandle.renderer.material = RailingSetup.railHandleMaterial;
					}

					if (RailingSetup.handleRigidbody){
						
						railingHandle.AddComponent<Rigidbody>();
						railingHandle.rigidbody.angularDrag = 0f;

						if (RailingSetup.handleGravity){
							
							railingHandle.rigidbody.useGravity = true;
							
						} else if (!RailingSetup.handleGravity){
							
							railingHandle.rigidbody.useGravity = false;
						}
						
						if (RailingSetup.handleKinematic){
							
							railingHandle.rigidbody.isKinematic = true;
							
						} else if (!RailingSetup.handleKinematic){
							
							railingHandle.rigidbody.isKinematic = false;
						}
					}
					
					if (!RailingSetup.handleCollider){
						
						Destroy(railingHandle.gameObject.GetComponent<SphereCollider>());
					}


					float railSphereScale = railScale.x / railScale.z / railScale.y;

					railingHandle.transform.localScale = new Vector3 (railSphereScale, railSphereScale, railSphereScale);

				} else if (stairNr > 0){

					railingHandle = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

					if (RailingSetup.railHandleMaterial != null){
						
						railingHandle.renderer.material = RailingSetup.railHandleMaterial;
					}

					if (RailingSetup.handleRigidbody){
						
						railingHandle.AddComponent<Rigidbody>();
						railingHandle.rigidbody.angularDrag = 0f;
						
						if (RailingSetup.handleGravity){
							
							railingHandle.rigidbody.useGravity = true;
							
						} else if (!RailingSetup.handleGravity){
							
							railingHandle.rigidbody.useGravity = false;
						}
						
						if (RailingSetup.handleKinematic){
							
							railingHandle.rigidbody.isKinematic = true;
							
						} else if (!RailingSetup.handleKinematic){
							
							railingHandle.rigidbody.isKinematic = false;
						}
					}
					
					if (!RailingSetup.handleCollider){
						
						Destroy(railingHandle.gameObject.GetComponent<BoxCollider>());

					} else if (RailingSetup.handleCollider){

						railingHandle.AddComponent<BoxCollider>();
					}

					Destroy(railingHandle.gameObject.GetComponent<CapsuleCollider>());

					railingHandle.transform.localScale = new Vector3 (railScale.x * 2f, railScale.x, midStairHypotenuseHandle);
				}

				Transform railHandle = railingHandle.GetComponent<Transform>();

				railingHandle.transform.position = new Vector3 (xRailingPosition, railing.transform.position.y + railing.transform.localScale.y, zRailingPosition);


				if (handleRotationSet){

					handleDistance = Mathf.Sqrt(Mathf.Pow((railing.transform.position.x + railing.transform.position.z) - (lastRailing.x + lastRailing.z), 2) + Mathf.Pow(railing.transform.position.y - lastRailing.y, 2));

					handleRotation = (Mathf.Asin((StairsSetup.VerticalRangeBetween + stairs.transform.localScale.y) / handleDistance));
					
					if (float.IsNaN(handleRotation)){
						
						handleRotation = lastHandleRotation;
					}
					
					lastHandleRotation = handleRotation;

					lastRailing = railing.transform.position;

					if (stairNr > 1 ){

						handleRotationSet = false;
					}

				}

				if (stairNr == 0){

					railHandle.transform.Rotate (0f, 0f, 0f);

				} else if (stairNr > 0 && StairsSetup.stairsCCW){

					railHandle.transform.Rotate (midStairRotXHandle, StairsSetup.currentAngle, 0f);

				} else if (stairNr > 0 && !StairsSetup.stairsCCW){

					railHandle.transform.Rotate (-midStairRotXHandle, StairsSetup.currentAngle, 0f);
				}

				rail.parent = centerPillar;
				
				railHandle.parent = transform;
			
			// IF RAILING IS NOT NEEDED CREATE POSITIONS FOR THE ANGLED COLLIDERS
			} else {

				xRailingPosition = centerPillar.parent.position.x - Mathf.Cos(StairsSetup.currentAngle * Mathf.Deg2Rad) * (centerPillarRadius + StairsSetup.stairSize.x);
				
				zRailingPosition = centerPillar.parent.position.z + Mathf.Sin(StairsSetup.currentAngle * Mathf.Deg2Rad) * (centerPillarRadius + StairsSetup.stairSize.x);
			}

			// CREATE ANGLED COLLIDERS BETWEEN THE STAIRS FOR BETTER CHARACTER CONTROLLING. IF YOU TURN THESE OF REMEMBER TO USE STAIR COLLIDERS.
			if (StairsSetup.angledStairCollider){

				// If stairs go up and to the right
				if (StairsSetup.stairSize.y + StairsSetup.VerticalRangeBetween >= 0){
					
					// Check if last stair and if it is, do not make any more colliders. So last collider will be inbetween second to last and last stair
					if (stairNr < amountOfColliders){
						
						stairMoveCollidersStart = GameObject.CreatePrimitive(PrimitiveType.Cube);
						stairMoveCollidersEnd = GameObject.CreatePrimitive(PrimitiveType.Cube);
						
						Transform stairMoveColliderStart = stairMoveCollidersStart.GetComponent<Transform>();
						Transform stairMoveColliderEnd = stairMoveCollidersEnd.GetComponent<Transform>();
						
						stairMoveCollidersStart.renderer.enabled = false;
						stairMoveCollidersEnd.renderer.enabled = false;
						
						stairMoveColliderStart.parent = transform;
						stairMoveColliderEnd.parent = transform;
						
						// Calculations

						// Rotation near the pillar
						float midStairLenXStart = centerPillarRadius + StairsSetup.stairSize.x / 13f;

						float midStairDistXStart = midStairLenXStart * Mathf.Tan((stairAngle) * Mathf.Deg2Rad);

						float midStairHypotenuseStart = Mathf.Sqrt(Mathf.Pow(midStairDistXStart, 2) + Mathf.Pow(StairsSetup.VerticalRangeBetween + StairsSetup.stairSize.y, 2));

						float midStairRotXStart = Mathf.Asin((StairsSetup.VerticalRangeBetween + StairsSetup.stairSize.y) / midStairHypotenuseStart) * Mathf.Rad2Deg;

						// Rotation near the railing
						float midStairLenXEnd = centerPillarRadius + StairsSetup.stairSize.x / 2f;

						float midStairDistXEnd = midStairLenXEnd * Mathf.Tan(stairAngle * Mathf.Deg2Rad);
						
						float midStairHypotenuseEnd = Mathf.Sqrt(Mathf.Pow(midStairDistXEnd, 2) + Mathf.Pow(StairsSetup.VerticalRangeBetween + StairsSetup.stairSize.y, 2));

						float midStairRotXEnd = Mathf.Asin((StairsSetup.VerticalRangeBetween + StairsSetup.stairSize.y) / midStairHypotenuseEnd) * Mathf.Rad2Deg;


						stairMoveCollidersStart.transform.localScale = new Vector3(StairsSetup.stairSize.x / 2f, 0.01f, midStairHypotenuseStart);
						stairMoveCollidersEnd.transform.localScale = new Vector3(StairsSetup.stairSize.x / 2f, 0.01f, midStairHypotenuseEnd * 1.75f);

						// Collider's position in X-axis
						float stairMoveCollidersPosXStart = ((xRailingPosition - stairPivot.transform.position.x) / 5f) + stairPivot.transform.position.x;
						float stairMoveCollidersPosXEnd = ((xRailingPosition - stairPivot.transform.position.x) / 1.5f) + stairPivot.transform.position.x;

						// Collider's position in X-axis
						float stairMoveCollidersPosZStart = ((zRailingPosition - stairPivot.transform.position.z) / 5f) + stairPivot.transform.position.z;
						float stairMoveCollidersPosZEnd = ((zRailingPosition - stairPivot.transform.position.z) / 1.5f) + stairPivot.transform.position.z;

						
						// Collider's position in Y-axis
						float stairMoveCollidersPosYStart = stair.transform.position.y + StairsSetup.VerticalRangeBetween + StairsSetup.stairSize.y / 2.5f;
						float stairMoveCollidersPosYEnd = stair.transform.position.y + StairsSetup.stairSize.y;

						if (StairsSetup.stairsCCW){

							// According to stairsCCW
							float nextStepAngle = StairsSetup.currentAngle - stairAngle;
							
							stairMoveCollidersStart.transform.Rotate(midStairRotXStart, nextStepAngle, 0f);
							stairMoveCollidersEnd.transform.Rotate(midStairRotXEnd, nextStepAngle, 0f);

						} else if (!StairsSetup.stairsCCW){

							// According to stairsCCW
							float nextStepAngle = StairsSetup.currentAngle + stairAngle;
							
							stairMoveCollidersStart.transform.Rotate(-midStairRotXStart, nextStepAngle, 0f);
							stairMoveCollidersEnd.transform.Rotate(-midStairRotXEnd, nextStepAngle, 0f);
						}
						
						stairMoveCollidersStart.transform.position = new Vector3 (stairMoveCollidersPosXStart, stairMoveCollidersPosYStart, stairMoveCollidersPosZStart);
						stairMoveCollidersEnd.transform.position = new Vector3 (stairMoveCollidersPosXEnd, stairMoveCollidersPosYEnd, stairMoveCollidersPosZEnd);

					}
				}

				if (StairsSetup.stairSize.y + StairsSetup.VerticalRangeBetween < 0){
					
					// Check if last stair and if it is, do not make any more colliders. So last collider will be inbetween second to last and last stair
					if (stairNr < StairsSetup.amountOfStairs && stairNr > 0){
						
						stairMoveCollidersStart = GameObject.CreatePrimitive(PrimitiveType.Cube);
						stairMoveCollidersEnd = GameObject.CreatePrimitive(PrimitiveType.Cube);
						
						Transform stairMoveColliderStart = stairMoveCollidersStart.GetComponent<Transform>();
						Transform stairMoveColliderEnd = stairMoveCollidersEnd.GetComponent<Transform>();
						
						stairMoveCollidersStart.renderer.enabled = false;
						stairMoveCollidersEnd.renderer.enabled = false;
						
						stairMoveColliderStart.parent = transform;
						stairMoveColliderEnd.parent = transform;
						
						// Calculations
						
						// Rotation near the pillar
						float midStairLenXStart = centerPillarRadius + StairsSetup.stairSize.x / 13f;
						
						float midStairDistXStart = midStairLenXStart * Mathf.Tan((stairAngle) * Mathf.Deg2Rad);
						
						float midStairHypotenuseStart = Mathf.Sqrt(Mathf.Pow(midStairDistXStart, 2) + Mathf.Pow(StairsSetup.VerticalRangeBetween + StairsSetup.stairSize.y, 2));
						
						float midStairRotXStart = Mathf.Asin((StairsSetup.VerticalRangeBetween + StairsSetup.stairSize.y) / midStairHypotenuseStart) * Mathf.Rad2Deg;
						
						// Rotation near the railing
						float midStairLenXEnd = centerPillarRadius + StairsSetup.stairSize.x / 2f;
						
						float midStairDistXEnd = midStairLenXEnd * Mathf.Tan(stairAngle * Mathf.Deg2Rad);
						
						float midStairHypotenuseEnd = Mathf.Sqrt(Mathf.Pow(midStairDistXEnd, 2) + Mathf.Pow(StairsSetup.VerticalRangeBetween + StairsSetup.stairSize.y, 2));
						
						float midStairRotXEnd = Mathf.Asin((StairsSetup.VerticalRangeBetween + StairsSetup.stairSize.y) / midStairHypotenuseEnd) * Mathf.Rad2Deg;
						
						
						stairMoveCollidersStart.transform.localScale = new Vector3(StairsSetup.stairSize.x / 2f, 0.01f, midStairHypotenuseStart * 1.5f);
						stairMoveCollidersEnd.transform.localScale = new Vector3(StairsSetup.stairSize.x / 2f, 0.01f, midStairHypotenuseEnd * 1.75f);
						
						// Collider's position in X-axis
						float stairMoveCollidersPosXStart = ((xRailingPosition - stairPivot.transform.position.x) / 5f) + stairPivot.transform.position.x;
						float stairMoveCollidersPosXEnd = ((xRailingPosition - stairPivot.transform.position.x) / 1.5f) + stairPivot.transform.position.x;
						
						// Collider's position in X-axis
						float stairMoveCollidersPosZStart = ((zRailingPosition - stairPivot.transform.position.z) / 5f) + stairPivot.transform.position.z;
						float stairMoveCollidersPosZEnd = ((zRailingPosition - stairPivot.transform.position.z) / 1.5f) + stairPivot.transform.position.z;
						
						// Collider's position in Y-axis
						float stairMoveCollidersPosYStart = stair.transform.position.y - StairsSetup.VerticalRangeBetween + StairsSetup.stairSize.y / 2.5f;
						float stairMoveCollidersPosYEnd = stair.transform.position.y - StairsSetup.VerticalRangeBetween;
						
						if (StairsSetup.stairsCCW){
							
							// According to stairsCCW
							if (stairNr == 1){

								nextStepAngle = StairsSetup.currentAngle + stairAngle / 2;

							} else {

								nextStepAngle = StairsSetup.currentAngle;
							}

							stairMoveCollidersStart.transform.Rotate(midStairRotXStart, nextStepAngle, 0f);
							stairMoveCollidersEnd.transform.Rotate(midStairRotXEnd, nextStepAngle, 0f);
							
						} else if (!StairsSetup.stairsCCW){
							
							// According to stairsCCW
							if (stairNr == 1){
								nextStepAngle = StairsSetup.currentAngle - stairAngle / 2;
								
							} else {
								
								nextStepAngle = StairsSetup.currentAngle;
							}
							
							stairMoveCollidersStart.transform.Rotate(-midStairRotXStart, nextStepAngle, 0f);
							stairMoveCollidersEnd.transform.Rotate(-midStairRotXEnd, nextStepAngle, 0f);
						}
						
						stairMoveCollidersStart.transform.position = new Vector3 (stairMoveCollidersPosXStart, stairMoveCollidersPosYStart, stairMoveCollidersPosZStart);
						stairMoveCollidersEnd.transform.position = new Vector3 (stairMoveCollidersPosXEnd, stairMoveCollidersPosYEnd, stairMoveCollidersPosZEnd);
						
					}
				}
			}

			stairsApartVertical = stairsApartVertical + StairsSetup.VerticalRangeBetween;
		
		stairNr++;
		
		yield return 0;

		}
		
		creatingStairs = false;

		// If user used boolean value to create stairs set it to false so it only creates one set of staircases
		if (stairNr == StairsSetup.amountOfStairs){

			createStaircase = false;
		}
	}

	// CREATE MESH RENDERER AND MESH FILTER FOR FRONT PANEL MESH RENDERERS
	void CreateFrontPanelMesh (GameObject frontPanelMesh){

		frontPanelMesh.AddComponent<MeshFilter>();
		frontPanelMesh.AddComponent<MeshRenderer>();

		MeshFilter meshFilter = frontPanelMesh.GetComponent<MeshFilter>();

		Mesh mesh = new Mesh();
		meshFilter.mesh = mesh;
		
		//VERTICES (give position all 4 corners of the plane)

		Vector3[] vertices = new Vector3[4];

		if (StairsSetup.VerticalRangeBetween < 0 && StairsSetup.stairSize.y < 0 && !PanelSetup.createPanel){

			if (StairsSetup.stairsCCW){
				//if (stairsCCW){
				
				vertices = new Vector3[4]{
					
					// From bottom left to bottom right to top left and last top right of the plane, so all the 4 corners
					new Vector3(frontPanelMesh.transform.localScale.x / 2, frontPanelMesh.transform.localScale.y, 0), new Vector3(0, frontPanelMesh.transform.localScale.y, 0), new Vector3(frontPanelMesh.transform.localScale.x / 2, 0, 0), new Vector3(0,0,0)
				};
				
			} else if (!StairsSetup.stairsCCW) {
				
				vertices = new Vector3[4]{
					
					// Rotate vertices to rotate the mesh
					new Vector3(0,0,0), new Vector3(frontPanelMesh.transform.localScale.x / 2, 0, 0), new Vector3(0, frontPanelMesh.transform.localScale.y, 0), new Vector3(frontPanelMesh.transform.localScale.x / 2, frontPanelMesh.transform.localScale.y, 0)
					
				};
			}

		} else {

			if (StairsSetup.stairsCCW){
				
				vertices = new Vector3[4]{
					
					// From bottom left to bottom right to top left and last top right of the plane, so all the 4 corners
					new Vector3(0,0,0), new Vector3(frontPanelMesh.transform.localScale.x / 2, 0, 0), new Vector3(0, frontPanelMesh.transform.localScale.y, 0), new Vector3(frontPanelMesh.transform.localScale.x / 2, frontPanelMesh.transform.localScale.y, 0)
					
				};
				
			} else if (!StairsSetup.stairsCCW) {
				
				vertices = new Vector3[4]{
					
					// Rotate vertices to rotate the mesh
					new Vector3(frontPanelMesh.transform.localScale.x / 2, frontPanelMesh.transform.localScale.y, 0), new Vector3(0, frontPanelMesh.transform.localScale.y, 0), new Vector3(frontPanelMesh.transform.localScale.x / 2, 0, 0), new Vector3(0,0,0)
					
				};
			}
		}

		//TRIANGLES (2 triangles of the plane and give points from clockwise rotation of the triangle) using int cos of indexes
		int[] triangles = new int[6];
		
		triangles[0] = 0;
		triangles[1] = 2;
		triangles[2] = 1;
		
		triangles[3] = 2;
		triangles[4] = 3;
		triangles[5] = 1;
		
		//NORMALS (which direction is the plane shown)(if you dont need to see the object you can forget about this)
		Vector3[] normals = new Vector3[4];

		normals[0] = -Vector3.forward;
		normals[1] = -Vector3.forward;
		normals[2] = -Vector3.forward;
		normals[3] = -Vector3.forward;

		//UV's (how textures are shown % ex. point 0 is 0% width and height and point 3 is 100% and 100%)
		Vector2[] uv = new Vector2[4];
		
		uv[0] = new Vector2(0, 0);
		uv[1] = new Vector2(1, 0);
		uv[2] = new Vector2(0, 1);
		uv[3] = new Vector2(1, 1);
		
		//ASSIGN Arrays to objects
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.normals = normals;
		mesh.uv = uv;

		frontPanelMesh.renderer.material = PanelSetup.panelFrontMeshMaterial;

		mesh.RecalculateBounds();
		mesh.Optimize();

	}

	void OnDrawGizmos(){

		if (showGizmos){

			Gizmos.color = gizmoColor;
			
			Gizmos.DrawWireSphere (transform.position, gizmoSize);
		}
	}
}

