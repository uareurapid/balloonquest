using UnityEngine;
using System.Collections;
using BalloonQuest;


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
	private bool failSafeUsed = false;

	public Sprite defaultBalloon;
	public Sprite balloonOne;
	public Sprite balloonTwo;
	public Sprite balloonThree;
	public Sprite balloonFour;

	private bool isVisible = true;

	//this is needed for the pickup speed
	//the jelly must inherit player speed
	//at every new level the value is reset to deafut speed (1)


	public float speedX = 1;

	Transform cachedTransform;

	public float maxSpeed = 0.5f;
	public float moveForce = 200f;
	public float moveSpeed = 0.5f;

	private bool isMobilePlatform = false;
	
	private Vector3 startingPos;
	
	private GUISkin skin;
	
	private TextLocalizationManager translationManager;

	private bool facingRight = true;
	private bool hasLanded = false;
	private bool buyedInfiniteLifes = false;
	private bool moveBackward = true;
	private bool moveForward = false;
	private bool moving = false;
	private bool canMove = true;
	
	private PickupCounterScript coinsCounter;

	private bool hasBalloon = true;
	private bool hasParachute = false;

	private GameObject scripts;
	private static RuntimePlatform platform;
	SoundEffectsHelper soundEffects;
	private bool isStandingOnPlatform = false;

	private HeroScript hero;

	void Start() {

		skin = Resources.Load("GUISkin") as GUISkin;
		isDead = false;		
		playerHealth = gameObject.GetComponent<HealthScript>();

		cachedTransform = transform;
		//Save starting position
		startingPos = cachedTransform.position;
		
		scripts = GameObject.FindGameObjectWithTag("Scripts");
		coinsCounter = scripts.GetComponent<PickupCounterScript>();
		controller = scripts.GetComponent<GameControllerScript> ();
		hasParachute = false;
		failSafeUsed = false;
		hasBalloon = true;

		platform = Application.platform;
		isMobilePlatform = (platform == RuntimePlatform.IPhonePlayer) || (platform == RuntimePlatform.Android);
		hasLanded = false;

		soundEffects = GetSoundEffects ();

		hero = GetHero();
		
	}

	//check if we are on a platform
	public bool IsPlayerStandingOnPlatform() {

		return isStandingOnPlatform;
	}

	public void PlayerLandedOnPlatform(bool landed) {
		isStandingOnPlatform = landed;
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

	public void EnableGravityScale() {
		Rigidbody2D rig = GetComponent<Rigidbody2D>();
		if(rig!=null) {
			rig.gravityScale =1.0f;
			canMove = false;
		}
	}

	public void DisableGravityScale() {
		Rigidbody2D rig = GetComponent<Rigidbody2D>();
		if(rig!=null) {
			rig.gravityScale =0.0f;//don´t let him continue to fall
		}
	}

	public bool IsPlayerFalling() {
	   //if i don´t have balloon, for sure is falling
	   if(!hasBalloon) {
	     return true;
	   }

	   Rigidbody2D rig = GetComponent<Rigidbody2D>();
	   if(rig!=null) {
		 return rig.gravityScale == 1.0f && !hasBalloon;
	   }

	   return false;
	}


	void CheckInAppPurchases() {
	   //infinite lifes
	   //buyedInfiniteLifes = PlayerPrefs.HasKey(Soomla.MyStore.JellyTrooperAssets.JELLY_TROOPERS_INFINITE_LIFES_PRODUCT_ID);
	 }
	
	void OnGUI() {
	
		GUI.skin = skin;
		
		if(Event.current.type==EventType.Repaint ) {//&& !controller.IsGameOver()

			
			Matrix4x4 svMat = GUI.matrix;//save current matrix
			GUI.matrix = Matrix4x4.TRS(Vector3.zero,Quaternion.identity,resolutionHelper.scaleVector);

			if(failSafeIcon!=null && hasParachute && !failSafeUsed) {
			  failSafeRect = new Rect(10,50,56,48);
			  GUI.DrawTexture(failSafeRect,failSafeIcon);
			  DrawText("failsafe",10,85,53,120,40);
			}


			GUI.matrix = svMat;

		}
	}

	//TODO this should be an interface, somewhere
	string GetTranslationKey(string key) {
		return	translationManager.GetText(key);
	}

	public void KillPlayer() {
	  HandleLooseAllLifes();
	}

	public bool PlayerTouchedGround(){
	  return hasLanded;
	}

	//either landed or the ground already moved
	public bool PlayerReleasedBalloon(){
	  return hasLanded || !canMove;
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

		//if is standing on a landing platform we do not clamp, and let him die
		if (canMove && !isStandingOnPlatform) {

			//only clamp if can move otherwise let him go outside screen and kill it
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

		if(hasLanded && hero.HasHeroReachedTarget()) {

			Invoke("LoadNextLevel",3.0f);
		}


	}
	
		

	
	void FixedUpdate()
	{
		if(!isMobilePlatform) {

				float input =  Input.GetAxis("Horizontal");
				
				if(input!=0) {

					if(!moving) {
						//if i was already moving before, i do not play it again
						PlayMoveEffect();
					}

					moving = true;
				}
				else {
					moving = false;
				}
				
				if(moving && canMove) {

			
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
				if(moving && canMove) {

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

	public void Flip() {
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public bool IsPlayerFacingRight() {
	  return facingRight;
	}


	//reset for a new game/level
	public void ResetPlayer() {
	
		//HealthScript playerHealth = gameObject.GetComponent<HealthScript>();
		if(playerHealth!=null) {
			playerHealth.hitPoints = 3;	
		}
		isDead = false;
	}

	public void AddFailsafeParachute() {
		Debug.Log ("ADDING PARACHUTE");
		hasParachute = true;
		SpecialEffectsHelper effects = scripts.GetComponentInChildren<SpecialEffectsHelper> ();
		if (effects != null) {
			effects.PlayJElectricityEffect(transform.position);
		}
	}

	
	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		Debug.Log ("collided with: " + otherCollider.gameObject.tag);
		PerformUpdate(otherCollider.gameObject);
	}

	//handle the collision with another sprite (not other trigger)
	void OnCollisionEnter2D(Collision2D collision)
	{
		GameObject collisionObject = collision.gameObject;
		GroundScript ground = collisionObject.GetComponentInChildren<GroundScript> ();
		if (ground != null && !hasLanded) {
			HandleGroundCollision ();
		} 
		else {
			PerformUpdate(collision.gameObject);
		}



	}

	void PerformUpdate(GameObject collisionObject) {
		
		EnemyScript enemy = collisionObject.GetComponent<EnemyScript> ();
		//collided with enemy
		if (enemy != null) {
				
			if (PlayerHasBalloon ()) {
				BurstBallon ();
			}
			else if (PlayerHasParachute ()) {
				BurstParachute ();
			}
			else if(IsPlayerAlive()) {
				KillPlayer();
			}
	
				
		}
		else {
			GemScript gem = collisionObject.GetComponent<GemScript>();
			if(gem!=null) {
			  HandleGemCollision(gem);
			}
		}
		
		
	}

	public void HandleGemCollision(GemScript gem) {
	 if(hasBalloon) {
	   Sprite newSprite = gem.GetBalloonGift();
	   SpriteRenderer renderer = GetComponent<SpriteRenderer> ();
	   renderer.sprite = newSprite;
	   renderer.enabled = true;
	   GetComponent<PolygonCollider2D> ().enabled = true;
	   soundEffects.PlayPowerupSound();
	   
	 }
	}



	//handle collision with ground
	void HandleGroundCollision() {

		DisableGravityScale();

	    hasLanded = true;
		//play effects
		SpecialEffectsHelper fx = scripts.GetComponentInChildren<SpecialEffectsHelper> ();
		if (fx != null) {
			fx.PlayJellyLandedEffect(transform.position);
		}
		PlayLandingSound();

		PlayFireworks ();

		//finish level
			
		int level = controller.currentLevel;
		int max = controller.numberOfLevels;

		PlaySuccessSound ();

		if(level < max) {
			//Go to next level in 2 seconds!

			hero.StartMovingTowardsSign();
			//Play some animation
			controller.FinishLevel();

		}
			

	}


	//get the reference for the Hero obj
	private HeroScript GetHero() {

	 GameObject heroObj = GameObject.FindGameObjectWithTag("Hero");
	 return heroObj.GetComponent<HeroScript>();
	}

//todo also play some nice sounds
	void PlayFireworks() {
		Debug.Log ("PlayFireworks now!!");
		GameObject fireworks = GameObject.FindGameObjectWithTag ("Fireworks");
		if (fireworks != null) {
			Debug.Log ("ACTIVATE Fireworks now!!");
			//fireworks.SetActive(true);
			ActivateScript script = fireworks.GetComponent<ActivateScript>();
			if(script!=null) {
				Debug.Log ("CALLING ACTIVATE Fireworks now!!");
				script.Activate();
			}
		}
	}

	void PlaySuccessSound() {
		
		soundEffects.PlaySuccessSound ();

	}
	//landed
	void PlayLandingSound() {

		soundEffects.PlayLandingSound ();

	}

	private SoundEffectsHelper GetSoundEffects() {

		SoundEffectsHelper fx = scripts.GetComponentInChildren<SoundEffectsHelper> ();
		return fx;
	}

    void LoadNextLevel() {
		Application.LoadLevel("Level" + controller.GetNextLevel());
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
	
		Debug.Log("END GAME!!!!!");
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
		ShowGameOver (false);

		SoundEffectsHelper sfx = scripts.GetComponentInChildren<SoundEffectsHelper> ();
		if (sfx != null) {
			sfx.PlayHitDeadSound();
		}

		//play effects
		SpecialEffectsHelper fx = scripts.GetComponentInChildren<SpecialEffectsHelper> ();
		if (fx != null) {
			fx.PlayJellySoulEffect(transform.position);
		}
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

	public void PlayMoveEffect() {
		SoundEffectsHelper fx = scripts.GetComponentInChildren<SoundEffectsHelper>();
		if(fx!=null) {
			fx.PlayWooshSound();
		}
	}

	public void Jump(){

	}

	//disable player movement
	public void DisableMovement() {
		canMove = false;
	}
	public void PlayerStationary() {

	}

	public void SetOnLadder(bool onLadder) {

	}

	public void IncreaseCoins(int value) {

      if(coinsCounter!=null) {
        coinsCounter.AddPickup();
		if(coinsCounter.numberPickups>=GameConstants.MINIMUM_COINS_FAILSAFE_PARACHUTE && !hasParachute) {
			//should be bought with virtual currency?
			AddFailsafeParachute();
			coinsCounter.RemoveMultiplePickups(GameConstants.MINIMUM_COINS_FAILSAFE_PARACHUTE);
		}
      }
	}
	

	public bool IsMovingBackward() {
	 return true;
	}

	public bool IsMovingForward() {
	 return true;
	}

	//disables balloon
	public void BurstBallon() {
		//TODO play effects/sound
		hasBalloon = false;
		//play effects
		SpecialEffectsHelper fx = scripts.GetComponentInChildren<SpecialEffectsHelper> ();
		if (fx != null) {
			fx.PlayJellyHitDeadEffect(transform.position);
		}

		//disable colliders and renderers
		gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		gameObject.GetComponent<PolygonCollider2D> ().enabled = false;

		if (hasParachute) {
			//enable parachute sprite
			EnableParachute ();
		} 
		else {
			//make him fall to the ground
			EnableGravityScale ();
		}
	}

	public bool PlayerHasBalloon() {
		return hasBalloon;
	}

	public bool PlayerHasParachute() {
		return hasParachute;
	}

	//this is here, because the idea is:
	//if player touched ground out of camera, he would come walking to the center of the screen
	//of the camera before passing to next level
	public void setVisible(bool visible) {
	  	isVisible = visible;
	}

	//check if parachute is enabled/rendered
	private bool IsParachuteEnabled() {
		if (!hasParachute) {
			return false;
		}
		GameObject parachute = GameObject.FindGameObjectWithTag("Parachute");
		return parachute.GetComponent<SpriteRenderer> ().enabled;
	}

	public void BurstParachute() {

		//play effects
		SpecialEffectsHelper fx = scripts.GetComponentInChildren<SpecialEffectsHelper> ();
		if (fx != null) {
			fx.PlayJellyHitDeadEffect(transform.position);
		}

		GameObject parachute = GameObject.FindGameObjectWithTag("Parachute");
		parachute.GetComponent<SpriteRenderer> ().enabled = false;
		parachute.GetComponent<CircleCollider2D> ().enabled = false;
		//make him fall to the ground
		EnableGravityScale ();
		hasParachute = false;
	}

	private void EnableParachute() {
		GameObject parachute = GameObject.FindGameObjectWithTag("Parachute");
		if(parachute!=null) {
			parachute.GetComponent<SpriteRenderer>().enabled = true;
			parachute.GetComponent<CircleCollider2D> ().enabled = true;
			failSafeUsed = true;
		}
	}

	public bool CanPlayerMove() {
		return canMove;
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

