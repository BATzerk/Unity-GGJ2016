using UnityEngine;
using System.Collections;

public class BPEddyGlitter : BPBase {
	// Properties
//	private float alphaOscillationSpeed; // the higher this is, the more I look like a strobe light.
//	private Color colorA, colorB;
//	private float sparkleVolume; // almost always at 0. But when it's NOT, I'll lerp from my color to white (and back).

	
	// ================================================================
	//  Spawn
	// ================================================================
	override protected void Spawn() {
		base.Spawn ();
		
		lifetimeDuration = Random.Range (1f, 3f) * bgDialsRef.LifetimeScale;// lifetimeDurationOnSpawnScale;
		rotationVel = Random.Range (-0.5f, 0.5f);
//		alphaOscillationSpeed  = Random.Range (4f, 8f);
		parallaxScale = Random.Range(0.2f, 0.6f);
		diameter = 9f - parallaxScale * 10f;
//		sparkleVolume = 0;
		// HACKY access to worldIndex
//		int worldIndex = GameManagers.Instance.DataManager.worldIndexOnLoadGameScene;
//		colorA = Color.Lerp (Colors.GetBGParticleColorA (worldIndex), new Color(1,1,1, 0.1f), 0.4f);
//		colorB = Color.Lerp (Colors.GetBGParticleColorB (worldIndex), new Color(1,1,1, 0.1f), 0.5f);
		SetBaseColorFromPos ();
		// Don't forget to make sure my alpha's all right before letting me just pop into existence!
		UpdateAlpha ();
		ApplyAlpha ();
		
		GameUtils.SizeSprite (spriteRenderer, diameter,diameter);
	}
	
	private void SetBaseColorFromPos() {
		baseColor = Colors.GetBGParticleEddyGlitterColor (transform.localPosition);
	}
	
	override protected void OnChangedPosToStayInBounds() {
		SetBaseColorFromPos ();
	}
	
	
	// ================================================================
	//  Update
	// ================================================================
	override protected void Update() {
		UpdateVel ();
		base.Update ();
	}

	private void UpdateVel() {
		vel *= 0.8f;

		float timeOffset = Time.time*0.06f;
		float posX = this.transform.localPosition.x;
		float posY = this.transform.localPosition.y;
		float forceX = Mathf.PerlinNoise (posX*0.004f+timeOffset, posY*0.004f+timeOffset) - 0.5f;
		float forceY = Mathf.PerlinNoise (posX*0.0038f+200+timeOffset, posY*0.0037f-300+timeOffset) - 0.5f;
		forceX *= parallaxScale * bgDialsRef.StillToKinetic*2f;
		forceY *= parallaxScale * bgDialsRef.StillToKinetic*2f;
		vel += new Vector3 (forceX, forceY);
	}

	override protected void UpdateAlpha() {
		// HACK putting this in here
//		baseColor = Color.Lerp (colorA, colorB, lifetimePercent);
//		int worldIndex = GameManagers.Instance.DataManager.worldIndexOnLoadGameScene;
//		baseColor = Colors.GetBGParticleSwirlColor (worldIndex, transform.localPosition);

//		baseColor = Color.Lerp (colorA, colorB, 1-vel.magnitude*2f);

//		if (Random.Range (0f, 1f) < 0.001) {
//			sparkleVolume = 1;
//		}
//		if (sparkleVolume > 0.0001) {
//			baseColor = Color.Lerp (baseColor, Color.white, sparkleVolume);
//			sparkleVolume += (0-sparkleVolume) / 5f;
//		}


		if (lifetimePercent < 0.3f) {
			spriteAlpha = lifetimePercent / 0.3f;
		} else if (lifetimePercent > 0.6f) {
			spriteAlpha = (1 - lifetimePercent) / 0.4f;
		} else
			spriteAlpha = 1;
		// Scale down alpha, actually.
		spriteAlpha *= 0.7f;
		// Now compound that base alpha with some oscillation dancin'!
//		spriteAlpha *= (0.6f - Mathf.Sin(timeAlive*alphaOscillationSpeed)*0.4f);
	}
	
	
	
	
}



