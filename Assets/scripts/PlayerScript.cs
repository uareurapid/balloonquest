using UnityEngine;
using System.Collections;
using MrBalloony;


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
	private bool umbrellaUsed = false;

	public Sprite defaultBalloon;
	public Sprite balloonOne;

	//gems balloons
	public Sprite blueBalloon;//blue balloon
	public Sprite greenBalloon;//green balloon
	public Sprite redBalloon;//red balloon


	public Sprite colouredBalloon;

	//the user can move while standing on platform, along with her;
	private LandingPlatform landingPlatform;



	private bool isVisible = true;

	private bool isLanding = false;
	//it can be falling but not landing
	//the difference is if it has balloon or not and if the ground is visible
	private bool isFalling = false;

	//this is needed for the pickup speed
	//the jelly must inherit player speed
	//at every new level the value is reset to deafut speed (1)
	GroundScript ground;


	public float speedX = 1;

	Transform cachedTransform;

	public float maxSpeed = 0.5f;
	public float moveForce = 200f;
	public float jumpForce = 200f;
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
	
	private PickupCounterScript parachuteCounter;

	private bool hasBalloon = true;
	private bool hasParachute = false;
	private bool hasUmbrella = false;

	private GameObject scripts;
	private static RuntimePlatform platform;
	SoundEffectsHelper soundEffects;
	//always start on platform
	private bool isStandingOnPlatform = true;

	private bool hasRedGemGift = false;
	private bool hasGreenGemGift = false;
	private bool hasBlueGemGift = false;

	private HeroScript hero;

	private bool jump = false;


	void Start() {

		skin = Resources.Load("GUISkin") as GUISkin;
		isDead = false;		
		playerHealth = gameObject.GetComponent<HealthScript>();

		cachedTransform = transform;
		//Save starting position
		startingPos = cachedTransform.position;
		
		scripts = GameObject.FindGameObjectWithTag("Scripts");
		parachuteCounter = scripts.GetComponent<PickupCounterScript>();
		controller = scripts.GetComponent<GameControllerScript> ();
		//make sure we have this updated
		resolutionHelper = scripts.GetComponent<GUIResolutionHelper> ();
		resolutionHelper.CheckScreenResolution();

		hasParachute = false;
		failSafeUsed = false;
		umbrellaUsed = false;
		hasUmbrella = false;
		hasBalloon = true;

		platform = Application.platform;
		isMobilePlatform = (platform == RuntimePlatform.IPhonePlayer) || (platform == RuntimePlatform.Android);
		hasLanded = false;

		soundEffects = GetSoundEffects ();
		ground = GetGround ();

		hero = GetHero();

		isFalling = false;
		isLanding = false;

		//the possible gifts
		hasRedGemGift = false;
	    hasGreenGemGift = false;
	    hasBlueGemGift = false;

	    GameObject obj = GameObject.FindGameObjectWithTag("StandingPlatform");
		landingPlatform = obj.GetComponent<LandingPlatform>();
		
	}

	public void ResetDefaultSprite() {
	  GetComponent<SpriteRenderer>().sprite = defaultBalloon;
	  //reset these as well
	  hasRedGemGift = false;
	  hasGreenGemGift = false;
	  hasBlueGemGift = false;

	  ChangeAvatar(false,false,false);

	}

	//check if we are on a platform
	public bool IsPlayerStandingOnPlatform() {

		return isStandingOnPlatform;
	}

	public void PlayerLandedOnPlatform() {
		isStandingOnPlatform = true;
		DisableGravityScale();
		EnableMoveScript();
		ShowPlatform();
		landingPlatform.ResetHealthBar();
		isFalling = false;
		jump = false;

		if(isStandingOnPlatform && !hasParachute/* && !hasBalloon*/) {
		  canMove = true;
		}

	}
	
	void Awake() {

		
	}

	public LandingPlatform GetLandingPlatform() {
	  return landingPlatform;
	}


	public void EnableGravityScale() {
		Rigidbody2D rig = GetComponent<Rigidbody2D>();
		if(rig!=null) {
			rig.gravityScale =1.0f;
			//canMove = false;
		}
	}

	public void DisableGravityScale() {
		Rigidbody2D rig = GetComponent<Rigidbody2D>();
		if(rig!=null) {
			rig.gravityScale =0.0f;//don´t let him continue to fall
		}
	}

	public bool IsPlayerFallingToLand() {
	  return isLanding && ground.isVisible;
	}

	public bool IsPlayerFalling() {
	   //if i don´t have balloon, for sure is falling
	   if(!hasBalloon) {
	     return true;
	   }

	   Rigidbody2D rig = GetComponent<Rigidbody2D>();
	   if(rig!=null) {
		 return rig.gravityScale == 1.0f;/* && !hasBalloon;*/
	   }

	   return false;
	}

	GroundScript GetGround() {
		return GameObject.FindGameObjectWithTag("Ground").GetComponentInChildren<GroundScript>();
	}

	public bool IsGroundVisible() {
		return ground!=null && ground.isVisible;
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
	  if(!isDead) {
		HandleLooseAllLifes();
		Handheld.Vibrate();
	  }
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


		if(playerHealth.hitPoints==0 && !isDead) {
		  HandleLooseAllLifes();
		}

		//if is standing on a landing platform we do not clamp, and let him die
		//if ( /*(canMove && !isStandingOnPlatform) || ( IsPlayerFalling() && IsGroundVisible()) */) {

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

		//}

		if(hasLanded && hero.HasHeroReachedTarget()) {
		    canMove = false;
			Invoke("LoadNextLevel",3.0f);
		}

	
	}
	
		

	
	void FixedUpdate()
	{
		if(!isMobilePlatform) {

				float input =  Input.GetAxis("Horizontal");

			    float jumpingInput = Input.GetAxis("Vertical");
				
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


				if(jumpingInput>0 && !jump && !isFalling && isStandingOnPlatform ) {

				  //can jump
				  jump = true;

				}
			
				
				if( (jump && canMove) || (moving && canMove) ) {

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

					if(jump && !isFalling && isStandingOnPlatform) {
					   Debug.Log("JUMP");
					   GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));

					    

						//if(jumpingInput * GetComponent<Rigidbody2D>().velocity.y < maxSpeed)
						//	GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpingInput * jumpForce);

						  DisableMoveScript();
						  EnableGravityScale();
						  HidePlatform();
						  isStandingOnPlatform = false;
						  isFalling = true;
						  jump = false;

					}
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
	//TODO check also the particles
	void HidePlatform() {
	 landingPlatform.GetComponent<SpriteRenderer>().enabled = false;
	 ParticleSystem[] particles = landingPlatform.GetComponentsInChildren<ParticleSystem>();
	 foreach(ParticleSystem part in particles) {
	   part.Stop();
	 }
	 //explode it
		if(scripts!=null) {
			SpecialEffectsHelper effects = scripts.GetComponentInChildren<SpecialEffectsHelper>();
			if(effects!=null) {
				effects.PlayExplosionEffect(landingPlatform.transform.position);
			}
							
	    }

	}

	void ShowPlatform() {
	  landingPlatform.GetComponent<SpriteRenderer>().enabled = true;
	  ParticleSystem[] particles = landingPlatform.GetComponentsInChildren<ParticleSystem>();
	  foreach(ParticleSystem part in particles) {
	    part.Play();
	  }
	}

	void DisableMoveScript() {
	  MoveScript script =  GetComponent<MoveScript>();
	  script.enabled = false;
	}

	void EnableMoveScript() {
	  MoveScript script =  GetComponent<MoveScript>();
	  script.enabled = true;
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
		
		hasParachute = true;
		SpecialEffectsHelper effects = scripts.GetComponentInChildren<SpecialEffectsHelper> ();
		if (effects != null) {
			effects.PlayJElectricityEffect(transform.position);
		}
	}

	public void AddUmbrella() {
		
		hasUmbrella = true;
		SpecialEffectsHelper effects = scripts.GetComponentInChildren<SpecialEffectsHelper> ();
		if (effects != null) {
			effects.PlayJElectricityEffect(transform.position);
		}
	}

	
	void OnTriggerEnter2D(Collider2D otherCollider)
	{

		GameObject collisionObject = otherCollider.gameObject;
		GroundScript ground = collisionObject.GetComponentInChildren<GroundScript> ();
		if (ground != null) {
		    Debug.Log("Handle ground collision!");
			HandleGroundCollision (ground.isVisible);
		} 
		else if(!hasLanded) {
		   //ignore collisions when already landed
			PerformUpdate(otherCollider.gameObject);
		}
	}

	//handle the collision with another sprite (not other trigger)
	void OnCollisionEnter2D(Collision2D collision)
	{
		GameObject collisionObject = collision.gameObject;
		GroundScript ground = collisionObject.GetComponentInChildren<GroundScript> ();
		if (ground != null) {
		    Debug.Log("Handle ground collision!");
			HandleGroundCollision (ground.isVisible);
		} 
		else if(!hasLanded) {
		   //ignore collisions when already landed
			PerformUpdate(collision.gameObject);
		}



	}

	void PerformUpdate(GameObject collisionObject) {

	Debug.Log("PERFORM UPDATE");
		
		EnemyScript enemy = collisionObject.GetComponent<EnemyScript> ();
		//collided with enemy
		if ( (enemy != null && IsPlayerAlive()) && !IsPlayerFallingToLand() ) {
			Debug.Log("PERFORM UPDATE!!!!!");
		    //do not handle grounded enemy collisions
			if(PlayerTouchedGround()){
			  return;
			}

		   //check if i have a gift ballooon undestructible
		   BalloonScript ball = GetComponentInChildren<BalloonScript>();
		   if(ball!=null) {
		     //just handle the gift balloon stuff
		     HandleCollisionWhileUsingGiftBalloon(ball,enemy);
		     return;
		   }

				
			if (PlayerHasBalloon ()) {
				BurstBallon ();
				if(enemy.isBurner) {
					GetHero().BurnHero(false);
				}
				else {
					GetHero().BlinkWhenHit();
				}

			}
			else if (PlayerHasParachute ()) {
				BurstParachute ();
				if(enemy.isBurner) {
					GetHero().BurnHero(false);
				}
				else {
					GetHero().BlinkWhenHit();
				}
			}
			else if (PlayerHasUmbrella ()) {
				BurstUmbrella ();
				if(enemy.isBurner) {
					GetHero().BurnHero(false);
				}
				else {
					GetHero().BlinkWhenHit();
				}
			}
			else if(IsPlayerAlive()) {
				if(enemy.isBurner) {
					GetHero().BurnHero(true);
					//will die after the animation
				}
				else {
					GetHero().BlinkWhenHit();
					if(!IsGroundVisible() && !IsPlayerFalling()) {
						KillPlayer();
					}
					  
					

				}

			}
	
				
		}


		

	}

	//the name says it all no?? :-)
	public void HandleCollisionWhileUsingGiftBalloon(BalloonScript ball, EnemyScript enemy) {

		if(ball.IsUndestructibleThroughHits()) {
		       //blue
		      ball.DecreaseHitsCounter();
		      if(enemy.isExplosive) {
		        ParticleSystem part = enemy.gameObject.GetComponentInChildren<ParticleSystem>();
		        if(part!=null) {
		          part.Play();
		        }
		      }

		}
		/*else if(ball.IsUndestructibleThroughTime()) {
		       //green

		    if(!ball.startDestroying) {
		         //will start the countdown
				ball.StartCountdownDestruction();
		     }

		}*/
	}

	void UpdateHealthBar(float currentHealth) {
		GameObject healthbar = GameObject.FindGameObjectWithTag("HealthBar");
		if(healthbar!=null) {
			HealthBar bar = healthbar.GetComponent<HealthBar>();
			bar.SetCurrentHealth(currentHealth);
		}
	}

	public void HandleGemCollision(GemScript gem) {

	   PickupCounterScript counter;
	   bool isRed = false;
	   bool isBlue =  false;
	   bool isGreen = false;

	  if(gem.isRed) {
			counter = GameObject.FindGameObjectWithTag("RedGemCounter").GetComponent<PickupCounterScript>();
			isRed = true;
	  }
	  else if(gem.isGreen) {
			counter = GameObject.FindGameObjectWithTag("GreenGemCounter").GetComponent<PickupCounterScript>();
			isGreen =  true;
	  }
	  else {
	  		//blue
			counter = GameObject.FindGameObjectWithTag("BlueGemCounter").GetComponent<PickupCounterScript>();
			isBlue = true;
	  }

	  counter.AddPickup();
	  //add a new balloon?
	  if(counter.numberPickups >= gem.giftAfter && hasBalloon) {
	    //why the hasBalloon? Can´t i just pick one while falling? It would be nice, i could jump from a platform as well and get one


	    if( (isRed && hasRedGemGift) || (isBlue && hasBlueGemGift) || (isGreen && hasGreenGemGift) ) {
				//just destroy the gem object, as i already have this gift, and they don´t acumulate
	  			Destroy(gem.gameObject);
	    }
	    else {
			//i can only have one gift at the time
			BalloonScript existing = GetComponentInChildren<BalloonScript>();
			if(existing!=null) {
		  		Destroy(existing.gameObject);
			}

			Sprite newSprite = gem.GetBalloonGift();
	    	SpriteRenderer renderer = GetComponent<SpriteRenderer> ();
	    	renderer.sprite = newSprite;
	    	renderer.enabled = true;
	    	GetComponent<PolygonCollider2D> ().enabled = true;
	    	soundEffects.PlayPowerupSound();

			gem.PlayPowerupEffect();
	    	//add the balloon script
	    	counter.AddGiftGameObjectToPlayer(this);

	    	//now i need to reset it when i destroy the balloon script
	    	hasGreenGemGift = isGreen;
	    	hasRedGemGift = isRed;
	    	hasBlueGemGift = isBlue;

	    	ChangeAvatar(isRed,isGreen,isBlue);

	    	//destroy this gem
			Destroy(gem.gameObject);

	    }
		



	  }
	  else {
	    //just destroy it
		Destroy(gem.gameObject);
	  }



	}

	//Change the avatar background on the UI canvas
	void ChangeAvatar(bool red, bool green, bool blue) {
		GameObject avatar = GameObject.FindGameObjectWithTag("AvatarBalloon");
		if(avatar!=null) {
			UnityEngine.UI.Image image = avatar.GetComponent<UnityEngine.UI.Image>();
			if(red) {
				image.sprite = redBalloon;
			}
			else if(green) {
				image.sprite = greenBalloon;
			}
			else if(blue) {
				image.sprite = blueBalloon;
			}
			else {
				image.sprite = defaultBalloon;
			}
		}

   }

	//handle collision with ground
	public void HandleGroundCollision(bool groundVisible) {

		//HUGE FALL; WERE DEAD ANYWAY!!
		if (!groundVisible && IsPlayerAlive()) {
			KillPlayer();
			return;
		}
		//DisableGravityScale();
		//Normal landing stuff
		if(!hasLanded && IsPlayerAlive()) {

			hasLanded = true;

			//TODO not sure about these 2 here!!!
			//------------------------------------
			isLanding = false;
			isFalling = false;
			//------------------------------------

		    canMove = true;
		    //slow down movement
		    maxSpeed = maxSpeed / 2;
		    moveForce = moveForce / 2;

		    if(hasBalloon) {
		     ReleaseBalloon();
		    }
		    else if(hasParachute) {
		     ReleaseParachute();
		    }

			//play effects
			SpecialEffectsHelper fx = scripts.GetComponentInChildren<SpecialEffectsHelper> ();
			if (fx != null) {
				fx.PlayLandingEffect(transform.position);
				fx.PlayTouchdownEffect(transform.position);
			}
			PlayLandingSound();

			PlayFireworks ();

			//finish level
				
			int level = controller.currentLevel;
			int max = controller.numberOfLevels;




			controller.StopMusic();
			PlaySuccessSound ();
			controller.DisableGameElements(true);

			if(level < max) {
				//Only if i already opened the chest
				if(hero.HasOpenedChest())
					hero.StartMovingTowardsSign();
				//Play some animation
				controller.FinishLevel();

			}

		}//has landed && isJumping
		else if(jump) {
		  jump = false;
		}

	    
			

	}


	//get the reference for the Hero obj
	private HeroScript GetHero() {

	 GameObject heroObj = GameObject.FindGameObjectWithTag("Hero");
	 return heroObj.GetComponent<HeroScript>();
	}

//todo also play some nice sounds
	void PlayFireworks() {
		GameObject fireworks = GameObject.FindGameObjectWithTag ("Fireworks");
		if (fireworks != null) {
			//fireworks.SetActive(true);
			ActivateScript script = fireworks.GetComponent<ActivateScript>();
			if(script!=null) {
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
        PlayerPrefs.SetInt(GameConstants.NEXT_SCENE_KEY,controller.GetNextLevel());
        PlayerPrefs.Save();
		Application.LoadLevel(GameConstants.NEXT_SCENE_KEY);
		//Application.LoadLevel("Level" + controller.GetNextLevel())
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

	void ShowGameOverAndDestroy()
	{
		
		//yield return new WaitForSeconds(3);
		ShowGameOver(false);
		Destroy(gameObject);
	}

	//called by the engine when player dies
	void OnDestroy()
	{
	    //check needed, otherwise i might be just loading the next scene normally
	    if(isDead)
		  controller.DisableGameElements(false);
		
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

		UpdateHealthBar(0f);

		//take a screenshot of the level (and allow touch on this)
		controller.TakeScreenShot();

		SoundEffectsHelper sfx = scripts.GetComponentInChildren<SoundEffectsHelper> ();
		if (sfx != null) {
			sfx.PlayHitDeadSound();
		}

		//play effects
		SpecialEffectsHelper fx = scripts.GetComponentInChildren<SpecialEffectsHelper> ();
		if (fx != null) {
			fx.PlayJellySoulEffect(transform.position);
		}

		Invoke("ShowGameOverAndDestroy",1.2f);


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

	//disable player movement
	public void DisableMovement() {
		canMove = false;
	}

	public void PlayerStationary() {

	}

	public void SetOnLadder(bool onLadder) {

	}

	public void IncreaseCoins(int value) {

      if(parachuteCounter!=null) {
        parachuteCounter.AddPickup();
		if(parachuteCounter.numberPickups>=GameConstants.MINIMUM_COINS_FAILSAFE_PARACHUTE && !hasParachute) {
			//should be bought with virtual currency?
			AddFailsafeParachute();
			parachuteCounter.RemoveMultiplePickups(GameConstants.MINIMUM_COINS_FAILSAFE_PARACHUTE);
		}
      }
	}
	

	public bool IsMovingBackward() {
	 return true;
	}

	public bool IsMovingForward() {
	 return true;
	}

	//kills balloon!!!
	public void BurstBallon() {


		hasBalloon = false;
		isFalling = true;
		isLanding = false;

		//play effects
		SpecialEffectsHelper fx = scripts.GetComponentInChildren<SpecialEffectsHelper> ();
		if (fx != null) {
			fx.PlayJellyHitDeadEffect(transform.position);
		}

		//disable colliders and renderers
		ReleaseBalloon();

		if (hasParachute) {
			//enable parachute sprite, will reset the vars:
			//isFalling and isLanding
			EnableParachute ();
		} 
		if (hasUmbrella) {
			//enable parachute sprite, will reset the vars:
			//isFalling and isLanding
			EnableUmbrella();
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

	public bool PlayerHasUmbrella() {
		return hasUmbrella;
	}

	//disable the render and the collider
	void ReleaseBalloon() {
		gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		gameObject.GetComponent<PolygonCollider2D> ().enabled = false;
	}

	public void ReleaseStandingPlatform() {
		//GameObject parachute = GameObject.FindGameObjectWithTag("Parachute");
		landingPlatform.GetComponent<SpriteRenderer> ().enabled = false;
		landingPlatform.GetComponent<BoxCollider2D> ().enabled = false;
	}

	void ReleaseParachute() {
		GameObject parachute = GameObject.FindGameObjectWithTag("Parachute");
		parachute.GetComponent<SpriteRenderer> ().enabled = false;
		parachute.GetComponent<CircleCollider2D> ().enabled = false;
	}

	void ReleaseUmbrella() {
		GameObject umbrella = GameObject.FindGameObjectWithTag("Umbrella");
		umbrella.GetComponent<SpriteRenderer> ().enabled = false;
		umbrella.GetComponent<PolygonCollider2D> ().enabled = false;
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

	//kill the parachute!!!
	public void BurstParachute() {

		//play effects
		SpecialEffectsHelper fx = scripts.GetComponentInChildren<SpecialEffectsHelper> ();
		if (fx != null) {
			fx.PlayJellyHitDeadEffect(transform.position);
		}

		ReleaseParachute();
		//make him fall to the ground
		EnableGravityScale ();

		isFalling = true;
		isLanding = false;
		hasParachute = false;
	}

	public void BurstUmbrella() {

		//play effects
		SpecialEffectsHelper fx = scripts.GetComponentInChildren<SpecialEffectsHelper> ();
		if (fx != null) {
			fx.PlayJellyHitDeadEffect(transform.position);
		}

		ReleaseUmbrella();
		//make him fall to the ground
		EnableGravityScale ();

		isFalling = true;
		isLanding = false;
		hasUmbrella = false;
	}

	public void BurstStandingPlatform() {

		//play effects
		SpecialEffectsHelper fx = scripts.GetComponentInChildren<SpecialEffectsHelper> ();
		if (fx != null) {
			fx.PlayJellyHitDeadEffect(transform.position);
		}

		ReleaseStandingPlatform();
		//make him fall to the ground
		EnableGravityScale ();

		GetComponent<MoveScript>().enabled = false;

		isFalling = true;
		isLanding = false;
		hasParachute = false;
	}

	//called from the ground script
	public void MakePlayerFallToLand() {
		EnableGravityScale ();
		isFalling = true;
		isLanding = true;
		Debug.Log("Make player fall");
	}

	private void EnableParachute() {
		GameObject parachute = GameObject.FindGameObjectWithTag("Parachute");
		if(parachute!=null) {
			parachute.GetComponent<SpriteRenderer>().enabled = true;
			parachute.GetComponent<CircleCollider2D> ().enabled = true;
			failSafeUsed = true;
		}

		isFalling = false;
		isLanding = false;
	}

	private void EnableUmbrella() {
		GameObject umbrella = GameObject.FindGameObjectWithTag("Umbrella");
		if(umbrella!=null) {
			umbrella.GetComponent<SpriteRenderer>().enabled = true;
			umbrella.GetComponent<PolygonCollider2D> ().enabled = true;
			umbrellaUsed = true;
		}

		isFalling = false;
		isLanding = false;
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

