using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathNode : MonoBehaviour {
	// Constants
	private const float AURA_DIAMETER = 10f;
	private const float creationSpeedFriction = 0.95f;
	// Components
	[SerializeField] private SpriteRenderer auraSprite;
	[SerializeField] private SpriteRenderer lineSprite;
	// Properties
	private bool isTargetPath;
	private bool isTravelable;
	private Vector3 anchorPos; // where I AIM to be yo
	private Vector3 displayPos;
	private Vector3 vel = Vector3.zero;
	private float creationSpeed;
	public float familiarity = 0.3f; // determines brightness
	private float familiarityRequiredToEndorseUntravelableBranch;
	private float charge; // determines color
	private float nextNodeAngleDif; // plan this for the future
	private float locFromPrevNode = 0; // we start at 0 and make it up to 1.
	private float displayAngleBetweenNodes;
	private int numNodesUntilBranch;
	// References
	private GameController gameControllerRef;
	public PathNode previousNode;
	public List<PathNode> nextNodes;
//	public List<PathNode> nextUntravelableNodes;
	private PathNode nextUntravelableNode;

	// Getters
	public bool IsTravelable { get { return isTravelable; } }
	public float LocFromPrevNode { get { return locFromPrevNode; } }
	public float DisplayAngleBetweenNodes { get { return displayAngleBetweenNodes; } }
	public Vector3 AnchorPos { get { return anchorPos; } }
	private float AnchorAngleFromPreviousNodeAnchor {
		get {
			if (previousNode != null) {
				return Mathf.Atan2 (anchorPos.y - previousNode.AnchorPos.y, anchorPos.x - previousNode.AnchorPos.x);
			}
			return -Mathf.PI * 0.5f; // Default to down.
		}
	}
	public float GetDistanceFromSourceNode(float distanceAlreadyTotaled) {
		if (previousNode == null)
			return distanceAlreadyTotaled;
		else {
			distanceAlreadyTotaled += Vector3.Distance(anchorPos, previousNode.AnchorPos);
			return previousNode.GetDistanceFromSourceNode(distanceAlreadyTotaled);
		}
	}

	
	// ================================================================
	//	Initialize
	// ================================================================
	public void Initialize (GameController _gameControllerRef, Vector2 _pos, PathNode _previousNode, int _numNodesUntilBranch, float _creationSpeed, float _nextNodeAngleDif, bool _isTargetPath, bool _isTravelable) {
		gameControllerRef = _gameControllerRef;
		anchorPos = _pos;
		this.transform.localPosition = anchorPos;
		isTargetPath = _isTargetPath;
		isTravelable = _isTravelable;
		previousNode = _previousNode;
		numNodesUntilBranch = _numNodesUntilBranch;
		creationSpeed = _creationSpeed;
		nextNodeAngleDif = _nextNodeAngleDif;
		nextNodes = new List<PathNode> ();
		familiarityRequiredToEndorseUntravelableBranch = familiarity + 0.06f;
//		nextUntravelableNodes = new List<PathNode> ();

		UpdatePosAndSize ();
		GameUtils.SizeSprite (auraSprite, AURA_DIAMETER,AURA_DIAMETER);

		gameControllerRef.UpdatePathNodeBoundsRect (anchorPos);
	}


	private void MakeNextNodeAttempt () {
		float distanceToNextNode = 1f;
		if (!isTravelable) {
			distanceToNextNode *= 0.4f;
		}
		float angleToNextNode = AnchorAngleFromPreviousNodeAnchor + nextNodeAngleDif;//Random.Range (-0.5f, 0.5f);
		Vector3 nextNodePos = anchorPos + new Vector3 (Mathf.Cos (angleToNextNode)*distanceToNextNode, Mathf.Sin (angleToNextNode)*distanceToNextNode, 0);
		AddNextNode (nextNodePos, isTargetPath);
	}

	private void MakeBranchNodeAttempt(bool isLeftTurn, bool doMakeTargetPath) {
		float differenceVolume = Random.Range (0.5f, 1.5f);
		float distanceToNextNode = 1.8f * differenceVolume;
		if (!isTravelable) {
			distanceToNextNode *= 0.4f;
		}
		float angleToNextNode = AnchorAngleFromPreviousNodeAnchor + (isLeftTurn ? -0.5f : 0.5f) * differenceVolume;
		Vector3 nextNodePos = anchorPos + new Vector3 (Mathf.Cos (angleToNextNode)*distanceToNextNode, Mathf.Sin (angleToNextNode)*distanceToNextNode, 0);

		// Make the next node!!
		numNodesUntilBranch = Random.Range (2, 3);
		if (!isTravelable) { numNodesUntilBranch = 1; }
		nextNodeAngleDif = Random.Range (-0.2f, 0.2f);
		AddNextNode (nextNodePos, doMakeTargetPath);
	}
	
	private void AddNextNode(Vector3 nextNodePos, bool doMakeTargetPath) {
		// Creation speed too slow? Ehh, cut it out here, eh.
		float newNodeCreationSpeed = creationSpeed*creationSpeedFriction;
		if (newNodeCreationSpeed < 0.1f) {
			return;
		}
		
		GameObject pathNodePrefab = (GameObject)Resources.Load ("Prefabs/PathNode");
		PathNode newNode = Instantiate (pathNodePrefab).GetComponent<PathNode> ();
		newNode.Initialize (gameControllerRef, nextNodePos, this, numNodesUntilBranch, newNodeCreationSpeed, nextNodeAngleDif, doMakeTargetPath, isTravelable);
		nextNodes.Add (newNode);
	}
	private void AddNextUntravelableNode() {
		float newNodeCreationSpeed = 10*creationSpeedFriction;
		float differenceVolume = Random.Range (0.5f, 1.5f);
		float distanceToNextNode = 1f * differenceVolume;
		float angleToNextNode = AnchorAngleFromPreviousNodeAnchor + (Random.Range (0f,1f)<0.5 ? -0.5f : 0.5f) * differenceVolume;
		Vector3 nextNodePos = anchorPos + new Vector3 (Mathf.Cos (angleToNextNode)*distanceToNextNode, Mathf.Sin (angleToNextNode)*distanceToNextNode, 0);
		
		GameObject pathNodePrefab = (GameObject)Resources.Load ("Prefabs/PathNode");
		PathNode newNode = Instantiate (pathNodePrefab).GetComponent<PathNode> ();
		newNode.Initialize (gameControllerRef, nextNodePos, this, 2, newNodeCreationSpeed, nextNodeAngleDif, false, false);
		nextUntravelableNode = (newNode);
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




	
	// ================================================================
	//	Update
	// ================================================================
	private void Update() {
		// Maybe expire!
		// QQQ
//		familiarity -= 0.02f * Time.deltaTime;
//		if (familiarity <= 0) {
//			Expire ();
//			return;
//		}

		UpdateColor ();
		UpdateSize ();
		UpdateAndApplyVel ();
		
		// Maybe make next untravelable node!
		//*
		if (familiarity > familiarityRequiredToEndorseUntravelableBranch) {
			familiarityRequiredToEndorseUntravelableBranch += 0.6f * familiarity;
			if (nextUntravelableNode == null) {
				AddNextUntravelableNode ();
			}
			else {
				nextUntravelableNode.AddCreationSpeedToChildren(10);
			}
		}
		//*/

		// Already made our next guy? Okay then, don't do anything.
		if (locFromPrevNode >= 1) { return; }
		// No previous node? Don't do anything.
		if (previousNode == null) { return; }

		// Apply friction!
		creationSpeed *= Mathf.Pow(creationSpeedFriction, Time.deltaTime*GameProperties.WORLD_TIME_SCALE);

		// Add to loc!
		float distanceToPreviousNode = Vector3.Distance (anchorPos, previousNode.AnchorPos);
		locFromPrevNode += creationSpeed/distanceToPreviousNode*Time.deltaTime;

		// Maybe make next node(s)!
		if (locFromPrevNode >= 1) {
			// DO branch.
			if (numNodesUntilBranch-- <= 0) {
				nextNodeAngleDif += Random.Range(-0.2f, 0.2f);
				bool isFirstOneMaybeTarget = Random.Range(0f, 1f) < 0.5f;
				MakeBranchNodeAttempt (true, isFirstOneMaybeTarget&&isTargetPath);
				MakeBranchNodeAttempt (false, !isFirstOneMaybeTarget&&isTargetPath);
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
//		float lerpAmount = Mathf.PerlinNoise (transform.localPosition.x/80f, transform.localPosition.y/80f);
//		Color lineColor = Color.Lerp (new Color(0/255f,208/255f,255/255f), new Color(0/255f,64/255f,255/255f), lerpAmount);
		float auraAlpha = Mathf.Min (0.7f, (familiarity-0.15f) * 0.3f);
		float lineAlpha = familiarity;
		Color auraColor;
		Color lineColor;
		if (isTargetPath) {
			auraColor = new Color (13/255f, 255/255f, 0/255f);
			lineColor = new Color (180/255f, 255/255f, 50/255f);
		}
		else if (isTravelable) {
			auraColor = new Color (80/255f, 50/255f, 255/255f);
			lineColor = new Color (60/255f, 255/255f, 255/255f);
		}
		else {
			auraColor = new Color (255/255f, 100/255f, 0/255f);
			lineColor = new Color (255/255f, 200/255f, 0/255f);
		}
		auraColor = new Color (auraColor.r,auraColor.g,auraColor.b, auraAlpha);
		lineColor = new Color (lineColor.r,lineColor.g,lineColor.b, lineAlpha);
		auraSprite.color = auraColor;
		lineSprite.color = lineColor;
		if (!isTravelable) {
			auraSprite.enabled = false;
		}
	}
	
	private void UpdatePosAndSize() {
		UpdatePos ();
		UpdateSize ();
	}
	private void UpdatePos() {
		if (previousNode == null) { return; }
		displayPos = Vector3.Lerp (previousNode.transform.localPosition, transform.localPosition, locFromPrevNode);
		displayAngleBetweenNodes = Mathf.Atan2 (displayPos.y - previousNode.transform.localPosition.y, displayPos.x - previousNode.transform.localPosition.x) * Mathf.Rad2Deg;
		lineSprite.transform.localPosition = Vector3.Lerp (displayPos, previousNode.transform.localPosition, 0.5f) - this.transform.localPosition;
		lineSprite.transform.localEulerAngles = new Vector3 (0, 0, displayAngleBetweenNodes);
	}
	private void UpdateSize() {
		if (previousNode == null) { return; }
		float thickness = 0.05f + familiarity * 0.2f;
		if (!isTravelable) { thickness *= 0.4f; }
		float displayDistanceToPrevNode = Vector3.Distance (displayPos, previousNode.transform.localPosition);
		GameUtils.SizeSprite (lineSprite, displayDistanceToPrevNode, thickness);
	}

	private void UpdateAndApplyVel() {
		// Friction!
		vel *= 0.98f;

		// Ease back to position!
		vel += (anchorPos-transform.localPosition) / 200f;

		// Drift!
		float timeOffset = Time.time*0.05f;
		float posX = anchorPos.x;
		float posY = anchorPos.y;
		float forceX = Mathf.PerlinNoise (posX*0.04f+timeOffset, posY*0.04f+timeOffset) - 0.5f;
		float forceY = Mathf.PerlinNoise (posX*0.038f+200+timeOffset, posY*0.037f-300+timeOffset) - 0.5f;
//		float forceX = Mathf.Sin (timeOffset);
//		float forceY = Mathf.Cos (timeOffset);
//	float distanceToMouse = Vector3.Distance (this.transform.localPosition, gameControllerRef.MousePosWorld);
//	float forceX = (gameControllerRef.MousePosWorld.x - transform.localPosition.x) / distanceToMouse;
//	float forceY = (gameControllerRef.MousePosWorld.y - transform.localPosition.y) / distanceToMouse;
		forceX *= 0.01f;
		forceY *= 0.01f;
//		forceX += Input.GetAxis ("Horizontal") * 0.003f * Mathf.Max(0, 2-familiarity);
//		forceY += Input.GetAxis ("Vertical")   * 0.003f * Mathf.Max(0, 2-familiarity);
		vel += new Vector3 (forceX, forceY);

		this.transform.localPosition += vel;

		UpdatePosAndSize ();
	}


	
	// ================================================================
	//	Events
	// ================================================================
	public void OnTravelerEnterMe(Traveler traveler) {
		// Increase familiarity!
		familiarity += 0.03f;
//		if (familiarity > 1) { familiarity = 1; }
		// Shift charge!
		charge += (traveler.Charge*0.4f-charge) / 8f;
	}
	public void OnTravelerReachMyDeadEnd() {
		creationSpeed = 10f;
		if (isTargetPath) {
			gameControllerRef.OnTravelerReachTargetPathEnd (this);
		}
	}

	public void PushFromTravelerInputForce(Vector2 inputForce) {
		float forceVolumeX = 0.001f; // more is greater impact
		float forceVolumeY = 0.0003f; // more is greater impact
		vel += new Vector3(inputForce.x*forceVolumeX, inputForce.y*forceVolumeY, 0);
	}

	public void AddCreationSpeedToChildren (float _creationSpeedAdd) {
		if (nextNodes.Count == 0) {
			creationSpeed += _creationSpeedAdd;
		}
		else {
			for (int i=0; i<nextNodes.Count; i++) {
				nextNodes[i].AddCreationSpeedToChildren(_creationSpeedAdd / (float)nextNodes.Count);
			}
		}
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









