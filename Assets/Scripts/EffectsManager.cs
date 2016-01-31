using UnityEngine;
using System.Collections;

public class EffectsManager : MonoBehaviour {
	// References
//	[SerializeField] private GameController gameControllerRef;
	// Components
	[SerializeField] ParticleSystem targetPathBurstSystem;


	// Events
	public void OnTravelerReachTargetPathEnd (Vector3 pos) {
		targetPathBurstSystem.transform.localPosition = pos;
		targetPathBurstSystem.Emit (1);
	}


}
