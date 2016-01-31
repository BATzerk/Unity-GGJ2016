using UnityEngine;
using System.Collections;

public class GameUtils {
	
	static public void SizeSprite(SpriteRenderer sprite, float desiredWidth,float desiredHeight) {
		if (sprite == null) {
			Debug.LogWarning("Oops! We've passed in a null sprite into GameUtils.SizeSprite.");
			return;
		}
		// Reset my sprite's scale; find out its neutral size; scale the images based on the neutral size.
		sprite.transform.localScale = Vector2.one;
		float imgW = sprite.sprite.bounds.size.x;
		float imgH = sprite.sprite.bounds.size.y;
		sprite.transform.localScale = new Vector2(desiredWidth/imgW, desiredHeight/imgH);
	}

	
	public static void UpdateRectFromPoint(ref Rect rect, Vector2 point) {
		if (rect.xMin > point.x) { // LEFT
			rect.xMin = point.x;
		}
		if (rect.xMax < point.x) { // RIGHT
			rect.xMax = point.x;
		}
		if (rect.yMin > point.y) { // TOP
			rect.yMin = point.y;
		}
		if (rect.yMax < point.y) { // BOTTOM
			rect.yMax = point.y;
		}
	}



}





