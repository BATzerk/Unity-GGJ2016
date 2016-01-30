using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	// Properties
	private float loc; // from 0 to 1; nodePrev to nodeNext
	private float speed;
//	private float
	// References
	private PathNode nodePrev;
	private PathNode nodeNext;


	public void Initialize (PathNode _nodePrev, PathNode _nodeNext) {
		nodePrev = _nodePrev;
		nodeNext = _nodeNext;
		loc = 0;
		speed = 0.1f;
	}
	
	void Update () {
		// No nodes? Do nothin'!
		if (nodePrev == null || nodeNext == null) { return; }

		UpdateLoc ();
		UpdatePosition ();
	}

	private void UpdateLoc () {
		// Update loc!
		float distBetweenMyNodes = Vector2.Distance(nodePrev.pos, nodeNext.pos);
		loc += speed / distBetweenMyNodes;
		
		// Off the line??
		if (loc > 1) {
			// Determine which next node to go to!
			// TODO: ^This.
			PathNode _nodePrev = nodeNext;
			PathNode _nodeNext = nodeNext.nextNodes[0]; // TODO: Not the first one alwayz
			nodePrev = _nodePrev;
			nodeNext = _nodeNext;
			loc -= 1;
		}
	}

	private void UpdatePosition() {
		this.transform.localPosition = Vector2.LerpUnclamped (nodePrev.pos, nodeNext.pos, loc);
	}

}






