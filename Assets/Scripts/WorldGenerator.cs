using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGenerator : MonoBehaviour {
	// Properties


	void Start () {
		GenerateWorld ();
	}


	private void GenerateWorld() {
		// -- Step 1: Make nodes --
		BranchNode sourceNode = new GameObject ().AddComponent<BranchNode> ();
		sourceNode.Initialize (Vector2.zero, null, Vector2.zero, 2000, 0);


	}


	private void Update() {
		AcceptButtonInput ();
	}

	private void AcceptButtonInput() {
		if (Input.GetKeyDown (KeyCode.Return)) {
			DestroyAllNodes();
			GenerateWorld();
		}
	}


	private void DestroyAllNodes() {
		BranchNode[] allNodes = GameObject.FindObjectsOfType<BranchNode> ();
		for (int i=0; i<allNodes.Length; i++) {
			Destroy (allNodes[i].gameObject);
		}
	}

}




