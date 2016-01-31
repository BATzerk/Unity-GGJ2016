using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour {
	// Properties
	public PathNode sourceNode;
	// References
	private GameObject pathNodePrefab;


	private void Awake() {
		pathNodePrefab = (GameObject)Resources.Load ("Prefabs/PathNode");
	}


	public void GenerateWorld(GameController gameControllerRef) {
		DestroyAllNodes ();

		// Make the first TWO nodes.
		sourceNode = Instantiate (pathNodePrefab).GetComponent<PathNode> ();
		sourceNode.Initialize (gameControllerRef, Vector2.zero, null, 10, 0, 0);
		PathNode secondNode = Instantiate (pathNodePrefab).GetComponent<PathNode> ();
		secondNode.Initialize (gameControllerRef, new Vector2(0,0.5f), null, 4, 50, 0);
		sourceNode.nextNodes.Add (secondNode);
		secondNode.previousNode = sourceNode;
	}



	private void DestroyAllNodes() {
		PathNode[] allNodes = GameObject.FindObjectsOfType<PathNode> ();
		for (int i=0; i<allNodes.Length; i++) {
			Destroy (allNodes[i].gameObject);
		}
	}

}




