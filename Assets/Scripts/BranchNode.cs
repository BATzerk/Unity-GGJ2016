using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BranchNode : MonoBehaviour {
	// Properties
	public Vector2 pos;
	public BranchNode previousNode;
	public List<BranchNode> nextNodes;


	// Constructor
	public void Initialize (Vector2 _pos, BranchNode _previousNode, Vector2 sourcePos, float maxDistanceFromSourcePos, int numNodesCreated) {
		pos = _pos;
		this.transform.localPosition = pos;
		previousNode = _previousNode;
		nextNodes = new List<BranchNode> ();
		this.gameObject.name = "BranchNode";
		
		MakeNextNodeAttempt (sourcePos, maxDistanceFromSourcePos, numNodesCreated, true);
		MakeNextNodeAttempt (sourcePos, maxDistanceFromSourcePos, numNodesCreated, false);
	}


	private void MakeNextNodeAttempt(Vector2 sourcePos, float maxDistanceFromSourcePos, int numNodesCreated, bool isLeftTurn) {
		// Created too many nodes? Get outta town!!
		if (numNodesCreated > 3) { return; }

		float angleFromPreviousNode = -Mathf.PI*0.5f; // Default to down.
		if (previousNode != null) {
			angleFromPreviousNode = Mathf.Atan2 (pos.y-previousNode.pos.y, pos.x-previousNode.pos.x);
		}

		float differenceVolume = Random.Range (0.5f, 1.5f);
		float distanceToNextNode = 3 * differenceVolume;
		float angleToNextNode = angleFromPreviousNode + (isLeftTurn ? -0.5f : 0.5f) * differenceVolume;
		Vector2 nextNodePos = pos + new Vector2 (Mathf.Cos (angleToNextNode)*distanceToNextNode, Mathf.Sin (angleToNextNode)*distanceToNextNode);

		// If the distance we want to make the next node(s) is too great, DON'T make a next node!!
		if (Vector2.Distance (sourcePos, nextNodePos) > maxDistanceFromSourcePos) {
			return;
		}

		// Make the next node!!
		BranchNode newNode = new GameObject ().AddComponent<BranchNode> ();
		newNode.Initialize (nextNodePos, this, sourcePos, maxDistanceFromSourcePos, numNodesCreated+1);
		nextNodes.Add (newNode);
	}



	private void OnDrawGizmos () {
		Gizmos.color = Color.white;
		Gizmos.DrawSphere (this.transform.localPosition, 0.1f);
		if (previousNode != null) {
			Gizmos.DrawLine (pos, previousNode.pos);
		}
	}



}









