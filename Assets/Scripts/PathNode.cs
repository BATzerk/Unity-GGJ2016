using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathNode : MonoBehaviour {
	// Constants
	private const float creationSpeedFriction = 0.95f;
	// Components
	[SerializeField] private SpriteRenderer auraSprite;
	[SerializeField] private SpriteRenderer lineSprite;
	// Properties
	private Vector3 displayPos;
	private float creationSpeed;
	public float familiarity = 0.3f; // determines brightness
	private float charge; // determines color
//	private float delayToCreateNextNode; // doesn't change.
//	private float timeUntilCreateNextNode; // does change.
	private float nextNodeAngleDif; // plan this for the future
	private float locFromPrevNode = 0; // we start at 0 and make it up to 1.
	private int numNodesUntilBranch;
	private Rect boundsRect;
	// References
	public PathNode previousNode;
	public List<PathNode> nextNodes;

	// Getters
	private float angleFromPreviousNode {
		get {
			if (previousNode != null) {
				return Mathf.Atan2 (transform.localPosition.y - previousNode.transform.localPosition.y, transform.localPosition.x - previousNode.transform.localPosition.x);
			}
			return Mathf.PI * 0.5f; // Default to up.
		}
	}


	// Constructor
	public void Initialize (Vector2 _pos, PathNode _previousNode, int _numNodesUntilBranch, float _creationSpeed, float _nextNodeAngleDif) {
		this.transform.localPosition = _pos;
		previousNode = _previousNode;
		numNodesUntilBranch = _numNodesUntilBranch;
		creationSpeed = _creationSpeed;
		nextNodeAngleDif = _nextNodeAngleDif;
		nextNodes = new List<PathNode> ();
		lineSprite = GetComponentInChildren<SpriteRenderer> ();

		UpdatePosAndSize ();

		boundsRect = new Rect (0, 0, 20, 20);

//		timeUntilCreateNextNode = delayToCreateNextNode = 1f / creationSpeed;
	}


	private void MakeNextNodeAttempt () {
		float distanceToNextNode = 1f;
		float angleToNextNode = angleFromPreviousNode + nextNodeAngleDif;//Random.Range (-0.5f, 0.5f);
		Vector3 nextNodePos = transform.localPosition + new Vector3 (Mathf.Cos (angleToNextNode)*distanceToNextNode, Mathf.Sin (angleToNextNode)*distanceToNextNode, 0);
		AddNextNode (nextNodePos);
	}

	private void MakeBranchNodeAttempt(bool isLeftTurn) {
		float differenceVolume = Random.Range (0.5f, 1.5f);
		float distanceToNextNode = 1.8f * differenceVolume;
		float angleToNextNode = angleFromPreviousNode + (isLeftTurn ? -0.5f : 0.5f) * differenceVolume;
		Vector3 nextNodePos = transform.localPosition + new Vector3 (Mathf.Cos (angleToNextNode)*distanceToNextNode, Mathf.Sin (angleToNextNode)*distanceToNextNode, 0);

		// Make the next node!!
		numNodesUntilBranch = Random.Range (2, 3);
		nextNodeAngleDif = Random.Range (-0.2f, 0.2f);
		AddNextNode (nextNodePos);
	}

	private void AddNextNode(Vector3 nextNodePos) {
		// Outta bounds? Get outta town!
//		if (!boundsRect.Contains(nextNodePos)) {
//			return;
//		}

		// Creation speed too slow? Ehh, cut it out here, eh.
		float newNodeCreationSpeed = creationSpeed*creationSpeedFriction;
		if (newNodeCreationSpeed < 0.1f) {
			return;
		}
		
		GameObject pathNodePrefab = (GameObject)Resources.Load ("Prefabs/PathNode");
		PathNode newNode = Instantiate (pathNodePrefab).GetComponent<PathNode> ();
		newNode.Initialize (nextNodePos, this, numNodesUntilBranch, newNodeCreationSpeed, nextNodeAngleDif);
		nextNodes.Add (newNode);
	}






	/*
	private void OnDrawGizmos () {
		Color myColor = Color.Lerp(Color.blue, Color.yellow, 0.5f + charge*0.5f);
		myColor = new Color (myColor.r, myColor.g, myColor.b, familiarity);
		Gizmos.color = myColor;
//		Gizmos.DrawSphere (this.transform.localPosition, 0.01f);
		if (previousNode != null) {
			displayPos = Vector3.Lerp (previousNode.transform.localPosition, transform.localPosition, locFromPrevNode);
			Gizmos.DrawLine (displayPos, previousNode.transform.localPosition);
		}
	}
	*/





	private void Update() {
		// Maybe expire!
		// QQQ
//		familiarity -= 0.02f * Time.deltaTime;
//		if (familiarity <= 0) {
//			Expire ();
//			return;
//		}

		UpdateColor ();
		UpdateLineThickness ();

		// Already made our next guy? Okay then, don't do anything.
		if (locFromPrevNode >= 1) { return; }
		// No previous node? Don't do anything.
		if (previousNode == null) { return; }

		// Apply friction!
		creationSpeed *= Mathf.Pow(creationSpeedFriction, Time.deltaTime*GameProperties.WORLD_TIME_SCALE);

		float distanceToPreviousNode = Vector3.Distance (transform.localPosition, previousNode.transform.localPosition);
		locFromPrevNode += creationSpeed/distanceToPreviousNode*Time.deltaTime;
		if (locFromPrevNode >= 1) {
			// DO branch.
			if (numNodesUntilBranch-- <= 0) {
				nextNodeAngleDif += Random.Range(-0.2f, 0.2f);
				MakeBranchNodeAttempt (true);
				MakeBranchNodeAttempt (false);
			}
			else {
				MakeNextNodeAttempt ();
			}
			locFromPrevNode = 1;
		}

		// Update my look!
		UpdatePosAndSize ();
	}


	private void UpdateColor () {
//		Color myColor = Color.Lerp(Color.blue, Color.yellow, 0.5f + charge*0.5f);
		float lerpAmount = Mathf.PerlinNoise (transform.localPosition.x/20f, transform.localPosition.y/20f);
		Color myColor = Color.Lerp (new Color(0/255f,208/255f,255/255f), new Color(0/255f,64/255f,255/255f), lerpAmount);
		myColor = new Color (myColor.r, myColor.g, myColor.b, 0.5f + familiarity*0.5f);
		lineSprite.color = myColor;
		auraSprite.color = new Color (myColor.r, myColor.g, myColor.b, (familiarity-0.15f) * 0.3f);
	}
	
	private void UpdatePosAndSize() {
		if (previousNode == null) { return; }
		displayPos = Vector3.Lerp (previousNode.transform.localPosition, transform.localPosition, locFromPrevNode);
		float angleBetweenNodes = Mathf.Atan2 (displayPos.y - previousNode.transform.localPosition.y, displayPos.x - previousNode.transform.localPosition.x) * Mathf.Rad2Deg;
		lineSprite.transform.localPosition = Vector3.Lerp (displayPos, previousNode.transform.localPosition, 0.5f) - this.transform.localPosition;
		lineSprite.transform.localEulerAngles = new Vector3 (0, 0, angleBetweenNodes);
		UpdateLineThickness ();
	}
	private void UpdateLineThickness() {
		if (previousNode == null) { return; }
		float thickness = 0.02f + familiarity * 0.2f;
		float distanceBetweenNodes = Vector3.Distance (displayPos, previousNode.transform.localPosition);
		GameUtils.SizeSprite (lineSprite, distanceBetweenNodes, thickness);
	}



	// Events
	public void OnTravelerEnterMe(Traveler traveler) {
		// Increase familiarity!
		familiarity += 0.03f;
		if (familiarity > 1) { familiarity = 1; }
		// Shift charge!
		charge += (traveler.Charge*0.4f-charge) / 8f;
	}
	public void OnTravelerReachMyDeadEnd() {
		creationSpeed = 10f;
	}



	public void Expire() {
		// Remove me from my previous dude's list of next dudes!
		if (previousNode != null) {
			previousNode.nextNodes.Remove (this);
		}
		// Tell my child to expire!
		for (int i=0; i<nextNodes.Count; i++) {
			nextNodes[i].Expire();
		}
		// Destroy me eh!
		Destroy (this.gameObject);
	}


}









