using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathNode : MonoBehaviour {
	// Properties
	public Vector2 pos;
	public PathNode previousNode;
	public List<PathNode> nextNodes;

	// Getters
	private float angleFromPreviousNode {
		get {
			if (previousNode != null) {
				return Mathf.Atan2 (pos.y - previousNode.pos.y, pos.x - previousNode.pos.x);
			}
			return -Mathf.PI * 0.5f; // Default to down.
		}
	}


	// Constructor
	public void Initialize (Vector2 _pos, PathNode _previousNode, int numNodesUntilBranch, Vector2 sourcePos, float maxDistanceFromSourcePos, int numBranchesCreated) {
		pos = _pos;
		this.transform.localPosition = pos;
		previousNode = _previousNode;
		nextNodes = new List<PathNode> ();
		this.gameObject.name = "BranchNode";

		// DO branch.
		if (numNodesUntilBranch-- <= 0) {
			numBranchesCreated ++;
			MakeBranchNodeAttempt (sourcePos, maxDistanceFromSourcePos, numBranchesCreated, true);
			MakeBranchNodeAttempt (sourcePos, maxDistanceFromSourcePos, numBranchesCreated, false);
		}
		else {
			MakeNextNodeAttempt (numNodesUntilBranch, sourcePos, maxDistanceFromSourcePos, numBranchesCreated);
		}
	}

	private void MakeNextNodeAttempt (int numNodesUntilBranch, Vector2 sourcePos, float maxDistanceFromSourcePos, int numBranchesCreated) {
		float distanceToNextNode = 0.3f;
		float angleToNextNode = angleFromPreviousNode;
		Vector2 nextNodePos = pos + new Vector2 (Mathf.Cos (angleToNextNode)*distanceToNextNode, Mathf.Sin (angleToNextNode)*distanceToNextNode);
		AddNextNode (nextNodePos, numNodesUntilBranch, sourcePos, maxDistanceFromSourcePos, numBranchesCreated);
	}

	private void MakeBranchNodeAttempt(Vector2 sourcePos, float maxDistanceFromSourcePos, int numBranchesCreated, bool isLeftTurn) {
		float differenceVolume = Random.Range (0.5f, 1.5f);
		float distanceToNextNode = 0.3f * differenceVolume;
		float angleToNextNode = angleFromPreviousNode + (isLeftTurn ? -0.5f : 0.5f) * differenceVolume;
		Vector2 nextNodePos = pos + new Vector2 (Mathf.Cos (angleToNextNode)*distanceToNextNode, Mathf.Sin (angleToNextNode)*distanceToNextNode);

		// Make the next node!!
		int numNodesUntilBranch = Random.Range (10, 20);
		AddNextNode (nextNodePos, numNodesUntilBranch, sourcePos, maxDistanceFromSourcePos, numBranchesCreated);
	}

	private void AddNextNode(Vector2 nextNodePos, int numNodesUntilBranch, Vector2 sourcePos, float maxDistanceFromSourcePos, int numBranchesCreated) {
		// Created too many nodes? Get outta town!!
		if (numBranchesCreated > 3) {
			return;
		}
		// If the distance we want to make the next node(s) is too great, DON'T make a next node!!
		if (Vector2.Distance (sourcePos, nextNodePos) > maxDistanceFromSourcePos) {
			return;
		}

		PathNode newNode = new GameObject ().AddComponent<PathNode> ();
		newNode.Initialize (nextNodePos, this, numNodesUntilBranch, sourcePos, maxDistanceFromSourcePos, numBranchesCreated);
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









