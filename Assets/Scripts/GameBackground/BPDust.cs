using UnityEngine;
using System.Collections;

public class BPDust : BPBase {
	// Properties
	private float alphaOscillationSpeed; // the higher this is, the more I look like a strobe light.
	
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

		lifetimeDuration = Random.Range (3, 10) * bgDialsRef.LifetimeScale;
		rotationVel = Random.Range (-2, 2);
		float speed = Random.Range (0f, 0.2f) * bgDialsRef.StillToKinetic;
		float velAngle = Random.Range (0, Mathf.PI * 2);
		vel = new Vector3 (Mathf.Sin (velAngle)*speed, Mathf.Cos(velAngle)*speed, 0);
		alphaOscillationSpeed = Random.Range (2f, 4.6f);
		parallaxScale = Random.Range(0.1f, 0.4f);
		diameter = 7 - parallaxScale * 10;
//		baseColor = Colors.GetBGParticleDustColor (worldIndex, transform.localPosition);
		baseColor = Color.white;
//		spriteRenderer.color = baseColor;
		// Don't forget to make sure my alpha's all right before letting me just pop into existence!
		UpdateAlpha ();
		ApplyAlpha ();
		
		GameUtils.SizeSprite (spriteRenderer, diameter,diameter);
	}
	
	
	// ================================================================
	//  Update
	// ================================================================
	override protected void UpdateAlpha() {
		// Fade in quick; fade out slower
		if (lifetimePercent < 0.1f) {
			spriteAlpha = lifetimePercent / 0.1f;
		} else if (lifetimePercent > 0.7f) {
			spriteAlpha = (1 - lifetimePercent) / 0.3f;
		} else
			spriteAlpha = 1;
		// Now compound that base alpha with some oscillation dancin'!
		spriteAlpha *= (0.6f - Mathf.Sin(timeAlive*alphaOscillationSpeed)*0.4f);
	}
	
	
	
	
}



