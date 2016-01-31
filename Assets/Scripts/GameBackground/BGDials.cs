using UnityEngine;
using System.Collections;

/** This class is the conductor of the orchestra. All BGParticles have references to this class. Changing values here will affect the particles' behavior.
 This class is intended to be an easier way to change the feel of the background with only a few simple dials. */
public class BGDials : MonoBehaviour {
	// Properties
	[SerializeField] private float lifetimeScale;
	[SerializeField] private float lifetimeScaleTarget;
	[SerializeField] private float stillToKinetic;
	[SerializeField] private float stillToKineticTarget;

	// Getters
	public float LifetimeScale { get { return lifetimeScale; } }
	public float StillToKinetic { get { return stillToKinetic; } }

	
	// ================================================================
	//  Initialize
	// ================================================================
	private void Start() {
		ResetValuesToDefault ();
	}

	private void ResetValuesToDefault() {
		lifetimeScale = lifetimeScaleTarget = BGDialsData.DEFAULT_LIFETIME_SCALE;
		stillToKinetic = stillToKineticTarget = BGDialsData.DEFAULT_STILL_TO_KINETIC;
	}

	
	// ================================================================
	//  Events
	// ================================================================
//	public void OnGameControllerSetCurrentLevel (Level level) {
//		BGDialsData bgDialsData = level.LevelDataRef.bgDialsData;
//		lifetimeScaleTarget = bgDialsData.lifetimeScale;
//		stillToKineticTarget = bgDialsData.stillToKinetic;
//	}

	
	// ================================================================
	//  Update
	// ================================================================
	private void Update () {
		EaseValuesTowardsTargets ();
	}
	private void EaseValuesTowardsTargets() {
		if (lifetimeScale != lifetimeScaleTarget) {
			lifetimeScale += (lifetimeScaleTarget - lifetimeScale) / 100f;
		}
		if (stillToKinetic != stillToKineticTarget) {
			stillToKinetic += (stillToKineticTarget - stillToKinetic) / 100f;
		}
	}


}






