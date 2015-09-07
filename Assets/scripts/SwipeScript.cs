using UnityEngine;
using System.Collections;
using BalloonQuest;

public class SwipeScript : MonoBehaviour {
	
	
	private float fingerStartTime  = 0.0f;
	private Vector2 fingerStartPos = Vector2.zero;
	
	private bool isSwipe = false;
	private float minSwipeDist  = 50.0f;
	private float maxSwipeTime = 0.5f;
	
	bool supportsAccelerometer= false;
	bool accelerometerActivated = false;
	private PlayerScript player;
	
	void Start()  {
	
	   GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
	   player = playerObj.GetComponent<PlayerScript>();
	   //has support?
	   bool supportsAccelerometer = SystemInfo.supportsAccelerometer;
	   //if not supported or not activated, disable the corresponding script
	   if (!supportsAccelerometer || !IsAccelerometerActivated ()) {

			player.GetComponent<AccelerometerInputScript>().enabled = false;
	   }
	}

	bool IsAccelerometerActivated() {
		int useAccelerometer = PlayerPrefs.GetInt (GameConstants.ACCELEROMETER_SETTINGS_KEY, 0);
		accelerometerActivated = (useAccelerometer == 1) ? true : false;
		return accelerometerActivated;
	}
	// Update is called once per frame
	void Update () {
		float gestureTime;
		float gestureDist;
		
		if (!accelerometerActivated && Input.touchCount > 0 ){
			
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
						
						if(swipeType.x != 0.0f){
							if(swipeType.x > 0.0f){
								// MOVE RIGHT
								Debug.Log("GO RIGHT");
								player.MoveForward();
							}else{
								// MOVE LEFT
								Debug.Log("GO LEFT");
								player.MoveBackward();
							}
						}
						
						//
						/*if(swipeType.y != 0.0f ){
							if(swipeType.y > 0.0f){
								// MOVE UP
								if(player.IsDescendingLadder() || player.IsOnLadder()) {
								  player.StartClibing();
								}
								
							}else{
								// MOVE DOWN
								if(player.IsClibingLadder()) {
								  player.StartDescending();
								}//else crouch for instance??
							}
						}*/
						
						
					}
					break;
				case TouchPhase.Stationary :
					/* The touch is being moved */
					isSwipe = true;
					if(player.IsMovingBackward()) {
						player.MoveBackward();
					}
					else if(player.IsMovingForward()) {
						player.MoveForward();
					}
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
								player.MoveForward();
							}else{
								// MOVE LEFT
								player.MoveBackward();
								Debug.Log("GO LEFT 2");
							}
							player.PlayMoveEffect();
						}
						/*else {
						  player.PlayerStationary();
						}*/
						
						/*if(swipeType.y != 0.0f ){
							if(swipeType.y > 0.0f){
								// MOVE UP
								if(!player.IsOnLadder()) {
									Debug.Log("GO JUMP");
								  player.Jump();
								}
								else {
									Debug.Log("GO CLIBING");
								  player.StartClibing();
								}
							}else{
								// MOVE DOWN
							}
						}*/
						
					}
					
					break;
				}
			}
		}
		else {
		  player.PlayerStationary();
		}
		
	}
}