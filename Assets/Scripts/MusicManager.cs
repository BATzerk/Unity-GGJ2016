using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour {
	// Components
//	[SerializeField] private AudioMixerSnapshot musicSnapshot;
//	private AudioSource musicTrackAudioSource;
	// Properties
//	private bool isMusicMuted;
	// References
//	private AudioMixerSnapshot currentMusicSnapshot;
	// Getters
//	public bool IsMusicMuted { get { return isMusicMuted; } }
	
	
	// ================================================================
	//  Start
	// ================================================================
	private void Start() {
//		PlayMusicTrack ();
		AudioSource mySource = GetComponent<AudioSource> ();
		mySource.loop = true;
		mySource.Play ();
	}
	
	
	// ================================================================
	//  Basic Events
	// ================================================================
	/** This calls .Play() for all my audio source music tracks. Start, resume, play-- all the same thing here. */
//	public void PlayMusicTrack() {
//		musicTrackAudioSource.Play ();
//	}
	
//	private void SetCurrentMusicSnapshot(AudioMixerSnapshot _currentMusicSnapshot, float transitionDuration) {
//		currentMusicSnapshot = _currentMusicSnapshot;
//		currentMusicSnapshot.TransitionTo (transitionDuration);
//	}
	

	
	
}



