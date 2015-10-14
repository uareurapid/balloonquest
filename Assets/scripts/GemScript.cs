using UnityEngine;
using System.Collections;

public class GemScript : MonoBehaviour {

    //the player balloon will get this sprite
    public Sprite giftBaloon;
    public bool isRed = false;
	public bool isGreen = false;
	public bool isBlue = false;

	public int points = 10;


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

	public Sprite GetBalloonGift() {

	   return giftBaloon;
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


}
