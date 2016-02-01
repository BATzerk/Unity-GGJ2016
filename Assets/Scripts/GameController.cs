using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	// Events
//	public delegate void TravelerReachTargetPathEndAction (Vector3 pos);
//	public event TravelerReachTargetPathEndAction TravelerReachTargetPathEndEvent;
	// Components
	public PathNode sourceNode;
	// Properties
	private bool isGameOver;
	private bool isPaused = false;
	private float timeUntilCreateTraveler = -1;
	private float targetPathLength;
	private Vector3 mousePosWorld;
	private Rect pathNodeBoundsRect;
	private float timeUntilGameEnds;
	// References
	[SerializeField] AudioSource celestaAudio;
	[SerializeField] AudioSource sparkleAudio;
	[SerializeField] private EffectsManager effectsManagerRef;
	[SerializeField] private Text scoreText;
	[SerializeField] private Text gameOverText;
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
		
		pathNodePrefab = (GameObject)Resources.Load ("Prefabs/PathNode");
		travelerPrefab = (GameObject) Resources.Load ("Prefabs/Traveler");

		ResetGame ();
	}
	private void ResetGame() {
		DestroyAllNodes ();
		
		pathNodeBoundsRect = new Rect ();
		targetPathLength = 0;
		isGameOver = false;
		timeUntilGameEnds = 45f;
		scoreText.enabled = false;
		gameOverText.enabled = false;
		
		// Make the first TWO nodes.
		sourceNode = Instantiate (pathNodePrefab).GetComponent<PathNode> ();
		sourceNode.Initialize (this, Vector2.zero, null, 10, 0, 0, true, true);
		PathNode secondNode = Instantiate (pathNodePrefab).GetComponent<PathNode> ();
		secondNode.Initialize (this, new Vector2(0,-0.5f), null, 4, 50, 0, true, true);
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

		if (!isGameOver) {
			timeUntilCreateTraveler -= Mathf.Max (0, (1 - Input.GetAxis ("Vertical") * 1.6f) * Time.deltaTime);
			if (timeUntilCreateTraveler <= 0) {
				AddTraveler ();
				timeUntilCreateTraveler = 2f;
			}
//		}
//		else {
//			timeUntilCreateTraveler = -1;
//		}

			timeUntilGameEnds -= Time.deltaTime;
			if (timeUntilGameEnds <= 0) {
				EndGame ();
			}
		}
	}


	private void AcceptButtonInput() {
		if (Input.GetKeyDown (KeyCode.Return)) {
			ResetGame();
		}
		else if (Input.GetKeyDown (KeyCode.Escape)) {
			isPaused = !isPaused;
			Time.timeScale = isPaused ? 0 : 1;
		}
		else if (isGameOver && (Input.GetButtonDown("Submit") || Input.GetKeyDown(KeyCode.Return))) {
			ResetGame();
		}
//		else if (Input.GetKeyDown (KeyCode.Space)) {
//			AddTraveler ();
//		}
	}

	
	public void OnTravelerReachTargetPathEnd (PathNode pathNode) {
		effectsManagerRef.OnTravelerReachTargetPathEnd (pathNode.transform.localPosition);
		targetPathLength = pathNode.GetDistanceFromSourceNode (0);
		scoreText.text = "Distance: " + ((int)targetPathLength);
		celestaAudio.Play ();
		sparkleAudio.Play ();
	}


	private void EndGame() {
		isGameOver = true;
		
		scoreText.transform.localPosition = new Vector3(-Screen.width*0.5f + 20, -Screen.height*0.5f+16, 0);
		gameOverText.transform.localPosition = new Vector3(Screen.width*0.5f - 20, -Screen.height*0.5f+16, 0);
		scoreText.enabled = true;
		gameOverText.enabled = true;
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






