using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BranchNode : MonoBehaviour {
	// Properties
	public Vector2 pos;
	public BranchNode previousNode;
	public List<BranchNode> nextNodes;


	// Constructor
	public void Initialize (Vector2 _pos, BranchNode _previousNode, Vector2 sourcePos, float maxDistanceFromSourcePos) {
		pos = _pos;
		this.transform.localPosition = pos;
		previousNode = _previousNode;
		nextNodes = new List<BranchNode> ();
		this.gameObject.name = "BranchNode";
		
		MakeNextNodeAttempt (sourcePos, maxDistanceFromSourcePos);
//		MakeNextNodeAttempt (sourcePos, maxDistanceFromSourcePos);
	}


	private void MakeNextNodeAttempt(Vector2 sourcePos, float maxDistanceFromSourcePos) {
		float angleFromPreviousNode = 0;
		if (previousNode != null) {
			angleFromPreviousNode = Mathf.Atan2 (previousNode.pos.y-pos.y, previousNode.pos.x-pos.x);
		}

		float angleToNextNode = 0.1f;//angleFromPreviousNode + Random.Range (-20f, 20f)*Mathf.Deg2Rad;
		float distanceToNextNode = 20;//Random.Range (400, 800);
		Vector2 nextNodePos = pos + new Vector2 (Mathf.Cos (angleToNextNode)*distanceToNextNode, Mathf.Sin (angleToNextNode)*distanceToNextNode);

		// If the distance we want to make the next node(s) is too great, DON'T make a next node!!
		if (Vector2.Distance (sourcePos, nextNodePos) > maxDistanceFromSourcePos) {
			return;
		}

		// Make the next node!!
		BranchNode newNode = new GameObject ().AddComponent<BranchNode> ();
		newNode.Initialize (nextNodePos, this, sourcePos, maxDistanceFromSourcePos);
		nextNodes.Add (newNode);
	}



	private void OnDrawGizmos () {
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere (this.transform.localPosition, 0.5f);
	}



}









