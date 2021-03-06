﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
	private bool isGrounded = false;

	public Sprite defaultBalloon;
	public Sprite balloonOne;

	//gems balloons
	public Sprite blueBalloon;//blue balloon
	public Sprite greenBalloon;//green balloon
	public Sprite redBalloon;//red balloon

	//the position where it was when started falling
	private Vector3 fallingStartPosition = new Vector3(0f,0f,0f);

	public Sprite colouredBalloon;

	//the user can move while standing on platform, along with her;
	private LandingPlatform landingPlatform;

	//the key is needed to open the chest
	private bool hasKey = false;

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
	private PickupCounterScript umbrellaCounter;
	private PickupCounterScript coinCounter;

	private bool hasBalloon = false;
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
		parachuteCounter = GameObject.FindGameObjectWithTag("ParachuteCounter").GetComponent<PickupCounterScript>();
		umbrellaCounter = GameObject.FindGameObjectWithTag("UmbrellaCounter").GetComponent<PickupCounterScript>();
		coinCounter = scripts.GetComponent<PickupCounterScript>();

		controller = scripts.GetComponent<GameControllerScript> ();
		//make sure we have this updated
		resolutionHelper = scripts.GetComponent<GUIResolutionHelper> ();
		resolutionHelper.CheckScreenResolution();

		hasParachute = false;
		failSafeUsed = false;
		umbrellaUsed = false;
		hasUmbrella = false;
		hasBalloon = false;

		platform = Application.platform;
		isMobilePlatform = (platform == RuntimePlatform.IPhonePlayer) || (platform == RuntimePlatform.Android);
		hasLanded = false;

		soundEffects = GetSoundEffects ();
		ground = GetGround ();

		hero = GetHero();
		hasKey = false;
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
	  //GetComponent<SpriteRenderer>().sprite = defaultBalloon;
	  //reset these as well
	  hasRedGemGift = false;
	  hasGreenGemGift = false;
	  hasBlueGemGift = false;

	  //hide the balloon
	  ReleaseBalloon();

	  //change the avatar as well
	  ChangeAvatar(false,false,false,true);

	  //on platform again
	  isStandingOnPlatform = true;
	  //is not falling, this to avoid calculate falling distance
	  isFalling = false;
	  hasBalloon = false;

	  //show it again and reset health bar as well
      ShowPlatform();

	}

	//the key to open the chest!
	public bool HasChestKey() {
	  return hasKey;
	}

	//check if we are on a platform
	public bool IsPlayerStandingOnPlatform() {

		return isStandingOnPlatform;
	}

	public void PlayerLandedOnPlatform() {

		isStandingOnPlatform = true;

		//hide the sprite and disable the collider
		if(hasBalloon) {
		  ReleaseBalloon();
		}

		//stop falling
		DisableGravityScale();
		//enable move script
		EnableMoveScript();
		//show the landing platform
		ShowPlatform();
		isFalling = false;
		//landingPlatform.ResetHealthBar();
		jump = false;
		//disable the colliders again
		DisableTerrainColliders();

		if(isStandingOnPlatform && !hasParachute/* && !hasBalloon*/) {
		  canMove = true;
		}

		if(isGrounded) {
		   //not anymore
			isGrounded = false;
		}

	}
	
	void Awake() {

		
	}

	public LandingPlatform GetLandingPlatform() {
	  return landingPlatform;
	}


	public void EnableGravityScale() {
		Debug.Log("EnableGravityScale");
		Rigidbody2D rig = GetComponent<Rigidbody2D>();
		if(rig!=null) {
			rig.gravityScale =1.0f;
			//canMove = false;
		}
	}

	public void DisableGravityScale() {
		Debug.Log("DisableGravityScale");
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
	
	/*void OnGUI() {
	
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
	}*/

	//TODO this should be an interface, somewhere
	string GetTranslationKey(string key) {
		return	translationManager.GetText(key);
	}

	public void KillPlayer() {
	  if(!isDead) {
		HandleLooseAllLifes();
		Handheld.Vibrate();
		if(isStandingOnPlatform) {
		   ParticleSystem [] particles = landingPlatform.gameObject.GetComponentsInChildren<ParticleSystem>();
		   foreach(ParticleSystem part in particles) {
		     if(part.tag!=null && part.CompareTag("PlatformSmoke")) {
		       part.Play(true);
		     }
		     else {
		      part.Stop(true);
		     }
		  }
		}
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

	  if(isStandingOnPlatform) {
	    MoveScript move = GetComponent<MoveScript>();
	    Debug.Log("SPEED Y: " + move.speed.y);
	    Debug.Log("GRAVITI SCALE: " + GetComponent<Rigidbody2D>().gravityScale);
	  }

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


				if(jumpingInput>0 && !jump && !isFalling && !hasBalloon && (isStandingOnPlatform || isGrounded) ) {

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
					if(input > 0 && !facingRight) {
						// ... flip the player.
						Flip();
						PlayMoveSound();
					// Otherwise if the input is moving the player left and the player is facing right...
					}else if(input < 0 && facingRight) {
						// ... flip the player.
						Flip();
						PlayMoveSound();
					}
					if(jump && !isFalling && (isStandingOnPlatform || isGrounded) ) {
	
					   PerformJump();

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
					if(speed > 0 && !facingRight) {
						// ... flip the player.
						Flip();
						PlayMoveSound();
					}
					// Otherwise if the input is moving the player left and the player is facing right...
					else if(speed < 0 && facingRight) {
						// ... flip the player.
						Flip();
						PlayMoveSound();
					}
				
				} 

		  }
				
		
	}

	public void PerformJump() {

		GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));

		DisableMoveScript();
		EnableGravityScale();
		if(isStandingOnPlatform) {
			HidePlatform();
		}

		isStandingOnPlatform = false;
	    isFalling = true;
		jump = false;
		isGrounded = false;

		EnableTerrainColliders();
	}

	//enables terrain colliders, so player can stand there
	public void EnableTerrainColliders() {

		Ferr2DT_PathTerrain[] terrains = GameObject.FindObjectsOfType<Ferr2DT_PathTerrain>();
		foreach(Ferr2DT_PathTerrain terrain in terrains) {
			List<Collider2D> colliders = terrain.GetAllTerrainColliders();
			if(colliders!=null) {
				foreach(Collider2D collider in colliders) {
			  		collider.enabled = true;
				}
			}

		}

	}

	//enables terrain colliders, so player can stand there
	public void DisableTerrainColliders() {

		Ferr2DT_PathTerrain[] terrains = GameObject.FindObjectsOfType<Ferr2DT_PathTerrain>();
		foreach(Ferr2DT_PathTerrain terrain in terrains) {
			List<Collider2D> colliders = terrain.GetAllTerrainColliders();
			if(colliders!=null) {
				foreach(Collider2D collider in colliders) {
			  		collider.enabled = false;
				}
			}

		}

	}

	//TODO check also the particles
	void HidePlatform() {
	 //record the position when this started, because if the jump is to high from the ground/platform
	 //it will splash and die
	 fallingStartPosition = GetHero().transform.position;

	 landingPlatform.GetComponent<SpriteRenderer>().enabled = false;
	 //landingPlatform.GetComponent<BoxCollider2D> ().enabled = false;
	 ParticleSystem[] particles = landingPlatform.GetComponentsInChildren<ParticleSystem>();
	 foreach(ParticleSystem part in particles) {
	   part.Stop(true);
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
	  //how much distance has passed between hide/show the platform (aka, how big was the fall?)
	  if(isFalling) {
		CheckFallingDistance();
	  }

	  landingPlatform.GetComponent<SpriteRenderer>().enabled = true;
	  //landingPlatform.GetComponent<BoxCollider2D> ().enabled = true;
	  ParticleSystem[] particles = landingPlatform.GetComponentsInChildren<ParticleSystem>();
	  foreach(ParticleSystem part in particles) {
	    if(part.tag==null || !part.CompareTag("PlatformSmoke")) {
	        //do not play the smoke particle "PlatformSmoke"
			part.Play();
	    }
	  }

		//reset the health bar
	  landingPlatform.ResetHealthBar();

	  ChangeAvatar(false,false,false,true);
	}

	void DisableMoveScript() {
	  MoveScript script =  GetComponent<MoveScript>();
	  script.enabled = false;
	}

	void EnableMoveScript() {
	  MoveScript script =  GetComponent<MoveScript>();
	  script.enabled = true;
	  //script.speed.y = 2.0f;
	  //script.direction.y=-1.0f;
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

	public void AddFailsafeUmbrella() {
		
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
		
		EnemyScript enemy = collisionObject.GetComponent<EnemyScript> ();
		//collided with enemy
		if ( (enemy != null && IsPlayerAlive()) && !IsPlayerFallingToLand() ) {
		    //do not handle grounded enemy collisions
			if(PlayerTouchedGround()){
			  return;
			}

		   //check if i have a gift ballooon undestructible
		   BalloonScript ball = GetComponentInChildren<BalloonScript>();
		   if(ball!=null && hasBalloon) {
		     //just handle the gift balloon stuff
		     HandleCollisionWhileUsingGiftBalloon(ball,enemy);
		     //return;
		   }

			PlayHitEffect(collisionObject.transform.position);

			//get reference for the hero
			HeroScript hero = GetHero();

				
			if (hasBalloon) {
	
				BurstBallon ();
				//BurnOrFlip(enemy,false);
			}
			else if (PlayerHasParachute ()) {
				BurstParachute ();
				//BurnOrFlip(enemy,false);
			}
			else if (PlayerHasUmbrella ()) {
				BurstUmbrella ();
				//BurnOrFlip(enemy,false);
			}
			else if(IsPlayerAlive()) {

				BurnOrFlip(enemy,true);

			}
	
				
		}//collider with the level chest key
		else if(collisionObject.GetComponent<GoldenKeyScript>()!=null) {
		  hasKey = true;
		  //move towards the key counter object
		  MoveTowardsScript moveTowards = collisionObject.GetComponent<MoveTowardsScript>();
		  if(moveTowards) {
		    moveTowards.StartMovingTowards(true);
		  }
		  //only destroy when reach final target
		  //Destroy(collisionObject);
		}
		//not on platform, maybe collided with terrain, but i do not care if i have a balloon
		else if(!isStandingOnPlatform){
			Ferr2DT_PathTerrain terrain = collisionObject.GetComponent<Ferr2DT_PathTerrain>();
			if(terrain!=null) {

			  Debug.Log("COLLIDED TERRAIN");
			  //check how much did i fall
			  CheckFallingDistance();
				 
			  isGrounded = true;
			  isFalling = false;
			  jump = false;

			  //NOTE the balloon does not collide with terrain, as this is a gift to the player
			  //but the parachute and the umbrellas burst!
			  if(hasParachute) {
			    BurstParachute();
			  }
			  else if(hasUmbrella) {
			   BurstUmbrella();
			  }

			  PlayDustEffect();


			  }


		}

		

	}

	void BurnOrFlip(EnemyScript enemy, bool dieAfter) {

		if(enemy.isBurner) {

			hero.BurnHero(dieAfter);
		}
		else {
			hero.BlinkWhenHit();
			hero.FlipUpsideDown();
		}

		//if is falling and i see the ground i don't die
		if(dieAfter && !IsGroundVisible() && !IsPlayerFalling()) {
			KillPlayer();
		}
	}

	//hit red effect
	void PlayHitEffect(Vector3 position) {
		SpecialEffectsHelper fx = scripts.GetComponentInChildren<SpecialEffectsHelper>();
		if(fx!=null) {
			fx.PlayHitDeadEffect(position);
		}
	}
	//check how much did i fall
	void CheckFallingDistance() {
		Vector3 currentPosition = GetHero().transform.position;
	  	float diff = currentPosition.y - fallingStartPosition.y;
	  	//he will die right after the smash sequence
		if(Mathf.Abs(diff) > 15f && IsPlayerAlive()) {
	    	PlayHitEffect(GetHero().transform.position);
			GetHero().SwitchToSmashedSprite();
	  	}
	}

	void PlayDustEffect() {

		GameObject dust = GameObject.FindGameObjectWithTag("DustPuff");
		if(dust) {
			ParticleSystem part = dust.GetComponent<ParticleSystem>();
			if(part!=null) {
			   part.Play();
			}

		}
	}

	public void OnCollisionExit(Collision collisionInfo) {
		if(!isStandingOnPlatform){
			Ferr2DT_PathTerrain terrain = collisionInfo.gameObject.GetComponent<Ferr2DT_PathTerrain>();
			if(terrain!=null) {

			  isGrounded = false;

			  GameObject dust = GameObject.FindGameObjectWithTag("DustPuff");
			    if(dust) {
			      ParticleSystem part = dust.GetComponent<ParticleSystem>();
			      if(part!=null) {
			       part.Stop(true);
				  }

			    }

			  }
		}
    }

    public bool IsPlayerGrounded() {
      return isGrounded;
    }

    public bool IsPlayerLanded() {
      return hasLanded;
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
       //only increase counter on reach target
	  //counter.AddPickup();
	 
	  //move towards the fake counter object
	  MoveTowardsScript moveTowards = gem.GetComponent<MoveTowardsScript>();
	  if(moveTowards!=null) {
	    moveTowards.StartMovingTowards(true);
	  }
	  //avoid any other unwanted collision
	  gem.DisableColliders();

	  //add a new gift balloon?
		if(counter.numberPickups > 0 && (counter.numberPickups % gem.giftAfter == 0) ) {
	    //why the hasBalloon? Can´t i just pick one while falling? It would be nice, i could jump from a platform as well and get one

	   

	    //TODO maybe i should keep track of current platform health, and then restore it back???
	    //but maybe i was not standing on the platform, and i was on the air??

	    if(isStandingOnPlatform) {
			//stop decreasing counter
			landingPlatform.StopCountdown();
	    }
		//die the platform
	    isStandingOnPlatform = false;
	    //call hide anyway
		HidePlatform();


	    if( (isRed && hasRedGemGift) || (isBlue && hasBlueGemGift) || (isGreen && hasGreenGemGift) ) {
				//just destroy the gem object, as i already have this gift, and they don´t acumulate
	  			if(moveTowards==null)
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

			//now we have a balloon
			hasBalloon = true;

				//if is grounded than disable the terrain collider, so he can start falling again
			//also disable gravity scale, and enable movement
			if(isGrounded || hasBalloon) {

				RestartDescent();
			}

	    	soundEffects.PlayPowerupSound();

			gem.PlayPowerupEffect();
	    	//add the balloon script, will restart the health counter too
	    	counter.AddGiftGameObjectToPlayer(this);

	    	//now i need to reset it when i destroy the balloon script
	    	hasGreenGemGift = isGreen;
	    	hasRedGemGift = isRed;
	    	hasBlueGemGift = isBlue;

	    	ChangeAvatar(isRed,isGreen,isBlue,false);

	    	//destroy this gem
			if(moveTowards==null)
			   Destroy(gem.gameObject);

	    }
		



	  }
	  else {
	    //just destroy it
		if(moveTowards==null)
			Destroy(gem.gameObject);
	  }



	}

	void RestartDescent() {
		//to enable movement script
		DisableGravityScale();
		//do not collide with terrain while in balloon
		DisableTerrainColliders();
		//enable movement
		EnableMoveScript();
		//not anymore
		isGrounded = false;
	}

	//Change the avatar background on the UI canvas
	void ChangeAvatar(bool red, bool green, bool blue, bool platform) {
		GameObject avatarBalloon = GameObject.FindGameObjectWithTag("AvatarBalloon");
		GameObject avatarPlatform = GameObject.FindGameObjectWithTag("AvatarPlatform");

		UnityEngine.UI.Image balloonImage = avatarBalloon.GetComponent<UnityEngine.UI.Image>();
		UnityEngine.UI.Image platformImage = avatarPlatform.GetComponent<UnityEngine.UI.Image>();

		if(platform) {
			balloonImage.enabled = false;
			platformImage.sprite = landingPlatform.GetComponent<SpriteRenderer>().sprite;
			platformImage.enabled = true;
		}
		else {
			platformImage.enabled = false;

			if(red) {
				balloonImage.sprite = redBalloon;
			}
			else if(green) {
				balloonImage.sprite = greenBalloon;
			}
			else if(blue) {
				balloonImage.sprite = blueBalloon;
			}
			else {
				//the landing platform sprite
				platformImage.sprite = landingPlatform.GetComponent<SpriteRenderer>().sprite;
			}

			balloonImage.enabled = true;
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
		StartCoroutine(PrepareForGameOver());
	}

	IEnumerator PrepareForGameOver()
	{
		yield return new WaitForSeconds(2);
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
			fx.PlaySoulEffect(transform.position);
		}

		Invoke("ShowGameOverAndDestroy",1.2f);


	}

	//TODO CHECK IF GOING UP OR DOWN
	//http://answers.unity3d.com/questions/414146/how-to-detect-object-go-up-or-down.html




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
		

		if(!isStandingOnPlatform && isGrounded)
		   PlayDustEffect();
	}

	public void PlayMoveSound() {
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
		moveForward = false;
	    moveBackward =  false;
	    moving = false;
	}

	public void SetOnLadder(bool onLadder) {

	}

	public void IncreaseCoins(int value) {

		if(coinCounter!=null) {
		  coinCounter.AddPickup();
		}

		if(coinCounter.numberPickups >= GameConstants.MINIMUM_COINS_FAILSAFE_PARACHUTE) {

			if(parachuteCounter!=null) {
	        	parachuteCounter.AddPickup();
				if(parachuteCounter.numberPickups > 0  && (parachuteCounter.numberPickups % GameConstants.MINIMUM_COINS_FAILSAFE_PARACHUTE == 0) && !hasParachute) {
					//should be bought with virtual currency?
					AddFailsafeParachute();
					parachuteCounter.RemoveMultiplePickups(GameConstants.MINIMUM_COINS_FAILSAFE_PARACHUTE);
				}
			}
		}

		if(coinCounter.numberPickups >= GameConstants.MINIMUM_COINS_FAILSAFE_PARACHUTE)  {
			if(umbrellaCounter!=null) {
				umbrellaCounter.AddPickup();
				if(umbrellaCounter.numberPickups > 0 && (umbrellaCounter.numberPickups % GameConstants.MINIMUM_COINS_FAILSAFE_UMBRELLA == 0)  && !hasUmbrella) {
					//should be bought with virtual currency?
					AddFailsafeUmbrella();
					umbrellaCounter.RemoveMultiplePickups(GameConstants.MINIMUM_COINS_FAILSAFE_UMBRELLA);
				}
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
		PlayHitEffect(transform.position);

		//disable colliders and renderers
		ReleaseBalloon();


		if(landingPlatform.IsCountdownPaused()) {
		  		//landingPlatform.ResetHealthBar();
		  	ShowPlatform();
		}
		else {
		    
			if (hasParachute && !isGrounded) {
				//enable parachute sprite, will reset the vars:
				//isFalling and isLanding
				EnableParachute ();
				//DisableTerrainColliders();
			} 
			else if (hasUmbrella && !isGrounded) {
				//enable parachute sprite, will reset the vars:
				//isFalling and isLanding
				EnableUmbrella();
				//DisableTerrainColliders();
			} 
			else {
				//just make him fall to the ground
				EnableGravityScale ();
			}

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
		hasBalloon = false;
	}

	//public void ReleaseStandingPlatform() {
		//GameObject parachute = GameObject.FindGameObjectWithTag("Parachute");
	//	landingPlatform.GetComponent<SpriteRenderer> ().enabled = false;
	//	landingPlatform.GetComponent<BoxCollider2D> ().enabled = false;
	//}

	void ReleaseParachute() {
		GameObject parachute = GameObject.FindGameObjectWithTag("Parachute");
		parachute.GetComponent<SpriteRenderer> ().enabled = false;
		parachute.GetComponent<CircleCollider2D> ().enabled = false;
		hasParachute = false;
	}

	void ReleaseUmbrella() {
		GameObject umbrella = GameObject.FindGameObjectWithTag("Umbrella");
		umbrella.GetComponent<SpriteRenderer> ().enabled = false;
		umbrella.GetComponent<PolygonCollider2D> ().enabled = false;
		hasUmbrella = false;
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
			fx.PlayHitDeadEffect(transform.position);
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
			fx.PlayHitDeadEffect(transform.position);
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
		PlayHitEffect(transform.position);

		if(isStandingOnPlatform) {
			HidePlatform();
		}

		if (hasParachute) {
			//enable parachute sprite, will reset the vars:
			//isFalling and isLanding
			EnableParachute ();
		} 
		else if (hasUmbrella) {
			//enable parachute sprite, will reset the vars:
			//isFalling and isLanding
			EnableUmbrella();
		} 
		else {
			//just make him fall to the ground
			EnableGravityScale ();

			//disable movement TODO check DisableMovement implementation
			GetComponent<MoveScript>().enabled = false;

			isFalling = true;
			isLanding = false;
			hasParachute = false;
			hasUmbrella = false;

		}
		//in any case i am not on the platform
		isStandingOnPlatform = false;
		//enable terrain collisions in any case
		EnableTerrainColliders();



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

		hasParachute = true;
		isFalling = false;
		isLanding = false;


		//&& hasParachute && !failSafeUsed
	}

	private void EnableUmbrella() {
		GameObject umbrella = GameObject.FindGameObjectWithTag("Umbrella");
		if(umbrella!=null) {
			umbrella.GetComponent<SpriteRenderer>().enabled = true;
			umbrella.GetComponent<PolygonCollider2D> ().enabled = true;
			umbrellaUsed = true;
		}

		hasUmbrella = true;
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

