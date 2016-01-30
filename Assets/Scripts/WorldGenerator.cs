using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour {
	// Properties
	public PathNode sourceNode;



	public void GenerateWorld() {
		DestroyAllNodes ();

		sourceNode = new GameObject ().AddComponent<PathNode> ();
		sourceNode.Initialize (Vector2.zero, null, 10, Vector2.zero, 20f, 0);
	}



	private void DestroyAllNodes() {
		PathNode[] allNodes = GameObject.FindObjectsOfType<PathNode> ();
		for (int i=0; i<allNodes.Length; i++) {
			Destroy (allNodes[i].gameObject);
		}
	}

}




