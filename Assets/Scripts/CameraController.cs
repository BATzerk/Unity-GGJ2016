using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	// Constants
	private const float POS_EASING = 120f;
	private const float ROTATION_EASING = 80f;
	private const float ZOOM_EASING = 120f;
	private const float POS_Z = -10f;
	// Components
	private Camera cameraScript;
	// References
	[SerializeField] private GameController gameControllerRef;
//	[SerializeField] private Traveler player;

	
	
	// ================================================================
	//	Initialize
	// ================================================================
	void Awake() {
		cameraScript = GetComponent<Camera> ();
	}
	
	
	// ================================================================
	//	Update
	// ================================================================
	void Update () {
		UpdatePosAndZoom ();
//		FollowPlayer ();
	}

	private void UpdatePosAndZoom () {
		Vector3 targetPos = new Vector3(gameControllerRef.PathNodeBoundsRect.center.x, gameControllerRef.PathNodeBoundsRect.center.y, POS_Z);
		float targetZoom = gameControllerRef.PathNodeBoundsRect.height * 0.7f; // Basically HACK-ed.

		transform.localPosition += (targetPos-transform.localPosition) / POS_EASING;
		cameraScript.orthographicSize += (targetZoom-cameraScript.orthographicSize) / ZOOM_EASING;
	}

	/*
	private void FollowPlayer() {
		Vector3 targetPos = new Vector3 (player.transform.localPosition.x, player.transform.localPosition.y-1f, player.transform.localPosition.z);
		Vector3 prevNodePos = player.nodePrev.transform.localPosition;
		Vector3 nextNodePos = player.nodeNext.transform.localPosition;

		this.transform.localPosition += new Vector3 (
			(targetPos.x - this.transform.localPosition.x) / POS_EASING,
			(targetPos.y - this.transform.localPosition.y) / POS_EASING,
			0);

		float currentRotation = this.transform.localEulerAngles.z;
		float targetRotation = -Mathf.Atan2 (prevNodePos.x-nextNodePos.x, prevNodePos.y-nextNodePos.y) * Mathf.Rad2Deg;
		if (currentRotation < -180)
			currentRotation += 360;
		if (currentRotation > 180)
			currentRotation -= 360;
		this.transform.localEulerAngles += new Vector3(0, 0, (targetRotation-currentRotation)/ROTATION_EASING);
	}
	*/


}



