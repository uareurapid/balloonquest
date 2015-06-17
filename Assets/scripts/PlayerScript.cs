﻿using UnityEngine;
using System.Collections;



/// <summary>
/// Player controller and behavior
/// </summary>
public class PlayerScript : MonoBehaviour
{
	
	//ScoreScript scoreScript;

	//rect with the texture/direction icons
	bool isDead = false;
	public Texture2D lifeIcon;
	private HealthScript playerHealth;
	private GameControllerScript controller;
	private GUIResolutionHelper resolutionHelper;

	public Texture2D failSafeIcon;
	private Rect failSafeRect;
	public bool failSafeUsed = false;

	//this is needed for the pickup speed
	//the jelly must inherit player speed
	//at every new level the value is reset to deafut speed (1)
	public float speedX = 1;

	Transform cachedTransform;

	public float maxSpeed = 0.5f;
	public float moveForce = 200f;
	public float moveSpeed = 0.5f;

	public bool isMobilePlatform = false;
	
	private bool isVisible=false;
	
	private Vector3 startingPos;
	
	private GUISkin skin;
	
	private TextLocalizationManager translationManager;

	private bool facingRight = true;

	private bool buyedInfiniteLifes = false;
	private bool moveBackward = true;
	private bool moveForward = false;
	private bool moving = false;

	void Start() {

		skin = Resources.Load("GUISkin") as GUISkin;
		isDead = false;		
		playerHealth = gameObject.GetComponent<HealthScript>();

		cachedTransform = transform;
		//Save starting position
		startingPos = cachedTransform.position;


		
	}
	
	void Awake() {

		/*GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
		if(scripts!=null) {
		  controller = scripts.GetComponent<GameControllerScript>();
		  resolutionHelper = scripts.GetComponent<GUIResolutionHelper>();
		  translationManager = scripts.GetComponent<TextLocalizationManager>();
		}
		else {
		  controller = GameControllerScript.Instance;
		  resolutionHelper = GUIResolutionHelper.Instance;
		  translationManager = TextLocalizationManager.Instance;
		}

		translationManager.LoadSystemLanguage(Application.systemLanguage);*/
		//changeSpriteCounter = Time.deltaTime;
		//make sure we have this updated
		resolutionHelper = GUIResolutionHelper.Instance;
		resolutionHelper.CheckScreenResolution();

		//CheckInAppPurchases();
	}

	//todo, code me
	/*public void LaunchFailsafe() {
		GameObject jelly = GameObject.FindGameObjectWithTag("Jelly");
		if(jelly!=null) {
		  JellyScript script = jelly.GetComponent<JellyScript>();
		  if(script!=null){
		    script.LaunchFailsafe();
		  }
		}
	}*/

	void CheckInAppPurchases() {
	   //infinite lifes
	   //buyedInfiniteLifes = PlayerPrefs.HasKey(Soomla.MyStore.JellyTrooperAssets.JELLY_TROOPERS_INFINITE_LIFES_PRODUCT_ID);
	 }
	
	void OnGUI() {
	
		GUI.skin = skin;
		
		if(Event.current.type==EventType.Repaint ) {//&& !controller.IsGameOver()

			
			/*Matrix4x4 svMat = GUI.matrix;//save current matrix
			GUI.matrix = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,resolutionHelper.scaleVector);
			
			int num = buyedInfiniteLifes ? 1 : playerHealth.hitPoints;
			
			DrawText(GetTranslationKey(GameConstants.MSG_LIFES) + " ",20,20,45,120,40);

		    int x = 70; int y=50;
			//just draw 1 x N
			if(buyedInfiniteLifes) {
				  Rect life = new Rect(x,y,48,48);
				  GUI.DrawTexture(life, lifeIcon);
				  DrawText(" X " + GetTranslationKey(GameConstants.MSG_INFINITE_LIFES),20,120,50,140,40);
			}
			else {
				for(int i=0; i < num; i++) {
				  Rect life = new Rect(x,y,48,48);
				  GUI.DrawTexture(life, lifeIcon);
				  x+=48;
			    }
			}



			if(failSafeIcon!=null && !failSafeUsed) {
			  failSafeRect = new Rect(40, resolutionHelper.screenHeight-100,48,48);
			  GUI.DrawTexture(failSafeRect,failSafeIcon);
			  DrawText(GetTranslationKey(GameConstants.MSG_FAILSAFE),20,35,resolutionHelper.screenHeight-130,120,40);
			}


			GUI.matrix = svMat;
			*/
		}
	}

	//TODO this should be an interface, somewhere
	string GetTranslationKey(string key) {
		return	translationManager.GetText(key);
	}

	public void KillPlayer() {
	  HandleLooseAllLifes();
	}
	
	GUIStyle BuildSmallerLabelStyle() {
		
		GUIStyle centeredStyleSmaller = GUI.skin.GetStyle("Label");
		centeredStyleSmaller.alignment = TextAnchor.MiddleLeft;
		//centeredStyleSmaller.font = scrollFont;
		centeredStyleSmaller.fontSize = 20 * (int)GUIResolutionHelper.Instance.scaleVector.x;
		return centeredStyleSmaller;
	}
	
	public void DrawText(string text, int fontSize, int x, int y,int width,int height) {
		
		
		GUIStyle centeredStyleSmaller = GUI.skin.GetStyle("Label");
		centeredStyleSmaller.alignment = TextAnchor.MiddleLeft;
		centeredStyleSmaller.font = controller.messagesFont;
		centeredStyleSmaller.fontSize = fontSize;
		
		GUI.Label (new Rect(x, y, width, height), text);
	}

	/*
	* Increase swing speed to double value
	*/
	public void IncreaseMovementSpeedBy(int speedFactor) {
		speedX = speedX * speedFactor;
	}
	
	void Update()
	{

	 if(IsPlayerAlive()) {

		/*JellyScript jelly = GameObject.FindGameObjectWithTag("Jelly").GetComponent<JellyScript>();
		if(jelly!=null) {
	
		  failSafeUsed = !jelly.CanLaunchFailSafe();
		}
		else {
		  failSafeUsed = true;
		}


		if (!failSafeUsed && Input.touches.Length ==1) {
			    
			Touch touch = Input.touches[0];
			    
			if(touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)  {

				Vector2 fingerPos = new Vector2(0,0);
				fingerPos.y =  resolutionHelper.screenHeight - (touch.position.y / Screen.height) * resolutionHelper.screenHeight;
				fingerPos.x = (touch.position.x / Screen.width) * resolutionHelper.screenWidth;

				if(failSafeRect!=null && failSafeRect.Contains(fingerPos) && playerHealth.hitPoints!=null) {

					
					//need to check if can be used, first
					//if(!failSafeUsed) { //jelly.CanLaunchFailSafe()
						//failSafeUsed = true;
				  		LaunchFailsafe();
					//}
				  
				}

			}

		}*/
	 }
		

		if(playerHealth.hitPoints==0) {
		  HandleLooseAllLifes();
		}

		var dist = (transform.position - Camera.main.transform.position).z;
			
			var leftBorder = Camera.main.ViewportToWorldPoint(
				new Vector3(0, 0, dist)
				).x;
			
			var rightBorder = Camera.main.ViewportToWorldPoint(
				new Vector3(1, 0, dist)
				).x;
			
			var topBorder = Camera.main.ViewportToWorldPoint(
				new Vector3(0, 0, dist)
				).y;
			
			var bottomBorder = Camera.main.ViewportToWorldPoint(
				new Vector3(0, 1, dist)
				).y;

		transform.position = new Vector3(
				Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
				Mathf.Clamp(transform.position.y, topBorder, bottomBorder),
				transform.position.z
				);
		
	}
	
		

	
	void FixedUpdate()
	{
		if(!isMobilePlatform) {

				float input =  Input.GetAxis("Horizontal");
				
				if(input!=0) {
					moving = true;
				}
				else {
					moving = false;
				}
				
				if(moving) {
					// The Speed animator parameter is set to the absolute value of the horizontal input.
					//anim.SetFloat("Speed", Mathf.Abs(h));
					
					// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
					if(input * GetComponent<Rigidbody2D>().velocity.x < maxSpeed)
						// ... add a force to the player.
						GetComponent<Rigidbody2D>().AddForce(Vector2.right * input * moveForce);
					
					// If the player's horizontal velocity is greater than the maxSpeed...
					if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
						// ... set the player's velocity to the maxSpeed in the x axis.
						GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
					
					// If the input is moving the player right and the player is facing left...
					if(input > 0 && !facingRight)
						// ... flip the player.
						Flip();
					// Otherwise if the input is moving the player left and the player is facing right...
					else if(input < 0 && facingRight)
						// ... flip the player.
						Flip();
				}
				
				
		  }
		  else {

			// The Speed animator parameter is set to the absolute value of the horizontal input.
				//anim.SetFloat("Speed", Mathf.Abs(h));
				
				// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
				if(moving) {

				    float speed = moveForward ? moveSpeed : -moveSpeed;
				    
					if(moveSpeed * GetComponent<Rigidbody2D>().velocity.x < maxSpeed) {
						// ... add a force to the player.
						GetComponent<Rigidbody2D>().AddForce(Vector2.right * speed * moveForce);
						}
			
					
					// If the player's horizontal velocity is greater than the maxSpeed...
					if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > maxSpeed)
						// ... set the player's velocity to the maxSpeed in the x axis.
						GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Sign(GetComponent<Rigidbody2D>().velocity.x) * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
					
					// If the input is moving the player right and the player is facing left...
					if(speed > 0 && !facingRight)
						// ... flip the player.
						Flip();
					// Otherwise if the input is moving the player left and the player is facing right...
					else if(speed < 0 && facingRight)
						// ... flip the player.
						Flip();
				
				} 

		  }
				
		
	}

	
	
	public static Vector3 ClampVector3(Vector3 vec, Vector3 min, Vector3 max)
	{
		return Vector3.Min(max,Vector3.Max(min,vec));
	}

	void Flip() {
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}


	
	void OnBecameVisible() {
		isVisible = true;
	}
	
	void OnBecameInvisible() {
		isVisible = false;
	}
	
	//reset for a new game/level
	public void ResetPlayer() {
	
		//HealthScript playerHealth = gameObject.GetComponent<HealthScript>();
		if(playerHealth!=null) {
			playerHealth.hitPoints = 3;	
		}
		isDead = false;
	}
	

	
	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		
	}

	//handle the collision with another sprite (not other trigger)
	void OnCollisionEnter2D(Collision2D collision)
	{

	}
	

	//takes a life

	public void TakeLife() {

		if(playerHealth!=null) {
			playerHealth.Damage(1);

			if(playerHealth.hitPoints>0) {
				//controller.isJellyFalling = false;

			}
		
		}
	}
	
	public void TakeAllLifes() {
		
		if(playerHealth!=null) {
			playerHealth.Damage(20);//20 is enough
			
		}
	}

	public int GetRemainingLifes() {
	  return playerHealth.hitPoints;
	}
	
	
	//wait for 3 seconds before show next level
	IEnumerator PrepareForNextLevel()
	{
		yield return new WaitForSeconds(3);
		ShowGameOver(true);
	}



	//called by the engine when player dies
	void OnDestroy()
	{
	  
		/*if(controller.IsGameStarted()) {
			ShowGameOver(false);
		}*/
		//else just ignore this stuff
		
	}
	
	void ShowGameOver(bool showNextlevel) {
	
	    controller.EndGame(showNextlevel);
		// Game Over.
		// Add the script to the parent because the current game
		// object is likely going to be destroyed immediately.
		transform.parent.gameObject.AddComponent<GameOverScript>();
		
		
	}
	
	
	//when player dies, we save the score and end the game
	public void HandleLooseAllLifes() {
		
		isDead = true;
		playerHealth.hitPoints=0;
		//set the score key pref
		//PlayerPrefs.SetInt("HighScore",scoreScript.score);
		Destroy(gameObject);
	}
	
	public bool IsPlayerAlive() {
	  return !isDead && playerHealth.hitPoints > 0;
	}
	
	//increase the player health by x
	public void IncreaseHealthBy(int amount) {

		if(playerHealth!=null) {
		  playerHealth.hitPoints += amount;
		}
	}

	//self destroy method
	IEnumerator SelfDestroyPlayer() {
	 
		yield return new WaitForSeconds(3);
		ShowGameOver(false);
	}

	public void MoveBackward() {
	  moveBackward = true;
	  moveForward = false;
	  moving = true;
	}
	public void MoveForward() {
	  moveForward = true;
	  moveBackward =  false;
	  moving = true;
	}

	public void Jump(){

	}

	public void PlayerStationary() {

	}

	public void SetOnLadder(bool onLadder) {

	}

	public void IncreaseCoins(int value) {

	}

	public bool IsMovingBackward() {
	 return true;
	}

	public bool IsMovingForward() {
	 return true;
	}
}


// * - Awake() is called once when the object is created. See it as replacement of a classic constructor method.
// * - Start() is executed after Awake(). The difference is that the Start() method is not called if the script is not enabled (remember the checkbox on a component in the "Inspector").
// * - Update() is executed for each frame in the main game loop.
// * - FixedUpdate() is called at every fixed framerate frame. You should use this method over Update() when dealing with physics ("RigidBody" and forces).
// * - Destroy() is invoked when the object is destroyed. It's your last chance to clean or execute some code.
  
// * OnCollisionEnter2D(CollisionInfo2D info) is invoked when another collider is touching this object collider.
// * OnCollisionExit2D(CollisionInfo2D info) is invoked when another collider is not touching this object collider anymore.
// * OnTriggerEnter2D(Collider2D otherCollider) is invoked when another collider marked as a "Trigger" is touching this object collider.
// * OnTriggerExit2D(Collider2D otherCollider)

