using UnityEngine;
using System.Collections;

public class BPBig : BPBase {

	
	// ================================================================
	//  Initialize
	// ================================================================
//	public void Initialize (Transform _parentTransform, CameraController _cameraControllerRef) {
//		cameraControllerRef = _cameraControllerRef;
//		parentSystem = _parentSystem;
//		this.transform.SetParent (_parentTransform);
//
//		spriteRenderer = GetComponent<SpriteRenderer> ();
//
//		Spawn ();
//		// Prewarm!
//		timeAlive = Random.Range (0, lifetimeDuration);
//
//		// Add event listeners!
//		GameManagers.Instance.EventManager.CameraPosChangedEvent += OnCameraPosChanged;
//		GameManagers.Instance.EventManager.CameraViewSectorChangedEvent += OnCameraViewSectorChanged;
//	}

	override protected void Spawn() {
		base.Spawn ();

		// What makes ME special!!
		lifetimeDuration = Random.Range (12, 20) * bgDialsRef.LifetimeScale;
		rotationVel = Random.Range (-0.5f, 0.5f);
		float speed = Random.Range (0f, 0.6f);
		float velAngle = Random.Range (0, Mathf.PI * 2);
		vel = new Vector3 (Mathf.Sin (velAngle)*speed, Mathf.Cos(velAngle)*speed, 0);
		float fakedZDistance = Random.Range (0f, 1f); // this is on NO unit scale. It's intentionally arbitrary and to taste.
		parallaxScale = 0.4f;
		diameter = 1600 - fakedZDistance * 600;
//		float hue = ((this.transform.localPosition.x+this.transform.localPosition.y) / 1000f * 0.16f) % 1f;
//		if (hue < 0) hue += 1;
//		baseColor = (new HSBColor (hue, 0.9f, 1f, 0.2f)).ToColor ();
		baseColor = Colors.GetBGParticleBigColor ();
		baseColor.a = 0.10f;
		// Don't forget to make sure my alpha's all right before letting me just pop into existence!
		UpdateAlpha ();
		ApplyAlpha ();

		GameUtils.SizeSprite (spriteRenderer, diameter,diameter);
//		targetScale = spriteRenderer.transform.localScale;
	}
	
	
	// ================================================================
	//  Update
	// ================================================================
	override protected void UpdateAlpha() {
		spriteAlpha = 1 - Mathf.Abs(0.5f - lifetimePercent) * 2; // fade IN, then OUT.
	}

		
		


}



