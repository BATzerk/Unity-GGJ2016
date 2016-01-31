using UnityEngine;
using System.Collections;

/** This class is used solely for serialization purposes. BGDials redundantly has its own values. */
public class BGDialsData {
	// Constants
	public const float DEFAULT_LIFETIME_SCALE = 1f;
	public const float DEFAULT_STILL_TO_KINETIC = 0.3f;
	private const string propertyPrefix = "bgDial_"; // for writing/parsing from level file.
	// Dials!
	public float lifetimeScale; // From 0 to infinity; 1 is default.
	public float stillToKinetic; // From 0 to 1, though you can totally go past 1 for more extreme effects.
	
	
	// ================================================================
	//  Reset
	// ================================================================
	public void Reset (int worldIndex) {
		lifetimeScale = DEFAULT_LIFETIME_SCALE;
		stillToKinetic = DEFAULT_STILL_TO_KINETIC;
	}
	
	
	// ================================================================
	//  Initialize
	// ================================================================
	public void SetPropertyFromString(string fieldString, float value) {
		if (fieldString == propertyPrefix + "lifetimeScale") {
			lifetimeScale = value;
		}
		else if (fieldString == propertyPrefix + "stillToKinetic") {
			stillToKinetic = value;
		}
		else {
			Debug.LogWarning("Unknown field in levelProperties: " + fieldString);
		}
	}
	public string SerializeForLevelFile() {
		string returnString = "";
		if (lifetimeScale != DEFAULT_LIFETIME_SCALE) {
			returnString += ";" + propertyPrefix + "lifetimeScale:" + lifetimeScale;
		}
		if (stillToKinetic != DEFAULT_STILL_TO_KINETIC) {
			returnString += ";" + propertyPrefix + "stillToKinetic:" + stillToKinetic;
		}
		return returnString;
	}
	
}
