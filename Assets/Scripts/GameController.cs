using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	// Components
	private WorldGenerator worldGenerator;
	// Properties
	private bool isPaused = false;
	private Vector3 mousePosWorld;
	// References
	private Camera cameraRef;
	private List<Traveler> travelers;


	// Getters
	public Vector3 MousePosWorld { get { return mousePosWorld; } }



	void Start () {
		cameraRef = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera> ();
		worldGenerator = GetComponent<WorldGenerator> ();

		ResetGame ();
	}
	private void ResetGame() {
		// Generate world!
		worldGenerator.GenerateWorld ();
		// Reset travelers!
		DestroyAllTravelers ();
		GameObject travelerPrefab = (GameObject) Resources.Load ("Prefabs/Traveler");
		travelers = new List<Traveler> ();
		int NUM_TRAVELERS = 8;
		for (int i=0; i<NUM_TRAVELERS; i++) {
			Traveler newTraveler = Instantiate (travelerPrefab).GetComponent<Traveler>();
			float charge = i<NUM_TRAVELERS*0.5f ? -1 : 1;
			newTraveler.Initialize (this, worldGenerator.sourceNode, charge);
			travelers.Add (newTraveler);
		}
	}


//	private void OnDrawGizmos () {
//		Gizmos.color = Color.cyan;
//		Gizmos.DrawSphere (mousePosWorld, 1);
//	}

	
	
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
	}


	private void DestroyAllTravelers() {
		if (travelers != null) {
			for (int i=0; i<travelers.Count; i++) {
				Destroy (travelers [i].gameObject);
			}
		}
		travelers = null;
	}


}






