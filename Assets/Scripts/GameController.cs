using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	// Properties
	[SerializeField] private Player player;
	// Components
	private WorldGenerator worldGenerator;



	void Start () {
		worldGenerator = GetComponent<WorldGenerator> ();

		ResetGame ();
	}
	private void ResetGame() {
		// Generate world!
		worldGenerator.GenerateWorld ();
		// Reset player!
		player.Initialize (worldGenerator.sourceNode, worldGenerator.sourceNode.nextNodes [0]);
	}

	
	
	private void Update() {
		AcceptButtonInput ();
	}
	
	private void AcceptButtonInput() {
		if (Input.GetKeyDown (KeyCode.Return)) {
			ResetGame();
		}
	}
}
