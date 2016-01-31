using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BPSLensFloater : BPSBase {
	// Overridable properties
	override protected string ParticlePrefabName { get { return "BPLensFloater"; } }



	// ================================================================
	//  Events
	// ================================================================
	override public void SetNumParticles(int _numParticles) {
		base.SetNumParticles (_numParticles);
		
		// Set parallaxScales! So there's always an even distribution.
		for (int i=0; i<numParticles; i++) {
			(particles [i] as BPLensFloater).SetParallaxScale ((0.5f - (i/(float)numParticles)) * 0.3f);
		}
	}
	


	
	
	
}



