using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/** BPS = BackgroundParticleSystem */
public class BPSBase : MonoBehaviour {
	// Components
	protected List<BPBase> particles;
	// Properties
	protected int numParticles;
	// References
	private GameBackground gameBackgroundRef;
	private GameObject particlePrefab;
	private CameraController cameraControllerRef;
	
	// Overridable properties
	virtual protected string ParticlePrefabName { get { return null; } }
	
	
	
	// ================================================================
	//  Initialize
	// ================================================================
	protected void Awake () {
		// Identify references!
		gameBackgroundRef = this.transform.parent.gameObject.GetComponent<GameBackground> ();
		cameraControllerRef = gameBackgroundRef.GetCameraControllerRef ();

		particlePrefab = (GameObject)Resources.Load (GameProperties.GAME_BACKGROUND_PREFABS_PATH + ParticlePrefabName);
		
		InitializeParticles ();
	}
	private void InitializeParticles() {
		DestroyAllParticles ();
		particles = new List<BPBase> ();
	}
	
	
	
	// ================================================================
	//  Update
	// ================================================================
	void Update () {
		
	}
	
	
	
	// ================================================================
	//  Events
	// ================================================================
	virtual public void SetNumParticles(int _numParticles) {
		numParticles = _numParticles;
		// Add particles!
		while (particles.Count < numParticles) {
			BPBase particle = ((GameObject)Instantiate(particlePrefab)).GetComponent<BPBase>();
			particle.Initialize (this.transform, gameBackgroundRef.BGDials, cameraControllerRef);
			particles.Add (particle);
		}
		// Tell all particles if they're to respawn or not!
		for (int i=0; i<particles.Count; i++) {
			particles [i].DoRespawn = i < numParticles;
		}
	}
//	public void SetParticleLifetimeScale (float _lifetimeScale) {
//		for (int i=0; i<particles.Count; i++) {
//			particles [i].SetLifetimeScale (_lifetimeScale);
//		}
//	}
	
	
	
	// ================================================================
	//  Destroy!
	// ================================================================
	private void DestroyAllParticles() {
		if (particles != null) {
			for (int i=0; i<particles.Count; i++) {
				Destroy (particles[i].gameObject);
			}
			particles = null;
		}
	}
	
	
	
}



