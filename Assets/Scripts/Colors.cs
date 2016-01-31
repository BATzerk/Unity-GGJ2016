using UnityEngine;
using System.Collections;

public class Colors {
	
	public static Color GetBGParticleColorA (int worldIndex) {
		switch (worldIndex) {
		case 1: return new Color(128/255f, 0/255f, 255/255f);
		case 2: return new Color(0/255f, 246/255f, 255/255f);
		case 3: return new Color(255/255f, 220/255f, 0/255f);
		case 4: return new Color(255/255f, 0/255f, 64/255f);
		case 5: return new Color(255/255f, 0/255f, 208/255f);
		case 6: return new Color(0/255f, 120/255f, 245/255f);
		case 7: return new Color(255/255f, 0/255f, 208/255f);
		default: return new Color(128/255f, 0/255f, 255/255f);
		}
	}
	public static Color GetBGParticleColorB (int worldIndex) {
		switch (worldIndex) {
		case 1: return new Color(0/255f, 255/255f, 240/255f);
		case 2: return new Color(255/255f, 0/255f, 246/255f);
		case 3: return new Color(255/255f, 55/255f, 0/255f);
		case 4: return new Color(0/255f, 80/255f, 255/255f);
		case 5: return new Color(0/255f, 208/255f, 255/255f);
		case 6: return new Color(210/255f, 106/255f, 245/255f);
		case 7: return new Color(0/255f, 208/255f, 255/255f);
		default: return new Color(0/255f, 255/255f, 240/255f);
		}
	}
	
	public static Color GetBGParticleBigColor () {
		int worldIndex = 1;
		Color colorA = GetBGParticleColorA (worldIndex);
		Color colorB = GetBGParticleColorB (worldIndex);
		return Color.Lerp (colorA,colorB, Random.Range (0f, 1f));
	}
	public static Color GetBGParticleSwirlColor (Vector3 pos) {
		int worldIndex = 1;
		Color colorA = GetBGParticleColorA (worldIndex);
		Color colorB = GetBGParticleColorB (worldIndex);
		
		//		float lerpAmount = Mathf.PerlinNoise (pos.x/4000f, pos.y/4000f) - 0.5f;
		//		lerpAmount = Mathf.Sqrt (Mathf.Abs (lerpAmount)) * GameMathUtils.Sign (lerpAmount) + 0.5f; // Push it closer to the 0 or 1 extremes.
		float lerpAmount = Mathf.PerlinNoise (pos.x/500f, pos.y/500f);
		return Color.Lerp (colorA, colorB, lerpAmount);
	}
	public static Color GetBGParticleEddyGlitterColor (Vector3 pos) {
		int worldIndex = 1;
		Color colorA = GetBGParticleColorA (worldIndex);
		Color colorB = GetBGParticleColorB (worldIndex);
		float lerpAmount = Mathf.PerlinNoise (pos.x/240f, pos.y/240f);
		return Color.Lerp (colorA, colorB, lerpAmount);
	}

}
