using UnityEngine;
using System.Collections;

public class LandingPlatform : MonoBehaviour {

    public bool isDetachable = false;
	PlayerScript player;
	BoxCollider2D colliderBox;

	private bool restoreCurrentHealth = false;
	private bool pauseCounter = false;

	public float platformLifetime = 15f; //15 seconds
	private float currentPlatformLifetime = 15f; //15 seconds
	private bool startDestroying = false;

	HealthBar healthbar = null;
	// Use this for initialization
	void Start () {

	 GameObject pl = GameObject.FindGameObjectWithTag("Player");
	 if(pl!=null) {
		player = pl.GetComponent<PlayerScript>();
	 }

	 colliderBox = GetComponent<BoxCollider2D>();
	 startDestroying = false;

	 currentPlatformLifetime = platformLifetime;

		GameObject bar = GameObject.FindGameObjectWithTag("HealthBar");
		if(bar!=null) {
		   healthbar = bar.GetComponent<HealthBar>();
		}

	}

	//restore health
	public void RestoreCurrentHealth() {
	//start counting again
	 pauseCounter = false;
	 startDestroying = true;
	}

	//stop counter
	public void StopCountdown() {
	  pauseCounter = true;
	}

	public bool IsCountdownPaused() {
	  return pauseCounter;
	}
	
	// Update is called once per frame
	void Update () {

	  if(colliderBox!=null && player!=null) {
	    colliderBox.enabled = true;// (player.IsPlayerFalling() && !player.PlayerHasBalloon());
	  }

	  if(startDestroying) {
		startDestroying = false;//avoid call this part again
		InvokeRepeating("DecreaseSecondsCounter",1.0f,1.0f);
	  }
	}

	public void DecreaseSecondsCounter() {
	  if(pauseCounter) {
	    return;//do nothing
	  }
	  currentPlatformLifetime-=1f;
		if(currentPlatformLifetime < 0f) {
			currentPlatformLifetime = 0f;
			CancelInvoke("DecreaseSecondsCounter");
			player.BurstStandingPlatform();
	  }

		UpdateHealthBar(currentPlatformLifetime);
	  
	}

	public void UpdateHealthBar(float currentValue) {

		healthbar.SetCurrentHealth(currentValue);

	}

	public void SetHealthBar() {
		healthbar.SetMaxHealth(platformLifetime);
	}

	public void ResetHealthBar() {
	  CancelInvoke("DecreaseSecondsCounter");
	  currentPlatformLifetime = platformLifetime;
	  StartCountdownDestruction();
	}

	//only for time
	public void StartCountdownDestruction() {
	  if(startDestroying)
	    return;

	  startDestroying = true;
	}
	
	void OnCollisionEnter2D(Collision2D collision)
	{
		  //go down
		  HandleCollision(collision.gameObject);
		
	}

	private void HandleCollision(GameObject collisionObject) {

		HeroScript playerObj = collisionObject.GetComponent<HeroScript>();
		//if i'm already on a platform then ignore the collision
		if(playerObj!=null && player!=null && !player.IsPlayerStandingOnPlatform()) {

		    //assign the player movement with the same direction of the platform mevement
		    MovingPlatformScript moving = GetComponent<MovingPlatformScript>();
		    if(moving!=null) {
				MoveScript movement = player.GetComponent<MoveScript>();
		    	if(moving.startGoingDown) {
		    	  //down movement
		    	  movement.direction.y = -1f;
		    	}
		    	else {
		    	  movement.direction.y = 1f;
		    	}
		    }
		    //destroy the platform
			Destroy(gameObject);
			player.PlayerLandedOnPlatform();
		}

	}

	void OnTriggerEnter2D(Collider2D collision) {
		
		  HandleCollision(collision.gameObject);
		
	}



}
