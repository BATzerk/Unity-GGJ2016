using UnityEngine;
using System.Collections;

public class BPLensFloater : BPBase {
	
	// ================================================================
	//  Spawn
	// ================================================================
	override protected void Spawn() {
		base.Spawn ();
		
		lifetimeDuration = Random.Range (3, 9) * bgDialsRef.LifetimeScale;
		rotationVel = Random.Range (-2, 2);
		float speed = Random.Range (0, 0.2f) * -(parallaxScale-1);
		float velAngle = Random.Range (0, Mathf.PI * 2);
		vel = new Vector3 (Mathf.Sin (velAngle)*speed, Mathf.Cos(velAngle)*speed, 0);
		baseColor = Color.white;
		// Don't forget to make sure my alpha's all right before letting me just pop into existence!
		UpdateAlpha ();
		ApplyAlpha ();
	}
	public void SetParallaxScale(float _parallaxScale) {
		parallaxScale = _parallaxScale;
		diameter = 2 + Mathf.Abs(parallaxScale)*60f;
		GameUtils.SizeSprite (spriteRenderer, diameter,diameter);
	}
	
	
	// ================================================================
	//  Update
	// ================================================================
	override protected void UpdateAlpha() {
		// Fade in quick; fade out slower
		if (lifetimePercent < 0.3f) {
			spriteAlpha = lifetimePercent / 0.3f;
		} else if (lifetimePercent > 0.7f) {
			spriteAlpha = (1 - lifetimePercent) / 0.3f;
		} else
			spriteAlpha = 1;
		// Scale down alpha, actually.
		spriteAlpha *= 0.07f;
	}
	
	
	
	
}



