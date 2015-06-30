using UnityEngine;
using System.Collections;

public class SelectStageScene : MonoBehaviour {
	
	// You'll want to set this;
	public Texture2D        screenTexture;
	
	private float           minSwipeDistance        = 14;
	private float           maxSwipeTime            = 10;
	private float           startTime;
	
	private Vector2         startPos;
	private Vector2         currentPos;
	
	const int               SWIPE_NONE              = 0;
	const int               SWIPE_UP                = 1;
	const int               SWIPE_RIGHT             = 2;
	const int               SWIPE_DOWN              = 3;
	const int               SWIPE_LEFT              = 4;
	private int             swipeDirection          = SelectStageScene.SWIPE_NONE;
	
	private Vector2         screenTextureOffset     = Vector2.zero;
	private float           fadeAlpha               = 0f;
	private float           fadeSpeed               = 1f; // How fast the texture fades after swipe.

	public bool isMobilePlatform = true;
	
	public void Update() {


		if (!isMobilePlatform) {
			
			float input = Input.GetAxis ("Horizontal");
			if(input < 0) {
				GetComponent<CameraZoomInOutScript> ().MoveToPreviousLevel ();
			}
			else if(input>0) {
				GetComponent<CameraZoomInOutScript> ().MoveToNextLevel ();
			}

		}

		// If no swipe direction is set.
		else {
		
		
			if (swipeDirection == SelectStageScene.SWIPE_NONE) {
				// To fade back in after swipe (just to complete the loop)
				if (fadeAlpha > 0) {
					fadeAlpha -= Time.deltaTime * fadeSpeed;
				}
				// Getting input
				if (Input.touchCount > 0) {
					Touch touch = Input.touches [0];
					switch (touch.phase) {
					case TouchPhase.Began:
						startPos = touch.position;
						startTime = Time.time;
						break;
					case TouchPhase.Moved:
						currentPos = touch.position;
						break;
					case TouchPhase.Ended:
						screenTextureOffset = currentPos - startPos;
					
					// By using swipe distance as a magnitude here, regardless of x or y axis, we'll be choosing a swipe direction.
					// If we were only interested in X axis we would use screenTextureOffset.x instead of swipeDistance
						if (Time.time - startTime < maxSwipeTime && (currentPos - startPos).magnitude > minSwipeDistance) {
							// Find if we've moved more on the x-axis or y-axis.
							if (Mathf.Abs (screenTextureOffset.x) > Mathf.Abs (screenTextureOffset.y)) {
								// x-axis
								if (screenTextureOffset.x > 0) {
									swipeDirection = SelectStageScene.SWIPE_RIGHT;
									GetComponent<CameraZoomInOutScript> ().MoveToNextLevel ();
								} else {
									swipeDirection = SelectStageScene.SWIPE_LEFT;
									GetComponent<CameraZoomInOutScript> ().MoveToPreviousLevel ();
								}
							} /*else {
							// y-axis
							if ( screenTextureOffset.y > 0 ) {
								swipeDirection = SelectStageScene.SWIPE_UP;
							} else {
								swipeDirection = SelectStageScene.SWIPE_DOWN;
							}
						}*/
						} else {
							swipeDirection = SelectStageScene.SWIPE_NONE;
						}
						break;
					}
				} else {
					screenTextureOffset *= 1 - Time.deltaTime * fadeSpeed;
				}
			} else {
				// This fades the texture and moves it further in direction of swipe.
				screenTextureOffset *= 1 + Time.deltaTime * fadeSpeed;
				fadeAlpha += Time.deltaTime * fadeSpeed;
				if (fadeAlpha > 1) {
					swipeDirection = SelectStageScene.SWIPE_NONE;
					Debug.Log ("Finished swipe movement : " + swipeDirection);
				}
			}

		}

	}
	
	public void OnGUI() {
		//GUI.color = Color.white - Color.black * fadeAlpha;

		//GUI.DrawTexture( new Rect( screenTextureOffset.x, screenTextureOffset.y, Screen.width, Screen.height ), screenTexture );
		/*// Specific axis. Constraining visually leaves easy potential access later on.
		if ( Mathf.Abs( screenTextureOffset.x ) > Mathf.Abs( screenTextureOffset.y ) ) {
			// x-axis
			GUI.DrawTexture( new Rect( screenTextureOffset.x, 0, Screen.width, Screen.height ), screenTexture );
		} else {
			// y-axis
			GUI.DrawTexture( new Rect( 0, screenTextureOffset.y, Screen.width, Screen.height ), screenTexture );
		}
		*/
	}
	
}