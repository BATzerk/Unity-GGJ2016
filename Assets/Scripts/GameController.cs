using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	// Components
	public PathNode sourceNode;
	// Properties
	private bool isPaused = false;
	private Vector3 mousePosWorld;
	private Rect pathNodeBoundsRect;
	// References
	private Camera cameraRef;
	private GameObject pathNodePrefab;
	private GameObject travelerPrefab;
//	private List<Traveler> travelers;


	// Getters
	public Rect PathNodeBoundsRect { get { return pathNodeBoundsRect; } }
	public Vector3 MousePosWorld { get { return mousePosWorld; } }


	
	// ================================================================
	//	Initialize
	// ================================================================
	void Start () {
		cameraRef = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ();
		pathNodeBoundsRect = new Rect ();
		
		pathNodePrefab = (GameObject)Resources.Load ("Prefabs/PathNode");
		travelerPrefab = (GameObject) Resources.Load ("Prefabs/Traveler");

		ResetGame ();
	}
	private void ResetGame() {
		DestroyAllNodes ();
		
		// Make the first TWO nodes.
		sourceNode = Instantiate (pathNodePrefab).GetComponent<PathNode> ();
		sourceNode.Initialize (this, Vector2.zero, null, 10, 0, 0);
		PathNode secondNode = Instantiate (pathNodePrefab).GetComponent<PathNode> ();
		secondNode.Initialize (this, new Vector2(0,0.5f), null, 4, 50, 0);
		sourceNode.nextNodes.Add (secondNode);
		secondNode.previousNode = sourceNode;

		// Reset travelers!
		DestroyAllTravelers ();
//		travelers = new List<Traveler> ();
//		int NUM_TRAVELERS = 8;
//		for (int i=0; i<NUM_TRAVELERS; i++) {
//			AddTraveler();
//			Traveler newTraveler = Instantiate (travelerPrefab).GetComponent<Traveler>();
//			float charge = i<NUM_TRAVELERS*0.5f ? -1 : 1;
//			newTraveler.Initialize (this, worldGenerator.sourceNode, charge);
//			travelers.Add (newTraveler);
//		}
	}

	public void UpdatePathNodeBoundsRect (Vector3 point) {
		GameUtils.UpdateRectFromPoint (ref pathNodeBoundsRect, point);
	}


//	private void OnDrawGizmos () {
//		Gizmos.color = Color.red;
//		Gizmos.DrawWireCube (pathNodeBoundsRect.center, new Vector3 (pathNodeBoundsRect.width, pathNodeBoundsRect.height));
//	}

	private void AddTraveler() {
		Traveler newTraveler = Instantiate (travelerPrefab).GetComponent<Traveler>();
		float charge = 1;
		newTraveler.Initialize (this, sourceNode, charge);
	}
	
	
	// ================================================================
	//	Update
	// ================================================================
	private void Update() {
		mousePosWorld = (Input.mousePosition - new Vector3(Screen.width*0.5f, Screen.height*0.5f)) / cameraRef.orthographicSize + cameraRef.transform.localPosition;

		AcceptButtonInput ();
	}


	private void AcceptButtonInput() {
		if (Input.GetKeyDown (KeyCode.Return)) {
			ResetGame();
		}
		else if (Input.GetKeyDown (KeyCode.Escape)) {
			isPaused = !isPaused;
			Time.timeScale = isPaused ? 0 : 1;
		}
		else if (Input.GetKeyDown (KeyCode.Space)) {
			AddTraveler ();
		}
	}

	
	// ================================================================
	//	Destroy!
	// ================================================================
	private void DestroyAllNodes() {
		PathNode[] allNodes = GameObject.FindObjectsOfType<PathNode> ();
		for (int i=0; i<allNodes.Length; i++) {
			Destroy (allNodes[i].gameObject);
		}
	}
	private void DestroyAllTravelers() {
		Traveler[] allTravelers = GameObject.FindObjectsOfType<Traveler> ();
		for (int i=0; i<allTravelers.Length; i++) {
			Destroy (allTravelers [i].gameObject);
		}
	}


}






