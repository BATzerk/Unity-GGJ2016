using UnityEngine;
using System.Collections;

public class GameBackground : MonoBehaviour {
	// Properties
	private int pworldIndex = -1; // previous worldIndex. When this changes, I destroy/remake ALL particle lists!
	// References
	[SerializeField] private CameraController cameraControllerRef;
	[SerializeField] private BPSBig particlesBig;
	[SerializeField] private BPSDust particlesDust;
	[SerializeField] private BPSSwirl particlesSwirl;
	[SerializeField] private BPSEddyGlitter particlesEddyGlitter;
	[SerializeField] private BPSLensFloater particlesLensFloater;
	private BGDials bgDials;

	// Getters
	public BGDials BGDials { get { return bgDials; } }
	public CameraController GetCameraControllerRef() { return cameraControllerRef; }

	
	
	// ================================================================
	//  Initialize
	// ================================================================
	void Start () {
		// Identify components!
		bgDials = GetComponent<BGDials> ();

		// Default background to look like dis!
		particlesSwirl.SetNumParticles (20);
		particlesEddyGlitter.SetNumParticles (120);

		// Add event listeners!
//		GameManagers.Instance.EventManager.GameControllerSetCurrentLevelEvent += OnGameControllerSetCurrentLevel;
	}

//	private void RecreateAllParticles() {
//		particlesBig.RecreateParticleList ();
//		particlesDust.RecreateParticleList ();
//		particlesSwirl.RecreateParticleList ();
//		particlesEddyGlitter.RecreateParticleList ();
//		particlesLensFloater.RecreateParticleList ();
//	}
	
	
	
	// ================================================================
	//  Update
	// ================================================================
	void Update () {

	}

	
	// ================================================================
	//  Events
	// ================================================================
	/*
	private void OnGameControllerSetCurrentLevel (Level currentLevel) {
		int worldIndex = currentLevel.LevelDataRef.WorldIndex;
		// Is this different from the PREVIOUS world??
//		if (worldIndex != pworldIndex) {
//			RecreateAllParticles ();
//			pworldIndex = worldIndex;
//		}

		bgDials.OnGameControllerSetCurrentLevel (currentLevel);

		// Default every system to 0 first.
		particlesBig.SetNumParticles (0);
		particlesDust.SetNumParticles (0);
		particlesSwirl.SetNumParticles (0);
		particlesEddyGlitter.SetNumParticles (0);
		particlesLensFloater.SetNumParticles (0);

		switch (worldIndex) {
		// ---- World 1 ----
		case 1:
			particlesBig.SetNumParticles (6);
			particlesDust.SetNumParticles (30);
			particlesLensFloater.SetNumParticles (8);
			break;
		// ---- World 2 ----
		case 2:
			particlesBig.SetNumParticles (6);
			particlesDust.SetNumParticles (140);
			particlesLensFloater.SetNumParticles (30);
			break;
		// ---- World 3 ----
		case 3:
			particlesBig.SetNumParticles (6);
			particlesEddyGlitter.SetNumParticles (100);
			particlesLensFloater.SetNumParticles (16);
			break;
		// ---- World 4 ----
		case 4:
			//			particlesDust.SetNumParticles (60);
			//			particlesDust.SetParticleLifetimeScale (0.2f);
			//			particlesLensFloater.SetNumParticles (18);
			
			particlesSwirl.SetNumParticles (20);
			particlesEddyGlitter.SetNumParticles (120);
			break;
		// ---- World 5 ----
		case 5:
			particlesBig.SetNumParticles (6);
			particlesEddyGlitter.SetNumParticles (200);
			break;
			// ---- World 6 ----
		case 6:
			particlesDust.SetNumParticles (40);
			break;
			// ---- World 7 ----
		case 7:
			particlesBig.SetNumParticles (6);
			particlesEddyGlitter.SetNumParticles (200);
			break;
		// ---- Default ----
		default:
			break;
		}
	}
	*/


	
	// ================================================================
	//  Destroy!
	// ================================================================
	private void OnDestroy() {
		// Remove event listeners!
//		GameManagers.Instance.EventManager.GameControllerSetCurrentLevelEvent -= OnGameControllerSetCurrentLevel;
	}



}




