using UnityEngine;
using System.Collections;

public class Traveler : MonoBehaviour {
	// Components
	[SerializeField] private SpriteRenderer auraSprite;
	[SerializeField] private SpriteRenderer bodySprite;
	// Properties
	private float loc; // from 0 to 1; nodePrev to nodeNext
	private float charge;
	private float diameter;
	private float speed;
	private Vector2 inputForce;
	// References
	private GameController gameControllerRef;
	public PathNode nodePrev;
	public PathNode nodeNext;
	private PathNode sourceNode;


	// Getters
	public float Charge { get { return charge; } }
	private PathNode GetProjectedNextNode() {
		// Only one? Just use that one duhh.
		if (nodeNext.nextNodes.Count < 2) {
			return nodeNext.nextNodes [0];
		}
		// More than one! Which do we go to??
		else {
			int bestFitIndex = -1;
			float bestFitDistance = 999999;
			for (int i=0; i<nodeNext.nextNodes.Count; i++) {
				float distanceToMouse = Vector2.Distance(gameControllerRef.MousePosWorld, new Vector2(nodeNext.nextNodes[i].transform.localPosition.x,nodeNext.nextNodes[i].transform.localPosition.y));
				if (bestFitDistance > distanceToMouse) {
					bestFitDistance = distanceToMouse;
					bestFitIndex = i;
				}
			}
			// There's a small chance we won't use the value we calculated.
			if (Random.Range(0f,1f)<0.1f || true) { bestFitIndex = Random.Range(0, nodeNext.nextNodes.Count); } // QQQ
			return nodeNext.nextNodes[bestFitIndex];
		}
	}


	// Initialize!
	public void Initialize (GameController _gameControllerRef, PathNode _sourceNode, float _charge) {
		gameControllerRef = _gameControllerRef;
		sourceNode = _sourceNode;
		charge = _charge;
		ReturnToSourceNode ();
		loc = 0;
		speed = Random.Range (0.1f, 0.3f);
		diameter = 0.1f;
		auraSprite.color = new Color (255/255f, 140/255f, 0/255f, 0.2f);
		bodySprite.color = new Color (255/255f, 234/255f, 0/255f);
	}

	private void ReturnToSourceNode() {
		nodePrev = sourceNode;
		nodeNext = null; // this'll get set automatically.
		loc = 0;
	}

	void Update () {
		// No next node?? See if we can make me one yet.
		if (nodeNext == null) {
			if (nodePrev!=null && nodePrev.nextNodes.Count>0) {
				nodeNext = nodePrev.nextNodes[0];
			}
		}

		// No nodes? Do nothin'!
		if (nodePrev == null || nodeNext == null) { return; }

		UpdateInputForce ();
		UpdateSpeed ();
		UpdateLoc ();
		UpdatePosition ();
		UpdateSize ();
	}

	private void UpdateInputForce() {
		inputForce = new Vector2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"));
	}

	private void UpdateSpeed () {
//		speed += (-Input.GetAxis ("Vertical")*0.06f - speed) / 8f;
	}

	private void UpdateLoc () {
		// Update loc!
		float distBetweenMyNodes = Vector2.Distance(nodePrev.transform.localPosition, nodeNext.transform.localPosition);
		loc += speed / distBetweenMyNodes * Time.timeScale;
		
		// Off the line??
		if (loc > 1) {
			// NO next node?? Go back to the source!
			if (nodeNext.nextNodes.Count == 0) {
				nodeNext.OnTravelerReachMyDeadEnd ();
				ReturnToSourceNode ();
			}
			// YES next node? Go onto it!
			else {
				// Determine which next node to go to!
				PathNode _nodePrev = nodeNext;
				PathNode _nodeNext = GetProjectedNextNode();
				nodePrev = _nodePrev;
				nodeNext = _nodeNext;
				nodeNext.OnTravelerEnterMe (this);
				loc -= 1;
			}
		}
		else if (loc < 0) {
			PathNode _nodePrev = nodePrev.previousNode;
			PathNode _nodeNext = nodePrev;
			nodePrev = _nodePrev;
			nodeNext = _nodeNext;
			loc += 1;
		}
	}

	private void UpdatePosition() {
		if (nodeNext == null) { return; }
		this.transform.localPosition = Vector2.LerpUnclamped (nodePrev.transform.localPosition, nodeNext.transform.localPosition, loc);
	}

	private void UpdateSize() {
		float displayDiameter = diameter;
		if (nodeNext != null && nodeNext.nextNodes.Count == 0) {
			displayDiameter *= 1-loc; // Shrink before I disappear!
		}
		
		GameUtils.SizeSprite (auraSprite, displayDiameter*24, displayDiameter*24);
		GameUtils.SizeSprite (bodySprite, displayDiameter, displayDiameter);
	}


}






