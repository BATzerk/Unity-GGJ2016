using UnityEngine;
using System.Collections;

public class BPBase : MonoBehaviour {
	// Properties
	private bool doRespawn = true;
	protected Color baseColor;
	private float masterAlpha = 1; // all of my sprites' alphas are finally scaled by this
	protected float spriteAlpha; // the alpha of the sprite *before* affected by masterAlpha
//	protected float spriteAlphaVel; // the alpha of the sprite *before* affected by masterAlpha
	protected float diameter;
	protected float diameterVel;
	protected float rotationVel;
	protected float parallaxScale; // 0 is 1:1 with game world (e.g. streets); 1 is 1:1 with camera's position (e.g. the moon); -0.5 is between the camera and the streets, all up in the player's face.
	protected float lifetimeDuration;
//	protected float lifetimeDurationOnSpawnScale = 1; // my particle system can use this to make me last artificially longer/shorter.
	protected float lifetimePercent; // from 0 to 1. lifetimeDuration / timeAlive.
	protected float timeAlive;
	protected Vector3 vel;
	// Components
	protected SpriteRenderer spriteRenderer;
	// References
	protected BGDials bgDialsRef;
	protected CameraController cameraControllerRef;

	// Getters / Setters
	public bool DoRespawn {
		get { return doRespawn; }
		set {
			doRespawn = value;
			// Am I to respawn but I'm NOT currently alive? Spawn me ASAP, broseph!
			if (doRespawn && timeAlive<0) {
				// Hackyish, non-visually-consistent delay between spawning particles. (Ideally, we'd use the particles' lifetime for this delay, but it's not that big a deal.)
				Invoke ("Spawn", Random.Range(0f, 5f));
			}
		}
	}

	
	// ================================================================
	//  Initialize
	// ================================================================
	public void Initialize (Transform _parentTransform, BGDials _bgDialsRef, CameraController _cameraControllerRef) {
		bgDialsRef = _bgDialsRef;
		cameraControllerRef = _cameraControllerRef;
		this.transform.SetParent (_parentTransform);
		
		spriteRenderer = GetComponent<SpriteRenderer> ();
		
		// Just gimmie some defaults for shiggles.
		lifetimeDuration = 5 * bgDialsRef.LifetimeScale;// * lifetimeDurationOnSpawnScale;
		vel = Vector3.zero;
//		spriteAlphaVel = 0;
		diameterVel = 0;
		rotationVel = 0;
		parallaxScale = 0;
		baseColor = Color.white;
		spriteRenderer.color = baseColor;
		GameUtils.SizeSprite (spriteRenderer, diameter,diameter);

		// Spawn and prewarm!
		Spawn ();
		timeAlive = Random.Range (0, lifetimeDuration);
		
		// Add event listeners!
//		GameManagers.Instance.EventManager.CameraPosChangedEvent += OnCameraPosChanged;
//		GameManagers.Instance.EventManager.CameraViewSectorChangedEvent += OnCameraViewSectorChanged;
//		GameManagers.Instance.EventManager.CameraZoomChangedEvent += OnCameraZoomChanged;
	}


	virtual protected void Spawn() {
		// Reset-'em-s
		timeAlive = 0;
		lifetimePercent = 0;
		spriteRenderer.enabled = true;
		
		// Plop me somewhere random on the screen
//		Rect viewRect = cameraControllerRef.GetViewRectOfViewSector ();
		Rect viewRect = new Rect (0,0, 1500,1000);
		this.transform.localPosition = new Vector3 (viewRect.x + Random.Range(-viewRect.width,viewRect.width)*0.5f, viewRect.y + Random.Range(-viewRect.height,viewRect.height)*0.5f, 0);
	}
	
	
	// ================================================================
	//  Update
	// ================================================================
	virtual protected void Update () {
		UpdateLifetime ();
		UpdateAlpha ();
		UpdateAndApplyDiameter ();
		ApplyAlpha ();
		ApplyRotationVel ();
		ApplyVel ();
	}
	

	virtual protected void ApplyRotationVel () {
		this.transform.localEulerAngles += new Vector3 (0, 0, rotationVel*Time.timeScale);
	}
	private void ApplyVel() {
		this.transform.localPosition += vel*Time.timeScale;
	}
	virtual protected void UpdateAlpha() {
//		spriteAlpha += spriteAlphaVel*Time.timeScale;
	}
	protected void ApplyAlpha() {
		spriteRenderer.color = new Color (baseColor.r,baseColor.g,baseColor.b, baseColor.a*spriteAlpha*masterAlpha);
	}
	private void UpdateLifetime () {
		timeAlive += Time.deltaTime;
		lifetimePercent = timeAlive / lifetimeDuration;
		if (lifetimePercent > 1) {
			OnLifetimeEnded();
		}
	}
	private void UpdateAndApplyDiameter() {
		if (diameterVel != 0) { // Optimization: If there's no change in diameter, don't do anything.
			diameter += diameterVel * Time.timeScale;
			GameUtils.SizeSprite (spriteRenderer, diameter,diameter);
		}
	}



	protected void SetMasterAlpha(float _masterAlpha) {
		masterAlpha = _masterAlpha;
		UpdateAlpha ();
	}
//	public void SetLifetimeScale(float _lifetimeScale) {
//		lifetimeDurationOnSpawnScale = _lifetimeScale;
//	}

	
	// ================================================================
	//  Events
	// ================================================================
	private void OnLifetimeEnded () {
		if (doRespawn) {
			Spawn ();
		}
		else {
			timeAlive = -1;
			spriteRenderer.enabled = false;
		}
	}
	private void OnCameraPosChanged (Vector3 deltaPos) {
		this.transform.localPosition -= deltaPos * parallaxScale;
	}
//	private void OnCameraViewSectorChanged (Rect viewRect) {
//		UpdateBounds ();
//	}

	/*
	private void UpdateBounds() {
		Rect boundsRect = cameraControllerRef.GetViewRectOfViewSector ();
		boundsRect.center -= new Vector2 (GameProperties.ORIGINAL_WIDTH, GameProperties.ORIGINAL_HEIGHT) * 0.5f; // alignment fixxx
		boundsRect.center -= new Vector2 (diameter, diameter) * 0.5f;
		boundsRect.size += new Vector2 (diameter, diameter);
		// I'm outta bounds?! Loop me, cuz!
		Vector3 plocalPosition = this.transform.localPosition;
		while (this.transform.localPosition.x < boundsRect.xMin) {
			this.transform.localPosition += new Vector3(boundsRect.width, 0, 0);
		}
		while (this.transform.localPosition.x > boundsRect.xMax) {
			this.transform.localPosition -= new Vector3(boundsRect.width, 0, 0);
		}
		while (this.transform.localPosition.y < boundsRect.yMin) {
			this.transform.localPosition += new Vector3(0, boundsRect.height, 0);
		}
		while (this.transform.localPosition.y > boundsRect.yMax) {
			this.transform.localPosition -= new Vector3(0, boundsRect.height, 0);
		}
		// Did I move? Call a thing!
		if (this.transform.localPosition != plocalPosition) {
			OnChangedPosToStayInBounds ();
		}
	}
	*/

	virtual protected void OnChangedPosToStayInBounds() {

	}

	
	
	// ================================================================
	//  Destroy!
	// ================================================================
	private void OnDestroy() {
		// Remove event listeners!
//		GameManagers.Instance.EventManager.CameraPosChangedEvent -= OnCameraPosChanged;
//		GameManagers.Instance.EventManager.CameraViewSectorChangedEvent -= OnCameraViewSectorChanged;
//		GameManagers.Instance.EventManager.CameraZoomChangedEvent -= OnCameraZoomChanged;
	}
	

	
}



