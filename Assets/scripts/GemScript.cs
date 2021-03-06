﻿using UnityEngine;
using System.Collections;

public class GemScript : MonoBehaviour {

    //the player balloon will get this sprite
    public Sprite giftBaloon;
    public bool isRed = false;
	public bool isGreen = false;
	public bool isBlue = false;

	public int points = 10;

	private bool isVisible = false;

	//give a gift after n pickups
	public int giftAfter = 5;

	//this is to avoid calling the collision handler twice
	private bool handledThis = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnBecameVisible() {
	if(isVisible)
	  return;

		isVisible = true;
	}
	
	void OnBecameInvisible() {
	 if(!isVisible)
	   return;

		isVisible = false;
		Destroy(gameObject);
	}

	public Sprite GetBalloonGift() {

	   return giftBaloon;
	}

	public void PlayPowerupEffect() {

	  GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
	  if(scripts!=null) {
	    SpecialEffectsHelper sfx = scripts.GetComponentInChildren<SpecialEffectsHelper>();

		GameObject player = GameObject.FindGameObjectWithTag("Player");
		float playerPos = player.transform.position.y;

	    if(isRed) {
	      sfx.PlayPowerupRed(new Vector3(transform.position.x,playerPos + 20,-5f));
	    }
		else if(isGreen) {
		  sfx.PlayPowerupGreen(new Vector3(transform.position.x,playerPos + 20,-5f));
	    }
		else {
		  sfx.PlayPowerupBlue(new Vector3(transform.position.x,playerPos + 20,-5f));
	    }
	  }
	}

	void OnTriggerEnter2D(Collider2D otherCollider)
	{

	  if(!handledThis) {
		PlayerScript player = otherCollider.GetComponent<PlayerScript> ();
		if(player!=null) {
		 //gem collided with player
		  if(player!=null && player.IsPlayerAlive() && !player.PlayerTouchedGround()) {
			player.HandleGemCollision(this);
			PlayPickupSound();
			handledThis = true;

		  }
		}
		else {
		    //could be a hero, which is basically same as player, but for the character only
			HeroScript hero = otherCollider.GetComponent<HeroScript> ();
			if(hero!=null) {
				player = hero.GetPlayer(); //do not handle it if i already touched ground
				if(player!=null && player.IsPlayerAlive() && !player.PlayerTouchedGround()) {
				  player.HandleGemCollision(this);
				  PlayPickupSound();
				  handledThis = true;
				}
			}
		}
	  }

		
	}

	//play a sound
	void PlayPickupSound() {
	  GameObject scripts = GameObject.FindGameObjectWithTag("Scripts");
	 
	  if(scripts!=null) {
		SoundEffectsHelper sfx = scripts.GetComponentInChildren<SoundEffectsHelper>();
		if (sfx != null) {
		   sfx.PlayPickupCoinSound();
	    }

		SpecialEffectsHelper fx = scripts.GetComponentInChildren<SpecialEffectsHelper> ();
		if (fx != null) {
		    if(isRed) {
		        //make sure we show it in front of the other stuff
				fx.Play5PointsEffect(new Vector3(transform.position.x,transform.position.y,-2f));
		    }
			else {
				fx.Play10PointsEffect(new Vector3(transform.position.x,transform.position.y,-2f));
			}
		}

		GameControllerScript gameEngine = scripts.GetComponent<GameControllerScript>();
		if(gameEngine!=null) {
			gameEngine.IncreaseScoreBy(points);
		}
	}//if scripts!=null

   }
   void OnDestroy() {

     Debug.Log("Destroying gem");
	 PickupCounterScript counter;

	  if(isRed) {
			counter = GameObject.FindGameObjectWithTag("RedGemCounter").GetComponent<PickupCounterScript>();
	  }
	  else if(isGreen) {
			counter = GameObject.FindGameObjectWithTag("GreenGemCounter").GetComponent<PickupCounterScript>();
	  }
	  else {
	  		//blue
			counter = GameObject.FindGameObjectWithTag("BlueGemCounter").GetComponent<PickupCounterScript>();
	  }
	  if(counter!=null)
	    counter.AddPickup();
   }

	public void DisableColliders() {
	 GetComponent<BoxCollider2D>().enabled = false;
	}
}
