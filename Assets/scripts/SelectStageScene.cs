﻿using UnityEngine;
using System.Collections;

public class SelectStageScene : MonoBehaviour {
	
	// You'll want to set this;
	public Texture2D        screenTexture;
	
	private float           minSwipeDistance        = 14;
	//private float           maxSwipeTime            = 10;
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

	private float fingerStartTime  = 0.0f;
	private Vector2 fingerStartPos = Vector2.zero;
	
	private bool isSwipe = false;
	private float minSwipeDist  = 50.0f;
	private float maxSwipeTime = 0.5f;
	
	private CameraZoomInOutScript cameraScript;
	private int cameraDirection;
	
	
	void Start() {
		cameraScript = GetComponent<CameraZoomInOutScript>();
		isMobilePlatform = (Application.platform == RuntimePlatform.IPhonePlayer) || (Application.platform == RuntimePlatform.Android);
	}
	
	public void Update() {


		
		if (!isMobilePlatform) {
			
			float input = Input.GetAxis ("Horizontal");
			if(input < 0) {
				cameraScript.MoveToPreviousLevel ();
			}
			else if(input>0) {
				cameraScript.MoveToNextLevel ();
			}

		}

		// If no swipe direction is set.
		else {
		
		
			float gestureTime;
			float gestureDist;
			
			if (Input.touchCount > 0){
				
				foreach (Touch touch in Input.touches)
				{
					switch (touch.phase)
					{
					case TouchPhase.Began :
						/* this is a new touch */
						isSwipe = true;
						fingerStartTime = Time.time;
						fingerStartPos = touch.position;
						break;
						
					case TouchPhase.Canceled :
						/* The touch is being canceled */
						isSwipe = false;
						break;
						
					case TouchPhase.Moved :
						/* The touch is being moved */
						isSwipe = true;
						
						gestureTime = Time.time - fingerStartTime;
						gestureDist = (touch.position - fingerStartPos).magnitude;
						
						if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist){
							Vector2 direction = touch.position - fingerStartPos;
							Vector2 swipeType = Vector2.zero;
							
							if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)){
								// the swipe is horizontal:
								swipeType = Vector2.right * Mathf.Sign(direction.x);
							}else{
								// the swipe is vertical:
								swipeType = Vector2.up * Mathf.Sign(direction.y);
							}

							//NOTE: acually we handle the swipes in the oposite way of desktop
							//swipe left --> move right
							//swipe right --> move left
							if(swipeType.x != 0.0f){
								if(swipeType.x > 0.0f){
									// MOVE RIGHT
									Debug.Log("GO previous level");
									cameraScript.MoveToPreviousLevel();
								}else{
									// MOVE LEFT
									Debug.Log("GO next level");
									cameraScript.MoveToNextLevel ();
								}
							}
							
				
							
							
						}
						break;
					case TouchPhase.Stationary :
						/* The touch is being moved */
						isSwipe = true;
						/*if(player.IsMovingBackward()) {
							player.MoveBackward();
						}
						else if(player.IsMovingForward()) {
							player.MoveForward();
						}
						break;*/
						break;
						
					case TouchPhase.Ended :
						
						gestureTime = Time.time - fingerStartTime;
						gestureDist = (touch.position - fingerStartPos).magnitude;
						
						if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist){
							Vector2 direction = touch.position - fingerStartPos;
							Vector2 swipeType = Vector2.zero;
							
							if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)){
								// the swipe is horizontal:
								swipeType = Vector2.right * Mathf.Sign(direction.x);
							}else{
								// the swipe is vertical:
								swipeType = Vector2.up * Mathf.Sign(direction.y);
							}
							
							if(swipeType.x != 0.0f){
								if(swipeType.x > 0.0f){
									// MOVE RIGHT
									Debug.Log("GO RIGHT 2");
									cameraScript.MoveToNextLevel ();
								}else{
									// MOVE LEFT
									cameraScript.MoveToPreviousLevel ();
									Debug.Log("GO LEFT 2");
								}
							}
					
							
						}
						
						break;
					}
				}
			}
			else {
				//player.PlayerStationary();
			}

		}

	}
	
	public void OnGUI() {

	}
	
}