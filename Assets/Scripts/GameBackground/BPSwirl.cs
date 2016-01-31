using UnityEngine;
using System.Collections;

public class BPSwirl : BPBase {
	// Properties
	private float alphaOscillationSpeed; // the higher this is, the more I look like a strobe light.

	
	// ================================================================
	//  Spawn
	// ================================================================
	override protected void Spawn() {
		base.Spawn ();
		
		// What makes ME special!!
		rotationVel = Random.Range (-1.6f, 1.6f);
		vel = Vector3.zero;
		float fakedZDistance = Random.Range (0f, 1f); // this is on NO unit scale. It's intentionally arbitrary and to taste.
		parallaxScale = Mathf.Lerp (0.3f, 0.64f, fakedZDistance);
		lifetimeDuration = Mathf.Lerp (3f, 24f, fakedZDistance) * bgDialsRef.LifetimeScale;
		alphaOscillationSpeed = Random.Range (1.6f, 3f);
//		spriteAlpha = 0.6f;
		diameter = 400 - fakedZDistance*300;
		diameterVel = 0.6f + fakedZDistance * 0.8f;
		SetBaseColorFromPos ();

		// Don't forget to make sure my alpha's all right before letting me just pop into existence!
		UpdateAlpha ();
		ApplyAlpha ();
		
		GameUtils.SizeSprite (spriteRenderer, diameter,diameter);
	}

	private void SetBaseColorFromPos() {
		baseColor = Colors.GetBGParticleSwirlColor (transform.localPosition);
	}

	override protected void OnChangedPosToStayInBounds() {
		SetBaseColorFromPos ();
	}
	
	
	// ================================================================
	//  Update
	// ================================================================
	override protected void UpdateAlpha() {
//		spriteAlpha = (1-lifetimePercent) * 0.4f;
		spriteAlpha = (1 - Mathf.Abs(0.5f-lifetimePercent)*2) * 0.3f; // fade IN, then OUT.
		// Now compound that base alpha with some oscillation dancin'!
		spriteAlpha *= (0.8f - Mathf.Sin(timeAlive*alphaOscillationSpeed)*0.2f);
	}
	override protected void ApplyRotationVel () {
		this.transform.localEulerAngles += new Vector3 (0, 0, rotationVel*bgDialsRef.StillToKinetic*Time.timeScale);
	}
	
	
	
	
	
}



